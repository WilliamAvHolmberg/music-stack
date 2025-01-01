import { RouteObject } from 'react-router-dom';
import { GameList } from './components/GameList';
import { GameHost } from './components/GameHost';
import { GameSetup } from './components/GameSetup';
import { GameTemplateEditor } from './components/templates/GameTemplateEditor';
import { TemplateList } from './components/templates/TemplateList';

export const gameRoutes: RouteObject[] = [
    {
        path: '/games',
        element: <GameList />
    },
    {
        path: '/games/new',
        element: <GameSetup />
    },
    {
        path: '/games/:id/host',
        element: <GameHost />
    },
    {
        path: '/games/:id/display',
        element: <GameHost mode="public" />
    },
    {
        path: '/games/templates',
        element: <TemplateList />
    },
    {
        path: '/games/templates/new',
        element: <GameTemplateEditor />
    },
    {
        path: '/games/templates/:id',
        element: <GameTemplateEditor />
    }
]; 