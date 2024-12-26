import { useNavigate } from 'react-router-dom';
import { Button } from '@/shared/components/ui/button';
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from '@/shared/components/ui/card';
import { useGetApiFlashcards } from '@/shared/api/hooks/api';

export function FlashcardSets() {
  const { data: sets, isLoading } = useGetApiFlashcards();
  const navigate = useNavigate();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Flashcard Sets</h1>
        <Button onClick={() => navigate('/flashcards/create')}>
          Create New Set
        </Button>
      </div>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        {sets?.map(set => (
          <Card key={set.id} className="cursor-pointer hover:bg-accent/50 transition-colors"
                onClick={() => navigate(`/flashcards/${set.id}/overview`)}>
            <CardHeader>
              <CardTitle>{set.title}</CardTitle>
              <CardDescription>
                {set.flashcards?.length ?? 0} cards • Created {new Date(set.createdAt ?? '').toLocaleDateString()}
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="text-sm text-muted-foreground">
                {set.description || 'No description'}
              </div>
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  );
} 