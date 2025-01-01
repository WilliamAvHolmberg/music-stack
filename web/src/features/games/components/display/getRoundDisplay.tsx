import { RoundType } from '../../types';
import { GuessTheMelodyDisplay } from './GuessTheMelodyDisplay';
import { FirstLineDisplay } from './FirstLineDisplay';

export function getRoundDisplay(type: RoundType, props: any) {
    switch (type) {
        case RoundType.GuessTheMelody:
            return <GuessTheMelodyDisplay {...props} />;
        case RoundType.FirstLine:
            return <FirstLineDisplay {...props} />;
        default:
            return <GuessTheMelodyDisplay {...props} />; // Fallback
    }
} 