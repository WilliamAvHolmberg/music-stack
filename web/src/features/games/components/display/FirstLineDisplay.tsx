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

export function FirstLineDisplay({ title, artist, points, isRevealed, extraInfo, year }: FirstLineDisplayProps) {
    return (
        <BaseRoundDisplay points={points} isRevealed={isRevealed}>
            {isRevealed ? (
                <div className="w-full max-w-4xl mx-auto">
                    <AnimatedReveal
                        title={title}
                        artist={artist}
                        points={points}
                        isRevealed={isRevealed}
                        extraInfo={extraInfo}
                        year={year}
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