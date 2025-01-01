import { AnimatedReveal } from './AnimatedReveal';
import { BaseRoundDisplay } from './BaseRoundDisplay';
import { Music } from 'lucide-react';

interface GuessTheMelodyDisplayProps {
    title: string;
    artist: string;
    points: number;
    isRevealed: boolean;
    extraInfo?: string;
    year: number;
}

export function GuessTheMelodyDisplay(props: GuessTheMelodyDisplayProps) {
    return (
        <BaseRoundDisplay points={props.points} isRevealed={props.isRevealed}>
            {props.isRevealed ? (
                <>
                    <div className="w-full max-w-4xl mx-auto">
                        <AnimatedReveal
                            title={props.title}
                            artist={props.artist}
                            points={props.points}
                            isRevealed={props.isRevealed}
                            extraInfo={props.extraInfo}
                            year={props.year}
                        />
                    </div>
                </>
            ) : (
                <div className="flex flex-col items-center justify-center space-y-4 py-32">
                    <Music className="w-16 h-16 text-blue-300 animate-bounce" />
                    <div className="text-4xl font-bold bg-gradient-to-r from-blue-400 to-purple-400 bg-clip-text text-transparent animate-pulse">
                        Lyssna på låten!
                    </div>
                </div>
            )}
        </BaseRoundDisplay>
    );
} 