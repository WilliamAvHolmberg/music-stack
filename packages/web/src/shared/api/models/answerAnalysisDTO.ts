/**
 * Generated by orval v7.3.0 🍺
 * Do not edit manually.
 * API
 * OpenAPI spec version: v1
 */

export interface AnswerAnalysisDTO {
  /** @nullable */
  feedback: string | null;
  /** @nullable */
  improvement: string | null;
  /** @nullable */
  keyPointsCovered: string[] | null;
  /** @nullable */
  keyPointsMissed: string[] | null;
  score: number;
}