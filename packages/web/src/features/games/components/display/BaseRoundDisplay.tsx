import { cn } from '@/shared/utils/utils';

interface BaseRoundDisplayProps {
    isRevealed: boolean;
    children: React.ReactNode;
}

export function BaseRoundDisplay({ isRevealed, children }: BaseRoundDisplayProps) {
    return (
        <div className="text-center space-y-8">
            <div className={cn(
                "transition-all duration-500",
                isRevealed && "animate-in fade-in-0 slide-in-from-bottom-4"
            )}>
                {children}
            </div>
        </div>
    );
} 