/**
 * Generated by orval v7.3.0 🍺
 * Do not edit manually.
 * API
 * OpenAPI spec version: v1
 */

export interface CreateRoundItemRequestDTO {
  /** @minLength 1 */
  artist: string;
  /** @nullable */
  extraInfo?: string | null;
  orderIndex?: number;
  points?: number;
  /** @minLength 1 */
  title: string;
}
