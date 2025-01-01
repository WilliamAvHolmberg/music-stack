import { AnimatedReveal } from './AnimatedReveal';
import { BaseRoundDisplay } from './BaseRoundDisplay';

interface FirstLineDisplayProps {
    title: string;
    artist: string;
    points: number;
    isRevealed: boolean;
    extraInfo: string;
    year: number;
}

export function FirstLineDisplay({ title, artist, isRevealed, extraInfo }: FirstLineDisplayProps) {
    return (
        <BaseRoundDisplay isRevealed={isRevealed}>
            {isRevealed ? (
                <div className="w-full max-w-4xl mx-auto">
                    <AnimatedReveal
                        title={title}
                        artist={artist}
                        isRevealed={isRevealed}
                    />
                </div>
            ) : (
                <div className="text-3xl font-bold text-blue-300">
                    {extraInfo}...
                </div>
            )}
        </BaseRoundDisplay>
    );
} 