/**
 * Generated by orval v7.3.0 🍺
 * Do not edit manually.
 * API
 * OpenAPI spec version: v1
 */
import type { RoundResponseDTO } from './roundResponseDTO';
import type { GameStatusDTO } from './gameStatusDTO';
import type { TeamResponseDTO } from './teamResponseDTO';

export interface GameSessionResponseDTO {
  createdAt: string;
  currentItemIndex: number;
  currentRoundIndex: number;
  id: number;
  /** @nullable */
  rounds: RoundResponseDTO[] | null;
  status: GameStatusDTO;
  /** @nullable */
  teams: TeamResponseDTO[] | null;
}
