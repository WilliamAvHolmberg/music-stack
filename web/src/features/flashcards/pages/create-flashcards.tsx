import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button } from '@/shared/components/ui/button';
import { Input } from '@/shared/components/ui/input';
import { Textarea } from '@/shared/components/ui/textarea';
import { Card, CardHeader, CardTitle, CardContent } from '@/shared/components/ui/card';
import { usePostApiFlashcardsGenerateContent } from '@/shared/api/hooks/api';
import { useToast } from '@/shared/hooks/use-toast';
import { useAIModel } from '@/features/ai/ai-model-context';

export function CreateFlashcards() {
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const { selectedModel } = useAIModel();
  const generate = usePostApiFlashcardsGenerateContent();
  const { toast } = useToast();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      console.log('Generating flashcards with model:', selectedModel);
      const result = await generate.mutateAsync({
        data: {
          title,
          content,
          model: selectedModel.id,
          provider: selectedModel.provider
        }
      });
      toast({
        title: 'Flashcards Generated',
        description: 'Your flashcards have been created successfully.'
      });
      navigate(`/flashcards/${result.id}/overview`);
    } catch (error: any) {
      console.log('Full error:', error.response?.data);

      const errorData = error.response?.data;
      let errorMessage = 'Failed to generate flashcards';

      if (errorData) {
        errorMessage = errorData.message;
        if (errorData.details) {
          errorMessage += `\nError: ${errorData.details.type}`;
          if (errorData.details.code) {
            errorMessage += ` (${errorData.details.code})`;
          }
        }
      }

      toast({
        title: 'Generation Error',
        description: errorMessage,
        variant: 'destructive'
      });
    }
  };

  return (
    <Card className="max-w-2xl mx-auto">
      <CardHeader>
        <CardTitle>Create Flashcards</CardTitle>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Input
              placeholder="Enter title for your flashcard set"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
            />
          </div>
          <div className="space-y-2">
            <Textarea
              placeholder="Paste your content here..."
              value={content}
              onChange={(e) => setContent(e.target.value)}
              className="min-h-[200px]"
              required
            />
          </div>
          <Button
            type="submit"
            className="w-full"
            disabled={generate.isPending}
          >
            {generate.isPending ? 'Generating...' : 'Generate Flashcards'}
          </Button>
        </form>
      </CardContent>
    </Card>
  );
} 