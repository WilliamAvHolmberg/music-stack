/**
 * Generated by orval v7.3.0 🍺
 * Do not edit manually.
 * API
 * OpenAPI spec version: v1
 */
import type { CreateTemplateRoundItemRequestDTO } from './createTemplateRoundItemRequestDTO';
import type { RoundTypeDTO } from './roundTypeDTO';

export interface CreateTemplateRoundRequestDTO {
  /** @nullable */
  instructions?: string | null;
  /** @nullable */
  items?: CreateTemplateRoundItemRequestDTO[] | null;
  orderIndex?: number;
  timeInMinutes?: number;
  /** @minLength 1 */
  title: string;
  type: RoundTypeDTO;
}
