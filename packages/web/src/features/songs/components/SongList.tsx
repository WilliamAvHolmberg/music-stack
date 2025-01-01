import { useNavigate } from 'react-router-dom';
import { Button } from '@/shared/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/shared/components/ui/card';
import { useGetApiSongs, useDeleteApiSongsId } from '@/shared/api/hooks/api';
import { SongCategory, SongLanguage } from '../../games/types';
import type { SongResponseDTO } from '@/shared/api/models';

export function SongList() {
    const navigate = useNavigate();
    const { data: songs, refetch } = useGetApiSongs();
    const { mutateAsync: deleteSong } = useDeleteApiSongsId();

    const handleDelete = async (id: number) => {
        try {
            await deleteSong({ id });
            refetch();
        } catch (error) {
            console.error('Failed to delete song:', error);
        }
    };

    return (
        <div className="container mx-auto p-6">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold">Songs</h1>
                <Button onClick={() => navigate('/songs/new')}>Add Song</Button>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {songs?.map((song: SongResponseDTO) => (
                    <Card key={song.id}>
                        <CardHeader>
                            <CardTitle className="flex justify-between items-center">
                                <span>{song.title}</span>
                                <div className="flex gap-2">
                                    <Button 
                                        variant="outline" 
                                        size="sm"
                                        onClick={() => navigate(`/songs/${song.id}/edit`)}
                                    >
                                        Edit
                                    </Button>
                                    <Button 
                                        variant="destructive" 
                                        size="sm"
                                        onClick={() => handleDelete(song.id!)}
                                    >
                                        Delete
                                    </Button>
                                </div>
                            </CardTitle>
                        </CardHeader>
                        <CardContent className="space-y-2">
                            <div><strong>Artist:</strong> {song.artist}</div>
                            <div><strong>First Line:</strong> {song.firstLine}</div>
                            <div><strong>Year:</strong> {song.year}</div>
                            <div><strong>Difficulty:</strong> {song.difficulty}/5</div>
                            <div><strong>Category:</strong> {SongCategory[song.category!]}</div>
                            <div><strong>Language:</strong> {SongLanguage[song.language!]}</div>
                            {song.spotifyId && (
                                <div><strong>Spotify ID:</strong> {song.spotifyId}</div>
                            )}
                        </CardContent>
                    </Card>
                ))}
            </div>
        </div>
    );
} 