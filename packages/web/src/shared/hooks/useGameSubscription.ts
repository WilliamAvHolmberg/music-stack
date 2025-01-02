import { useEffect, useRef, useCallback } from 'react';
import { signalRService } from '../services/signalr';
import { useConnectionStore } from '../stores/connectionStore';

interface UseGameSubscriptionProps {
    gameId: number;
    onGameChanged: () => void;
}

export function useGameSubscription({ gameId, onGameChanged }: UseGameSubscriptionProps) {
    const subscriptionRef = useRef(false);
    
    // Memoize the callback to prevent unnecessary re-subscriptions
    const handleGameChanged = useCallback((changedGameId: number) => {
        console.log('Game change callback received:', { changedGameId, expectedGameId: gameId });
        if (changedGameId === gameId) {
            onGameChanged();
        }
    }, [gameId, onGameChanged]);

    useEffect(() => {
        // Prevent duplicate subscriptions
        if (subscriptionRef.current) {
            console.log('Subscription already exists, skipping...');
            return;
        }

        console.log('Setting up game subscription for game:', gameId);
        subscriptionRef.current = true;
        let isSubscribed = true;

        useConnectionStore.getState().addSubscriber();
        
        const setup = async () => {
            if (!isSubscribed) return;
            
            try {
                await signalRService.joinGame(gameId);
                console.log('Successfully joined game, setting up change handler');
                
                // Only set up the handler if we're still subscribed
                if (isSubscribed) {
                    const cleanup = signalRService.onGameChanged(handleGameChanged);
                    return cleanup;
                }
            } catch (error) {
                console.error('Failed to join game:', error);
            }
        };

        // Set up the subscription
        setup();

        // Cleanup function
        return () => {
            console.log('Cleaning up game subscription for game:', gameId);
            isSubscribed = false;
            subscriptionRef.current = false;
            useConnectionStore.getState().removeSubscriber();
            signalRService.leaveGame(gameId).catch(console.error);
        };
    }, [gameId, handleGameChanged]); // Add handleGameChanged to dependencies
} 