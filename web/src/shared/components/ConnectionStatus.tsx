import { useConnectionStore } from '../stores/connectionStore';

export function ConnectionStatus() {
    const { status, error, activeSubscribers } = useConnectionStore();

    if (status === 'connected' || activeSubscribers === 0) return null;

    const getBgColor = () => {
        switch (status) {
            case 'connecting': return 'bg-yellow-500';
            case 'disconnected': return 'bg-orange-500';
            case 'error': return 'bg-red-500';
            default: return 'bg-gray-500';
        }
    };

    return (
        <div className={`absolute right-4 z-50 ${getBgColor()} text-white px-4 py-2 rounded-full shadow-lg flex items-center gap-2 my-48`}>
            {status === 'connecting' && (
                <div className="animate-spin h-4 w-4 border-2 border-white rounded-full border-t-transparent" />
            )}
            <span className="font-semibold capitalize">{status}</span>
            {error && <span className="text-sm opacity-75">({error})</span>}
        </div>
    );
} 