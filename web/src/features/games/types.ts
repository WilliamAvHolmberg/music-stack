import type { 
    GameTemplateResponseDTO,
    RoundItemResponseDTO
} from '@/shared/api/models';

export type GameTemplate = GameTemplateResponseDTO;
export type RoundItem = RoundItemResponseDTO;

export enum RoundType {
    GuessTheMelody = 0,
    FirstLine = 1,
    ArtistQuiz = 2
}

export enum RoundStatus {
    NotStarted = 0,
    InProgress = 1,
    Completed = 2
}

export enum SongCategory {
    Pop = 0,
    Rock = 1,
    Schlager = 2
}

export enum SongLanguage {
    Swedish = 0,
    English = 1
}

export enum GameStatus {
    NotStarted = 0,
    InProgress = 1,
    Completed = 2
}