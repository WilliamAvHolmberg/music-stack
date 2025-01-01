import { RoundType } from '../../types';
import { GuessTheMelodyDisplay } from './GuessTheMelodyDisplay';
import { FirstLineDisplay } from './FirstLineDisplay';

interface RoundDisplayProps {
    title: string;
    artist: string;
    points: number;
    isRevealed: boolean;
    extraInfo: string;
    year: number;
}

export function getRoundDisplay(type: RoundType, props: RoundDisplayProps) {
    switch (type) {
        case RoundType.GuessTheMelody:
            return <GuessTheMelodyDisplay {...props} />;
        case RoundType.FirstLine:
            return <FirstLineDisplay {...props} />;
        default:
            return <GuessTheMelodyDisplay {...props} />; // Fallback
    }
} 