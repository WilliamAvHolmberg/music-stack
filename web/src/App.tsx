import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Toaster } from '@/shared/components/ui/toaster';
import { AIModelProvider } from './features/ai/ai-model-context';
import { RootLayout } from './shared/layouts/root-layout';
import { ScrapingTestPage } from './features/scraping/pages/scraping-test';
import { W3ValidatorPage } from './features/w3/pages/w3-validator';

export function App() {
  return (
    <AIModelProvider>
      <BrowserRouter>
        <Routes>
          <Route element={<RootLayout />}>
            <Route path="/" element={<h1>Hello World</h1>} />
            <Route path="/scraping-test" element={<ScrapingTestPage />} />
            <Route path="/w3-validator" element={<W3ValidatorPage />} />
          </Route>
        </Routes>
        <Toaster />
      </BrowserRouter>
    </AIModelProvider>
  );
}