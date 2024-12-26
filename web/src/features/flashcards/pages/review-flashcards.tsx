import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Card } from '@/shared/components/ui/card';
import { Button } from '@/shared/components/ui/button';
import { Progress } from '@/shared/components/ui/progress';
import { useGetApiFlashcardsId, usePostApiReviewFlashcardIdReview } from '@/shared/api/hooks/api';
import { motion, AnimatePresence } from 'framer-motion';
import { Brain, ArrowRight, Check, X, ChevronLeft } from 'lucide-react';
import { cn } from '@/shared/utils/utils';
import { useToast } from '@/shared/hooks/use-toast';
import type { FlashcardDTO } from '@/shared/api/models';

export function ReviewFlashcards() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { toast } = useToast();
  const { data: flashcardSet, isLoading } = useGetApiFlashcardsId(Number(id), {});
  const processAnswer = usePostApiReviewFlashcardIdReview();
  const [currentIndex, setCurrentIndex] = useState(0);
  const [showAnswer, setShowAnswer] = useState(false);
  const [isProcessing, setIsProcessing] = useState(false);

  if (isLoading || !flashcardSet || !flashcardSet.flashcards?.length) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-xl font-light text-muted-foreground">Loading your review session...</div>
      </div>
    );
  }

  const dueFlashcards = flashcardSet.flashcards.filter((f: FlashcardDTO) => f.isMarkedForReview);

  if (!dueFlashcards.length) {
    return (
      <div className="min-h-screen bg-gradient-to-b from-blue-50/50 to-white flex items-center justify-center">
        <Card className="max-w-lg w-full mx-6">
          <div className="p-8 text-center space-y-4">
            <div className="p-3 bg-green-100 rounded-full w-fit mx-auto">
              <Check className="h-6 w-6 text-green-600" />
            </div>
            <h2 className="text-2xl font-light">All Caught Up!</h2>
            <p className="text-muted-foreground">You've completed all your reviews for now. Check back later!</p>
            <Button
              onClick={() => navigate(`/flashcards/${id}/overview`)}
              className="mt-4"
            >
              Back to Overview
            </Button>
          </div>
        </Card>
      </div>
    );
  }

  const currentCard = dueFlashcards[currentIndex];
  if (!currentCard?.id) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-xl font-light text-muted-foreground">Invalid flashcard data</div>
      </div>
    );
  }

  // After the check above, we know currentCard has an id
  const card = currentCard as Required<Pick<FlashcardDTO, 'id'>> & FlashcardDTO;
  const progress = ((currentIndex + 1) / dueFlashcards.length) * 100;

  const handleAnswer = async (isCorrect: boolean) => {
    if (isProcessing) return;
    setIsProcessing(true);

    try {
      await processAnswer.mutateAsync({
        id: card.id,
        data: {
          isCorrect
        }
      });

      if (currentIndex < dueFlashcards.length - 1) {
        setCurrentIndex(prev => prev + 1);
        setShowAnswer(false);
      } else {
        toast({
          title: 'Review Complete!',
          description: 'You\'ve reviewed all marked cards.'
        });
        navigate(`/flashcards/${id}/overview`);
      }
    } catch (error) {
      toast({
        title: 'Error submitting review',
        description: 'Failed to save your answer. Please try again.',
        variant: 'destructive'
      });
    } finally {
      setIsProcessing(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-b from-blue-50/50 to-white">
      <div className="max-w-4xl mx-auto px-6 py-12">
        {/* Header */}
        <div className="mb-12 space-y-6">
          <div className="flex items-center justify-between">
            <Button
              variant="ghost"
              onClick={() => navigate(`/flashcards/${id}/overview`)}
              className="text-muted-foreground hover:text-foreground"
            >
              <ChevronLeft className="h-4 w-4 mr-2" />
              Back to Overview
            </Button>
            <div className="flex items-center gap-2 text-sm text-muted-foreground">
              <Brain className="h-4 w-4" />
              Review Session
            </div>
          </div>
          
          <div className="space-y-2">
            <div className="flex justify-between items-center text-sm text-muted-foreground">
              <span>Progress</span>
              <span>Card {currentIndex + 1} of {dueFlashcards.length}</span>
            </div>
            <Progress value={progress} className="h-2" />
          </div>
        </div>

        {/* Card */}
        <AnimatePresence mode="wait">
          <motion.div
            key={card.id}
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: -20 }}
            transition={{ duration: 0.3 }}
          >
            <Card className="overflow-hidden">
              <div className="p-8 border-b">
                <div className="prose prose-lg max-w-none">
                  <h2 className="text-2xl font-light mb-0">{card.question}</h2>
                </div>
              </div>

              <div className="p-8 space-y-6">
                {!showAnswer ? (
                  <Button
                    onClick={() => setShowAnswer(true)}
                    className="w-full py-6 text-lg bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 shadow-lg hover:shadow-xl transition-all duration-300"
                  >
                    Show Answer
                    <ArrowRight className="ml-2 h-5 w-5" />
                  </Button>
                ) : (
                  <motion.div
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    className="space-y-6"
                  >
                    <Card className="p-6 bg-blue-50 border-blue-100">
                      <h3 className="text-lg font-medium text-blue-900 mb-2">Answer:</h3>
                      <div className="text-blue-800">{card.answer}</div>
                    </Card>

                    <div className="grid grid-cols-2 gap-4">
                      <Button
                        onClick={() => handleAnswer(false)}
                        variant="outline"
                        className={cn(
                          "py-6 text-lg border-2",
                          isProcessing && "opacity-50 cursor-not-allowed"
                        )}
                        disabled={isProcessing}
                      >
                        <X className="mr-2 h-5 w-5 text-red-500" />
                        Incorrect
                      </Button>
                      <Button
                        onClick={() => handleAnswer(true)}
                        className={cn(
                          "py-6 text-lg bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700",
                          isProcessing && "opacity-50 cursor-not-allowed"
                        )}
                        disabled={isProcessing}
                      >
                        <Check className="mr-2 h-5 w-5" />
                        Correct
                      </Button>
                    </div>

                    {card.notes && (
                      <div className="p-4 bg-yellow-50 border border-yellow-100 rounded-lg">
                        <div className="text-sm font-medium text-yellow-800 mb-1">Study Note:</div>
                        <div className="text-yellow-700">{card.notes}</div>
                      </div>
                    )}
                  </motion.div>
                )}
              </div>
            </Card>
          </motion.div>
        </AnimatePresence>
      </div>
    </div>
  );
} 