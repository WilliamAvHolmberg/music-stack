import { useNavigate } from 'react-router-dom';
import { Card } from '@/shared/components/ui/card';
import { Button } from '@/shared/components/ui/button';
import { StudyStatistics } from '../components/study-statistics';
import type { FlashcardDTO } from '@/shared/api/models';
import { motion } from 'framer-motion';
import { ChevronRight, BookOpen, Clock, Star } from 'lucide-react';
import { useGetApiFlashcardsId, useGetApiReviewFlashcardSetIdStatistics } from '@/shared/api/hooks/api';

interface FlashcardSetOverviewProps {
  id: number;
}

export function FlashcardSetOverview({ id }: FlashcardSetOverviewProps) {
  const navigate = useNavigate();
  const { data: flashcardSet, isLoading: isLoadingSet } = useGetApiFlashcardsId(id);
  const { data: statistics, isLoading: isLoadingStats } = useGetApiReviewFlashcardSetIdStatistics(id);

  if (isLoadingSet || isLoadingStats) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-xl font-light text-muted-foreground">Loading your study materials...</div>
      </div>
    );
  }

  if (!flashcardSet || !statistics) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-xl font-light text-muted-foreground">Set not found</div>
      </div>
    );
  }

  const handleStartStudy = () => {
    navigate(`/flashcards/${id}/study`);
  };

  const handleStudyDifficult = (card: FlashcardDTO) => {
    navigate(`/flashcards/${id}/study`, { state: { startWithCard: card.id } });
  };

  return (
    <div className="max-w-6xl mx-auto px-6 py-12 space-y-12">
      {/* Header */}
      <div className="flex justify-between items-start">
        <div className="space-y-4">
          <motion.h1 
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            className="text-5xl font-light tracking-tight"
          >
            {flashcardSet.title}
          </motion.h1>
          {flashcardSet.description && (
            <motion.p 
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 0.2 }}
              className="text-lg text-muted-foreground font-light"
            >
              {flashcardSet.description}
            </motion.p>
          )}
        </div>
        <motion.div
          initial={{ opacity: 0, scale: 0.9 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ delay: 0.3 }}
        >
          <Button 
            size="lg"
            onClick={handleStartStudy}
            className="px-8 py-6 text-lg font-medium rounded-2xl bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 shadow-lg hover:shadow-xl transition-all duration-300"
          >
            <BookOpen className="mr-2 h-5 w-5" />
            Start Study Session
          </Button>
        </motion.div>
      </div>

      {/* Quick Stats */}
      <motion.div 
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.4 }}
        className="grid grid-cols-3 gap-6"
      >
        <Card className="p-6 hover:shadow-lg transition-shadow duration-300">
          <div className="flex items-center space-x-4">
            <div className="p-3 bg-blue-100 rounded-xl">
              <BookOpen className="h-6 w-6 text-blue-600" />
            </div>
            <div>
              <div className="text-3xl font-semibold">{statistics.totalFlashcards}</div>
              <div className="text-sm text-muted-foreground">Total Cards</div>
            </div>
          </div>
        </Card>
        <Card className="p-6 hover:shadow-lg transition-shadow duration-300">
          <div className="flex items-center space-x-4">
            <div className="p-3 bg-green-100 rounded-xl">
              <Star className="h-6 w-6 text-green-600" />
            </div>
            <div>
              <div className="text-3xl font-semibold">{statistics.masteredFlashcards}</div>
              <div className="text-sm text-muted-foreground">Mastered</div>
            </div>
          </div>
        </Card>
        <Card className="p-6 hover:shadow-lg transition-shadow duration-300">
          <div className="flex items-center space-x-4">
            <div className="p-3 bg-purple-100 rounded-xl">
              <Clock className="h-6 w-6 text-purple-600" />
            </div>
            <div>
              <div className="text-3xl font-semibold">{statistics.completedReviews}</div>
              <div className="text-sm text-muted-foreground">Study Sessions</div>
            </div>
          </div>
        </Card>
      </motion.div>

      {/* Statistics */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.5 }}
      >
        <StudyStatistics 
          statistics={statistics} 
          onStudyDifficult={handleStudyDifficult}
        />
      </motion.div>

      {/* All Cards */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.6 }}
      >
        <Card className="overflow-hidden">
          <div className="p-8 border-b">
            <h2 className="text-2xl font-light">All Flashcards</h2>
          </div>
          <div className="divide-y">
            {flashcardSet.flashcards?.map((card, index) => (
              <motion.div 
                key={card.id}
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ delay: 0.1 * index }}
                className="p-6 hover:bg-muted/50 transition-colors duration-200"
              >
                <div className="flex justify-between items-start gap-4">
                  <div className="space-y-2 flex-1">
                    <div className="flex items-center gap-3">
                      <span className="text-sm font-medium text-muted-foreground">
                        Card {index + 1}
                      </span>
                      {card.isMarkedForReview && (
                        <span className="px-2 py-1 rounded-full bg-yellow-100 text-yellow-700 text-xs font-medium">
                          Needs Review
                        </span>
                      )}
                    </div>
                    <div className="font-medium text-lg">{card.question}</div>
                    <div className="text-muted-foreground">{card.answer}</div>
                    {card.notes && (
                      <div className="text-sm bg-blue-50 text-blue-700 p-3 rounded-lg">
                        {card.notes}
                      </div>
                    )}
                    <div className="flex items-center gap-4 text-sm text-muted-foreground mt-2">
                      <span>Reviews: {card.reviewHistory?.length || 0}</span>
                      <span>•</span>
                      <span>Correct: {card.reviewHistory?.filter(r => r.isCorrect).length || 0}</span>
                    </div>
                  </div>
                  <Button
                    variant="ghost"
                    size="icon"
                    className="mt-1"
                    onClick={() => handleStudyDifficult(card)}
                  >
                    <ChevronRight className="h-5 w-5" />
                  </Button>
                </div>
              </motion.div>
            ))}
          </div>
        </Card>
      </motion.div>
    </div>
  );
} 