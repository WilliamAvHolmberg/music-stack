import { Card } from '@/shared/components/ui/card';
import { Progress } from '@/shared/components/ui/progress';
import { motion } from 'framer-motion';
import { CheckCircle2, XCircle, ArrowUpCircle, RefreshCw } from 'lucide-react';
import { cn } from '@/shared/utils/utils';
import type { AnswerAnalysisDTO } from '@/shared/api/models';
import { Button } from '@/shared/components/ui/button';

interface AnswerAnalysisViewProps {
  analysis: AnswerAnalysisDTO;
  onRefresh?: () => void;
  isRefreshing?: boolean;
}

export function AnswerAnalysisView({ analysis, onRefresh, isRefreshing }: AnswerAnalysisViewProps) {
  const scoreColor = analysis.score >= 70 
    ? 'text-green-500' 
    : analysis.score >= 40 
      ? 'text-yellow-500' 
      : 'text-red-500';

  return (
    <motion.div
      initial={{ opacity: 0, y: 10 }}
      animate={{ opacity: 1, y: 0 }}
      className="space-y-4"
    >
      <Card className="p-4 bg-gradient-to-br from-slate-50 to-white border-slate-200">
        <div className="flex items-center justify-between mb-3">
          <div className="flex items-center gap-3">
            <Progress value={analysis.score} className="w-16 h-1.5" />
            <span className={cn("font-medium text-sm", scoreColor)}>
              {analysis.score}%
            </span>
          </div>
          <div className="flex items-center gap-2">
            {onRefresh && (
              <Button
                variant="ghost"
                size="sm"
                className="h-7 px-2 text-muted-foreground hover:text-foreground"
                onClick={onRefresh}
                disabled={isRefreshing}
              >
                <RefreshCw className={cn("h-3.5 w-3.5", isRefreshing && "animate-spin")} />
              </Button>
            )}
            <span className="text-xs font-medium text-muted-foreground">Analysis</span>
          </div>
        </div>

        <p className="text-sm text-muted-foreground leading-relaxed mb-4">
          {analysis.feedback}
        </p>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <div className="flex items-center gap-2 mb-2">
              <XCircle className="h-4 w-4 text-red-500" />
              <h4 className="text-xs font-medium text-red-700">Missed Points</h4>
            </div>
            <ul className="space-y-1.5">
              {analysis.keyPointsMissed?.map((point, i) => (
                <motion.li
                  key={i}
                  initial={{ opacity: 0, x: -5 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ delay: i * 0.1 }}
                  className="text-xs text-red-600 leading-relaxed"
                >
                  {point}
                </motion.li>
              ))}
            </ul>
          </div>

          <div>
            <div className="flex items-center gap-2 mb-2">
              <CheckCircle2 className="h-4 w-4 text-green-500" />
              <h4 className="text-xs font-medium text-green-700">Points Covered</h4>
            </div>
            <ul className="space-y-1.5">
              {analysis.keyPointsCovered?.map((point, i) => (
                <motion.li
                  key={i}
                  initial={{ opacity: 0, x: -5 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ delay: i * 0.1 }}
                  className="text-xs text-green-600 leading-relaxed"
                >
                  {point}
                </motion.li>
              ))}
            </ul>
          </div>
        </div>

        {analysis.improvement && (
          <div className="mt-4 pt-4 border-t border-slate-100">
            <div className="flex items-center gap-2 mb-2">
              <ArrowUpCircle className="h-4 w-4 text-blue-500" />
              <h4 className="text-xs font-medium text-blue-700">How to Improve</h4>
            </div>
            <p className="text-xs text-blue-600 leading-relaxed">
              {analysis.improvement}
            </p>
          </div>
        )}
      </Card>
    </motion.div>
  );
} 