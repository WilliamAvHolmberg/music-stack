import { RouteObject } from 'react-router-dom';
import { SongList } from './components/SongList';
import { SongForm } from './components/SongForm';

export const songRoutes: RouteObject[] = [
    {
        path: '/songs',
        element: <SongList />
    },
    {
        path: '/songs/new',
        element: <SongForm />
    },
    {
        path: '/songs/:id/edit',
        element: <SongForm />
    }
]; 