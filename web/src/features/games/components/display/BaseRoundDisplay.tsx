import { cn } from '@/shared/utils/utils';

interface BaseRoundDisplayProps {
    points: number;
    isRevealed: boolean;
    children: React.ReactNode;
}

export function BaseRoundDisplay({ points, isRevealed, children }: BaseRoundDisplayProps) {
    return (
        <div className="text-center space-y-8">
            {/* <div className="text-2xl font-bold text-yellow-400">
                {points} points
            </div> */}
            <div className={cn(
                "transition-all duration-500",
                isRevealed && "animate-in fade-in-0 slide-in-from-bottom-4"
            )}>
                {children}
            </div>
        </div>
    );
} 