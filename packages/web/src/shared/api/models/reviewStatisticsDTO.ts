/**
 * Generated by orval v7.3.0 🍺
 * Do not edit manually.
 * API
 * OpenAPI spec version: v1
 */
import type { ReviewStatisticsDTOReviewsByDay } from './reviewStatisticsDTOReviewsByDay';

export interface ReviewStatisticsDTO {
  averageScore?: number;
  completedReviews?: number;
  dueFlashcards?: number;
  lastReviewDate?: string;
  masteredFlashcards?: number;
  nextDueDate?: string;
  /** @nullable */
  reviewsByDay?: ReviewStatisticsDTOReviewsByDay;
  streakDays?: number;
  strugglingFlashcards?: number;
  totalFlashcards?: number;
}
