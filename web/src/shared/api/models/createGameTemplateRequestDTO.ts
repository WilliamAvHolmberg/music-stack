/**
 * Generated by orval v7.3.0 🍺
 * Do not edit manually.
 * API
 * OpenAPI spec version: v1
 */
import type { CreateTemplateRoundRequestDTO } from './createTemplateRoundRequestDTO';

export interface CreateGameTemplateRequestDTO {
  /** @nullable */
  description?: string | null;
  isPublic?: boolean;
  /** @minLength 1 */
  name: string;
  /** @nullable */
  rounds?: CreateTemplateRoundRequestDTO[] | null;
}
