import type { 
    GameSessionResponseDTO,
    TeamResponseDTO,
    RoundResponseDTO,
    SongResponseDTO,
    GameTemplateResponseDTO,
    RoundItemResponseDTO
} from '@/shared/api/models';

export type GameSession = GameSessionResponseDTO;
export type Team = TeamResponseDTO;
export type Round = RoundResponseDTO;
export type Song = SongResponseDTO;
export type GameTemplate = GameTemplateResponseDTO;
export type RoundItem = RoundItemResponseDTO;

export enum GameStatus {
    Created = 0,
    InProgress = 1,
    Finished = 2
}

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

export interface CreateGameTemplateRequest {
    name: string;
    description: string;
    isPublic: boolean;
}

export interface UpdateGameTemplateRequest {
    name: string;
    description: string;
    isPublic: boolean;
}

export interface CreateRoundRequest {
    type: RoundType;
    title: string;
    timeInMinutes: number;
    instructions: string;
    orderIndex: number;
}

export interface UpdateRoundRequest {
    title: string;
    timeInMinutes: number;
    instructions: string;
    orderIndex: number;
}

export interface CreateRoundItemRequest {
    title: string;
    artist: string;
    points: number;
    extraInfo: string;
    orderIndex: number;
}

export interface UpdateRoundItemRequest {
    title: string;
    artist: string;
    points: number;
    extraInfo: string;
    orderIndex: number;
} 