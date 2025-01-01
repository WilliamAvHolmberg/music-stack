import { cn } from '@/shared/utils/utils';
import { RoundStatus, RoundType } from '../types';
import type { RoundResponseDTO } from '@/shared/api/models';

interface RoundOverviewProps {
    rounds?: RoundResponseDTO[];
    currentRoundIndex: number;
}

export function RoundOverview({ rounds, currentRoundIndex }: RoundOverviewProps) {
    if (!rounds?.length) return null;

    return (
        <div className="space-y-2 bg-white/5 rounded-lg p-4">
            <h3 className="text-lg font-semibold mb-4">Round Overview</h3>
            {rounds.map((round, index) => (
                <div key={round.id}
                    className={cn(
                        "p-3 rounded-lg border transition-all",
                        "hover:bg-white/5",
                        index === currentRoundIndex && "border-green-500 bg-green-500/10",
                        round.status === RoundStatus.Completed && "border-blue-500 bg-blue-500/10",
                        round.status === RoundStatus.NotStarted && "border-gray-700 bg-white/5"
                    )}
                >
                    <div className="flex items-center justify-between">
                        <div>
                            <div className="font-medium">
                                {round.title || `Round ${index + 1}`}
                            </div>
                            <div className="text-sm text-muted-foreground">
                                {RoundType[round.type]} • {round.timeInMinutes} min
                                {round.items && ` • ${round.items.length} items`}
                            </div>
                        </div>
                        <span className={cn(
                            "px-2 py-1 rounded text-xs",
                            round.status === RoundStatus.NotStarted && "bg-gray-700",
                            round.status === RoundStatus.InProgress && "bg-green-500",
                            round.status === RoundStatus.Completed && "bg-blue-500",
                        )}>
                            {RoundStatus[round.status]}
                        </span>
                    </div>
                </div>
            ))}
        </div>
    );
} 