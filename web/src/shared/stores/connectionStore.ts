import { create } from 'zustand';

type ConnectionStatus = 'connecting' | 'connected' | 'disconnected' | 'error';

interface ConnectionState {
    status: ConnectionStatus;
    error?: string;
    activeSubscribers: number;
    setStatus: (status: ConnectionStatus, error?: string) => void;
    addSubscriber: () => void;
    removeSubscriber: () => void;
}

export const useConnectionStore = create<ConnectionState>((set) => ({
    status: 'disconnected',
    error: undefined,
    activeSubscribers: 0,
    setStatus: (status, error) => set({ status, error }),
    addSubscriber: () => set((state) => ({ activeSubscribers: state.activeSubscribers + 1 })),
    removeSubscriber: () => set((state) => ({ activeSubscribers: Math.max(0, state.activeSubscribers - 1) }))
})); 