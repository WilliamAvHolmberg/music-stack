import { Card } from '@/shared/components/ui/card';
import { Progress } from '@/shared/components/ui/progress';
import type { ReviewStatisticsDTO, FlashcardDTO } from '@/shared/api/models';
import { Brain, AlertTriangle } from 'lucide-react';

interface StudyStatisticsProps {
  statistics: ReviewStatisticsDTO;
  onStudyDifficult?: (flashcard: FlashcardDTO) => void;
}

export function StudyStatistics({ statistics }: StudyStatisticsProps) {
  const successRate = (statistics.completedReviews ?? 0) > 0
    ? ((statistics.masteredFlashcards ?? 0) / (statistics.completedReviews ?? 1)) * 100
    : 0;

  return (
    <div className="space-y-8">
      <Card className="overflow-hidden">
        <div className="p-8 border-b">
          <div className="flex items-center gap-3">
            <div className="p-2 bg-blue-100 rounded-lg">
              <Brain className="h-5 w-5 text-blue-600" />
            </div>
            <h2 className="text-2xl font-light">Learning Progress</h2>
          </div>
        </div>

        <div className="p-8 space-y-8">
          {/* Overall Progress */}
          <div className="space-y-3">
            <div className="flex justify-between items-end">
              <div>
                <h3 className="text-sm font-medium text-muted-foreground">Success Rate</h3>
                <div className="text-3xl font-light mt-1">{Math.round(successRate)}%</div>
              </div>
              <div className="text-sm text-muted-foreground">
                {statistics.masteredFlashcards ?? 0} of {statistics.completedReviews ?? 0} correct
              </div>
            </div>
            <Progress
              value={successRate}
              className="h-2 bg-blue-100"
            />
          </div>
        </div>
      </Card>

      {/* Difficult Cards */}
      {statistics.strugglingFlashcards && statistics.strugglingFlashcards > 0 && (
        <Card className="overflow-hidden">
          <div className="p-8 border-b">
            <div className="flex items-center gap-3">
              <div className="p-2 bg-yellow-100 rounded-lg">
                <AlertTriangle className="h-5 w-5 text-yellow-600" />
              </div>
              <h3 className="text-2xl font-light">Cards Needing More Practice</h3>
            </div>
          </div>
          <div className="divide-y">
            {/* Note: The API doesn't seem to provide the actual struggling cards list.
                You'll need to add an API endpoint to fetch these or modify the DTO */}
          </div>
        </Card>
      )}
    </div>
  );
} 