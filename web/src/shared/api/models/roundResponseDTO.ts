/**
 * Generated by orval v7.3.0 🍺
 * Do not edit manually.
 * API
 * OpenAPI spec version: v1
 */
import type { RoundItemResponseDTO } from './roundItemResponseDTO';
import type { RoundStatusDTO } from './roundStatusDTO';
import type { RoundTypeDTO } from './roundTypeDTO';

export interface RoundResponseDTO {
  id: number;
  /** @nullable */
  instructions?: string | null;
  isAnswerRevealed?: boolean;
  isPaused?: boolean;
  /** @nullable */
  items: RoundItemResponseDTO[] | null;
  orderIndex: number;
  status: RoundStatusDTO;
  timeInMinutes: number;
  timeLeft?: number;
  /** @nullable */
  title: string | null;
  type: RoundTypeDTO;
}