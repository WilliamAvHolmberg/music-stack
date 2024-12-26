import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card } from '@/shared/components/ui/card';
import { Button } from '@/shared/components/ui/button';
import { Textarea } from '@/shared/components/ui/textarea';
import { Progress } from '@/shared/components/ui/progress';
import { useGetApiFlashcardsId, usePostApiFlashcardsFlashcardIdAnalyze, usePostApiReviewFlashcardIdReview } from '@/shared/api/hooks/api';
import { motion, AnimatePresence } from 'framer-motion';
import { Brain, ArrowRight, Check, X, ChevronLeft, Loader2 } from 'lucide-react';
import { cn } from '@/shared/utils/utils';
import { useAIModel } from '@/features/ai/ai-model-context';
import { AnswerAnalysisView } from '../components/answer-analysis';
import type { AnswerAnalysisDTO, FlashcardDTO } from '@/shared/api/models';

interface StudyFlashcardsProps {
  id: number;
  startWithCardId?: number;
}

export function StudyFlashcards({ id, startWithCardId }: StudyFlashcardsProps) {
  const navigate = useNavigate();
  const { data: flashcardSet, isLoading } = useGetApiFlashcardsId(id, {});
  const processAnswer = usePostApiReviewFlashcardIdReview();
  const analyzeAnswer = usePostApiFlashcardsFlashcardIdAnalyze();
  const { selectedModel } = useAIModel();
  const [currentIndex, setCurrentIndex] = useState(0);
  const [showAnswer, setShowAnswer] = useState(false);
  const [answer, setAnswer] = useState('');
  const [isProcessing, setIsProcessing] = useState(false);
  const [analysis, setAnalysis] = useState<AnswerAnalysisDTO | null>(null);
  const [isAnalyzing, setIsAnalyzing] = useState(false);

  if (isLoading || !flashcardSet || !flashcardSet.flashcards?.length) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-xl font-light text-muted-foreground">Loading your study session...</div>
      </div>
    );
  }

  const currentCard = flashcardSet.flashcards[currentIndex];
  if (!currentCard?.id) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-xl font-light text-muted-foreground">Invalid flashcard data</div>
      </div>
    );
  }

  // After the check above, we know currentCard has an id
  const card = currentCard as Required<Pick<FlashcardDTO, 'id'>> & FlashcardDTO;
  
  const progress = ((currentIndex + 1) / flashcardSet.flashcards.length) * 100;

  const handleShowAnswer = async () => {
    setShowAnswer(true);
    if (answer.trim()) {
      setIsAnalyzing(true);
      try {
        const result = await analyzeAnswer.mutateAsync({
          id: card.id,
          data: answer,
          params: {
            model: selectedModel.id,
            provider: selectedModel.provider
          }
        });
        setAnalysis(result);
      } catch (error) {
        console.error('Failed to analyze answer:', error);
      } finally {
        setIsAnalyzing(false);
      }
    }
  };

  const handleRefreshAnalysis = async () => {
    setIsAnalyzing(true);
    try {
      const result = await analyzeAnswer.mutateAsync({
        id: card.id,
        data: answer,
        params: {
          model: selectedModel.id,
          provider: selectedModel.provider
        }
      });
      setAnalysis(result);
    } catch (error) {
      console.error('Failed to analyze answer:', error);
    } finally {
      setIsAnalyzing(false);
    }
  };

  const handleAnswer = async (isCorrect: boolean) => {
    if (isProcessing || isAnalyzing) return;
    setIsProcessing(true);

    try {
      await processAnswer.mutateAsync({
        id: card.id,
        data: {
          isCorrect
        }
      });

      if (currentIndex < (flashcardSet.flashcards?.length ?? 0) - 1) {
        setCurrentIndex(prev => prev + 1);
        setShowAnswer(false);
        setAnswer('');
        setAnalysis(null);
      } else {
        navigate(`/flashcards/${id}/overview`);
      }
    } finally {
      setIsProcessing(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-b from-slate-50 to-white">
      <div className="max-w-3xl mx-auto px-4 py-8">
        {/* Header */}
        <div className="mb-8">
          <div className="flex items-center justify-between mb-4">
            <Button
              variant="ghost"
              onClick={() => navigate(`/flashcards/${id}/overview`)}
              className="text-muted-foreground hover:text-foreground -ml-2"
            >
              <ChevronLeft className="h-4 w-4 mr-1" />
              Overview
            </Button>
            <div className="flex items-center gap-2 text-sm text-muted-foreground">
              <Brain className="h-4 w-4" />
              <span className="text-xs font-medium">Study Session</span>
            </div>
          </div>

          <div className="flex items-center justify-between text-xs text-muted-foreground mb-2">
            <span>Progress</span>
            <span>Card {currentIndex + 1} of {flashcardSet.flashcards?.length ?? 0}</span>
          </div>
          <Progress value={progress} className="h-1" />
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
            <Card className="overflow-hidden border-slate-200">
              <div className="p-6 border-b border-slate-100">
                <div className="prose prose-slate max-w-none">
                  <h2 className="text-xl font-medium mb-0 text-slate-900">{card.question}</h2>
                </div>
              </div>

              <div className="p-6 space-y-4">
                <Textarea
                  value={answer}
                  onChange={(e) => setAnswer(e.target.value)}
                  placeholder="Write your answer..."
                  className="min-h-[120px] text-base resize-none bg-slate-50/50 border-slate-200 focus:border-slate-300 focus:ring-slate-200"
                  disabled={showAnswer}
                />

                {!showAnswer ? (
                  <Button
                    onClick={handleShowAnswer}
                    className="w-full h-11 bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 text-sm font-medium"
                  >
                    Check Answer
                    <ArrowRight className="ml-2 h-4 w-4" />
                  </Button>
                ) : (
                  <motion.div
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    className="space-y-4"
                  >
                    <Card className="p-4 bg-blue-50/50 border-blue-100">
                      <h3 className="text-sm font-medium text-blue-900 mb-2">Correct Answer</h3>
                      <div className="text-sm text-blue-800 leading-relaxed">{card.answer}</div>
                    </Card>

                    {answer.trim() && (
                      <div className="relative">
                        {isAnalyzing ? (
                          <Card className="p-4 border-dashed border-slate-200 bg-slate-50/50">
                            <div className="flex items-center justify-center gap-2 text-sm text-muted-foreground">
                              <Loader2 className="h-4 w-4 animate-spin" />
                              Analyzing...
                            </div>
                          </Card>
                        ) : analysis && (
                          <AnswerAnalysisView
                            analysis={analysis}
                            onRefresh={handleRefreshAnalysis}
                            isRefreshing={isAnalyzing}
                          />
                        )}
                      </div>
                    )}

                    <div className="grid grid-cols-2 gap-3">
                      <Button
                        onClick={() => handleAnswer(false)}
                        variant="outline"
                        className={cn(
                          "h-11 text-sm font-medium border-slate-200 hover:bg-slate-50",
                          (isProcessing || isAnalyzing) && "opacity-50 cursor-not-allowed"
                        )}
                        disabled={isProcessing || isAnalyzing}
                      >
                        <X className="mr-2 h-4 w-4 text-red-500" />
                        Incorrect
                      </Button>
                      <Button
                        onClick={() => handleAnswer(true)}
                        className={cn(
                          "h-11 text-sm font-medium bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700",
                          (isProcessing || isAnalyzing) && "opacity-50 cursor-not-allowed"
                        )}
                        disabled={isProcessing || isAnalyzing}
                      >
                        <Check className="mr-2 h-4 w-4" />
                        Correct
                      </Button>
                    </div>

                    {card.notes && (
                      <div className="p-3 bg-yellow-50/50 border border-yellow-100 rounded-lg">
                        <div className="text-xs font-medium text-yellow-800 mb-1">Study Note</div>
                        <div className="text-sm text-yellow-700">{card.notes}</div>
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