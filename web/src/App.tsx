import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Toaster } from '@/shared/components/ui/toaster';
import { ConnectionStatus } from '@/shared/components/ConnectionStatus';
import { AIModelProvider } from './features/ai/ai-model-context';
import { RootLayout } from './shared/layouts/root-layout';
import { gameRoutes } from './features/games/routes';
import { songRoutes } from './features/songs/routes';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './shared/components/ui/card';
import { Button } from './shared/components/ui/button';
import { Link } from 'react-router-dom';

function WelcomePage() {
  return (
    <div className="max-w-4xl mx-auto space-y-8">
      <Card>
        <CardHeader>
          <CardTitle>Welcome to Song Stack</CardTitle>
          <CardDescription>
            Create and play music-based party games with your friends!
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="flex gap-4">
            <Button asChild>
              <Link to="/games/templates">Manage Game Templates</Link>
            </Button>
            <Button asChild variant="outline">
              <Link to="/games/new">Start New Game</Link>
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

export function App() {
  return (
    <AIModelProvider>
      <BrowserRouter>
        <ConnectionStatus />
        <Routes>
          <Route element={<RootLayout />}>
            <Route path="/" element={<WelcomePage />} />
            {gameRoutes.map((route) => (
              <Route
                key={route.path}
                path={route.path}
                element={route.element}
              />
            ))}
            {songRoutes.map((route) => (
              <Route
                key={route.path}
                path={route.path}
                element={route.element}
              />
            ))}
          </Route>
        </Routes>
        <Toaster />
      </BrowserRouter>
    </AIModelProvider>
  );
}