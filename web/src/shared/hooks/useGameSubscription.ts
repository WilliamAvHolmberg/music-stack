import { useEffect, useRef } from 'react';
import { signalRService } from '../services/signalr';
import { useConnectionStore } from '../stores/connectionStore';

interface UseGameSubscriptionProps {
    gameId: number;
    onGameChanged: () => void;
}

export function useGameSubscription({ gameId, onGameChanged }: UseGameSubscriptionProps) {
    const subscriptionRef = useRef(false);

    useEffect(() => {
        if (subscriptionRef.current) return;
        subscriptionRef.current = true;

        useConnectionStore.getState().addSubscriber();
        const setup = async () => {
            try {
                await signalRService.joinGame(gameId);
            } catch (error) {
                console.error('Failed to join game:', error);
            }
        };

        const cleanup = signalRService.onGameChanged((changedGameId) => {
            if (changedGameId === gameId) {
                onGameChanged();
            }
        });

        setup();

        return () => {
            subscriptionRef.current = false;
            cleanup();
            useConnectionStore.getState().removeSubscriber();
            signalRService.leaveGame(gameId).catch(console.error);
        };
    }, [gameId]);
} 