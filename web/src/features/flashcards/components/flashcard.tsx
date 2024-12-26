import { useState } from 'react';
import { Card, CardContent } from '@/shared/components/ui/card';
import { Button } from '@/shared/components/ui/button';
import { Badge } from '@/shared/components/ui/badge';
import type { FlashcardDTO } from '@/shared/api/models';
interface FlashcardProps {
  flashcard: FlashcardDTO;
  showAnswer?: boolean;
  onFlip?: () => void;
  onNext?: () => void;
  onPrevious?: () => void;
}

export function Flashcard({ 
  flashcard, 
  showAnswer = false,
  onFlip,
  onNext, 
  onPrevious 
}: FlashcardProps) {
  const [internalShowAnswer, setInternalShowAnswer] = useState(showAnswer);

  const toggleAnswer = () => {
    if (onFlip) {
      onFlip();
    } else {
      setInternalShowAnswer(!internalShowAnswer);
    }
  };

  const displayAnswer = onFlip ? showAnswer : internalShowAnswer;

  const importanceColor = {
    1: 'bg-red-500',
    2: 'bg-yellow-500',
    3: 'bg-green-500'
  }[flashcard.importance ?? 0];

  return (
    <Card className="w-full max-w-2xl mx-auto">
      <CardContent className="p-6 space-y-4">
        <div className="flex justify-between items-start">
          <Badge className={importanceColor}>
            Level {flashcard.importance}
          </Badge>
        </div>
        
        <div 
          className="min-h-[200px] flex items-center justify-center text-lg font-medium text-center p-4 cursor-pointer"
          onClick={toggleAnswer}
        >
          {displayAnswer ? flashcard.answer : flashcard.question}
        </div>

        <div className="flex justify-between pt-4">
          <Button
            variant="outline"
            onClick={onPrevious}
            disabled={!onPrevious}
          >
            Previous
          </Button>
          <Button
            variant="outline"
            onClick={toggleAnswer}
          >
            {displayAnswer ? 'Show Question' : 'Show Answer'}
          </Button>
          <Button
            variant="outline"
            onClick={onNext}
            disabled={!onNext}
          >
            Next
          </Button>
        </div>
      </CardContent>
    </Card>
  );
} 