import {
    HubConnection,
    HubConnectionBuilder,
    LogLevel,
    HttpTransportType,
    HubConnectionState
} from '@microsoft/signalr';
import { useConnectionStore } from '../stores/connectionStore';

class SignalRService {
    private connection: HubConnection | null = null;
    private connectionPromise: Promise<void> | null = null;
    private reconnectAttempt = 0;
    private gameSubscriptions: Map<number, () => void> = new Map();
    private gameCallbacks: Map<number, (gameId: number) => void> = new Map();

    private async ensureConnection(): Promise<void> {
        if (this.connection?.state === HubConnectionState.Connected) {
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
                    transport: HttpTransportType.WebSockets,
                    headers: { 'Cache-Control': 'no-cache' }
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
                // Reload page after reconnection to ensure clean state
                window.location.reload();
            });

            // Register base event handlers before starting connection
            this.connection.on('GameChanged', (gameId: number) => {
                console.log('Received GameChanged event:', gameId);
                console.log('Available callbacks:', Array.from(this.gameCallbacks.entries()));
                const callback = this.gameCallbacks.get(gameId);
                console.log('Found callback for game:', gameId, callback ? 'exists' : 'not found');
                if (callback) {
                    console.log('Executing callback for game:', gameId);
                    callback(gameId);
                }
            });

            await this.connection.start();
            
            // Wait for connection ID to be available
            let retries = 0;
            while (!this.connection.connectionId && retries < 5) {
                await new Promise(resolve => setTimeout(resolve, 1000));
                retries++;
                console.log('Waiting for connection ID, attempt:', retries);
            }

            if (!this.connection.connectionId) {
                throw new Error('Failed to get connection ID after 5 seconds');
            }

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
            
            if (!this.connection?.connectionId) {
                throw new Error('No connection ID available');
            }

            // Store game subscription for reconnection
            this.gameSubscriptions.set(gameId, () => this.handleReconnection(gameId));

            console.info('Attempting to join game:', {
                gameId,
                connectionId: this.connection.connectionId,
                state: this.connection.state
            });
            await this.connection.invoke('JoinGame', gameId);
            console.info('Successfully joined game:', {
                gameId,
                connectionId: this.connection.connectionId
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
            if (this.connection?.state !== HubConnectionState.Connected) {
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
            return () => { };
        }

        // Store callback for resubscription
        const gameId = this.gameSubscriptions.keys().next().value;
        console.log('Storing callback for game:', gameId);
        this.gameCallbacks.set(gameId, callback);

        // No need to register handlers here as they're already registered in startConnection
        console.info('Subscribed to game changes');

        return () => {
            console.log('Removing callback for game:', gameId);
            this.gameCallbacks.delete(gameId);
            console.info('Unsubscribed from game changes');
        };
    }

    private async handleReconnection(gameId: number) {
        try {
            await this.connection?.invoke('JoinGame', gameId);
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