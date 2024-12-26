import { BrowserRouter, Routes, Route, useParams, Navigate } from 'react-router-dom';
import { Toaster } from '@/shared/components/ui/toaster';
import { AIModelProvider } from './features/ai/ai-model-context';
import { RootLayout } from './shared/layouts/root-layout';
import { FlashcardSetOverview } from './features/flashcards/pages/flashcard-set-overview';
import { StudyFlashcards } from './features/flashcards/pages/study-flashcards';
import { FlashcardSets } from './features/flashcards/pages/flashcard-sets';
import { CreateFlashcards } from './features/flashcards/pages/create-flashcards';
import { ReviewFlashcards } from './features/flashcards/pages/review-flashcards';

// Wrapper components to handle route parameters
function FlashcardSetOverviewWrapper() {
  const { id } = useParams<{ id: string }>();
  return <FlashcardSetOverview id={Number(id)} />;
}

function StudyFlashcardsWrapper() {
  const { id } = useParams<{ id: string }>();
  return <StudyFlashcards id={Number(id)} />;
}

export function App() {
  return (
    <AIModelProvider>
      <BrowserRouter>
        <Routes>
          <Route element={<RootLayout />}>
            <Route path="/" element={<Navigate to="/flashcards" replace />} />
            <Route path="/flashcards" element={<FlashcardSets />} />
            <Route path="/flashcards/create" element={<CreateFlashcards />} />
            <Route path="/flashcards/:id/overview" element={<FlashcardSetOverviewWrapper />} />
            <Route path="/flashcards/:id/study" element={<StudyFlashcardsWrapper />} />
            <Route path="/flashcards/:id/review" element={<ReviewFlashcards />} />
          </Route>
        </Routes>
        <Toaster />
      </BrowserRouter>
    </AIModelProvider>
  );
}