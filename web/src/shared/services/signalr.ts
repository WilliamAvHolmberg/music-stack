import { 
    HubConnection, 
    HubConnectionBuilder, 
    LogLevel,
    HttpTransportType 
} from '@microsoft/signalr';
import { useConnectionStore } from '../stores/connectionStore';

class SignalRService {
    private connection: HubConnection | null = null;
    private connectionPromise: Promise<void> | null = null;
    private reconnectAttempt = 0;
    private gameSubscriptions: Map<number, () => void> = new Map();
    private gameCallbacks: Map<number, (gameId: number) => void> = new Map();

    private async ensureConnection(): Promise<void> {
        if (this.connection?.state === 'Connected') {
            return;
        }

        if (!this.connectionPromise) {
            this.connectionPromise = this.startConnection();
        }

        return this.connectionPromise;
    }

    private async startConnection(): Promise<void> {
        useConnectionStore.getState().setStatus('connecting');
        try {
            this.connection = new HubConnectionBuilder()
                .withUrl('/api/gamehub', {
                    withCredentials: true,
                    transport: HttpTransportType.WebSockets
                })
                .withAutomaticReconnect({
                    nextRetryDelayInMilliseconds: (retryContext) => {
                        this.reconnectAttempt = retryContext.previousRetryCount;
                        console.warn(
                            `SignalR Retry Attempt ${this.reconnectAttempt}:`,
                            retryContext.retryReason
                        );
                        // Return null to stop retrying
                        if (this.reconnectAttempt >= 5) return null;
                        // Exponential backoff: 0.5s, 1s, 2s, 4s, 8s
                        return Math.min(1000 * Math.pow(2, this.reconnectAttempt), 8000);
                    }
                })
                .configureLogging(LogLevel.Debug)
                .build();

            // Connection lifecycle logging
            this.connection.onclose((error) => {
                useConnectionStore.getState().setStatus('disconnected');
                if (this.reconnectAttempt >= 5) {
                    useConnectionStore.getState().setStatus('error', 'Connection lost - max retries exceeded');
                }
                console.error('SignalR Connection closed:', {
                    error,
                    state: this.connection?.state,
                    lastReconnectAttempt: this.reconnectAttempt
                });
            });
            
            this.connection.onreconnecting((error) => {
                useConnectionStore.getState().setStatus('connecting', 'Attempting to reconnect...');
                console.warn('SignalR Reconnecting:', {
                    error,
                    attempt: this.reconnectAttempt,
                    state: this.connection?.state
                });
            });
            
            this.connection.onreconnected((connectionId) => {
                useConnectionStore.getState().setStatus('connected');
                console.info('SignalR Reconnected:', {
                    connectionId,
                    attempts: this.reconnectAttempt,
                    state: this.connection?.state
                });
                this.reconnectAttempt = 0;
                // Simple solution: reload page after reconnection
                window.location.reload();
            });

            // Add error handler for connection errors
            this.connection.on('error', (error: Error) => {
                console.error('SignalR Error:', error);
                useConnectionStore.getState().setStatus('error', error.message || 'Connection error occurred');
            });

            await this.connection.start();
            useConnectionStore.getState().setStatus('connected');
            console.info('SignalR Connected successfully:', {
                connectionId: this.connection.connectionId,
                state: this.connection.state
            });
            this.connectionPromise = null;
        } catch (err) {
            useConnectionStore.getState().setStatus('error', (err as Error).message);
            console.error('SignalR Connection failed:', {
                error: err,
                state: this.connection?.state,
                connectionId: this.connection?.connectionId
            });
            this.connectionPromise = null;
            throw err;
        }
    }

    public async joinGame(gameId: number): Promise<void> {
        try {
            await this.ensureConnection();
            // Store game subscription for reconnection
            this.gameSubscriptions.set(gameId, () => this.handleReconnection(gameId));

            console.info('Attempting to join game:', {
                gameId,
                connectionId: this.connection?.connectionId,
                state: this.connection?.state
            });
            await this.connection?.invoke('JoinGame', gameId);
            console.info('Successfully joined game:', {
                gameId,
                connectionId: this.connection?.connectionId
            });
        } catch (err) {
            console.error('Failed to join game:', {
                gameId,
                error: err,
                state: this.connection?.state,
                connectionId: this.connection?.connectionId
            });
            throw err;
        }
    }

    public async leaveGame(gameId: number): Promise<void> {
        try {
            if (this.connection?.state !== 'Connected') {
                console.warn('Cannot leave game - not connected:', {
                    gameId,
                    state: this.connection?.state
                });
                return;
            }
            this.gameSubscriptions.delete(gameId);
            this.gameCallbacks.delete(gameId);
            await this.connection.invoke('LeaveGame', gameId);
            console.info('Successfully left game:', {
                gameId,
                connectionId: this.connection?.connectionId
            });
        } catch (err) {
            console.error('Failed to leave game:', {
                gameId,
                error: err,
                state: this.connection?.state
            });
            throw err;
        }
    }

    public onGameChanged(callback: (gameId: number) => void): () => void {
        if (!this.connection) {
            console.warn('Attempting to subscribe before connection exists');
            return () => {};
        }

        // Store callback for resubscription
        const gameId = parseInt(this.connection.connectionId?.split('_')[1] || '0');
        this.gameCallbacks.set(gameId, callback);

        this.connection?.on('gamechanged', callback);
        console.info('Subscribed to game changes');

        return () => {
            this.connection?.off('gamechanged', callback);
            this.gameCallbacks.delete(gameId);
            console.info('Unsubscribed from game changes');
        };
    }

    private async handleReconnection(gameId: number) {
        try {
            await this.connection?.invoke('JoinGame', gameId);
            // Re-register the event handler
            const callback = this.gameCallbacks.get(gameId);
            if (callback) {
                this.connection?.off('gamechanged', callback); // Remove old handler
                this.connection?.on('gamechanged', callback);  // Add new handler
            }
            console.info('Resubscribed to game after reconnection:', gameId);
        } catch (error) {
            console.error('Failed to resubscribe after reconnection:', error);
        }
    }

    public async disconnect(): Promise<void> {
        if (this.connection) {
            await this.connection.stop();
            this.connection = null;
        }
    }
}

export const signalRService = new SignalRService(); 