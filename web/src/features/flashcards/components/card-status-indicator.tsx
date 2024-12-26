import { CheckCircle2, XCircle, Circle } from 'lucide-react';
import { cn } from '@/shared/utils/utils';

interface CardStatusIndicatorProps {
  status?: 'correct' | 'incorrect';
  size?: number;
  className?: string;
}

export function CardStatusIndicator({ status, size = 16, className }: CardStatusIndicatorProps) {
  if (!status) {
    return <Circle className={cn("text-muted-foreground", className)} size={size} />;
  }

  if (status === 'correct') {
    return <CheckCircle2 className={cn("text-green-500", className)} size={size} />;
  }

  return <XCircle className={cn("text-red-500", className)} size={size} />;
} 