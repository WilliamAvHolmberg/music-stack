import { Music } from 'lucide-react';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '@/shared/components/ui/select';
import { useGetApiSongs } from '@/shared/api/hooks/api';
import { SongCategory } from '../../types';

interface SongSelectorProps {
    onSelect: (song: { title: string; artist: string; firstLine?: string | null; spotifyId?: string | null }) => void;
}

export function SongSelector({ onSelect }: SongSelectorProps) {
    const { data: songs = [] } = useGetApiSongs();

    return (
        <Select
            onValueChange={(value) => {
                const song = songs.find(s => s.id?.toString() === value);
                if (song) {
                    onSelect({
                        title: song.title || '',
                        artist: song.artist || '',
                        firstLine: song.firstLine,
                        spotifyId: song.spotifyId
                    });
                }
            }}
        >
            <SelectTrigger>
                <SelectValue placeholder="Select a song" />
            </SelectTrigger>
            <SelectContent>
                {songs.map((song) => (
                    <SelectItem 
                        key={song.id} 
                        value={song.id?.toString() || ''}
                        className="py-2"
                    >
                        <div className="flex items-center gap-2">
                            <Music className="h-4 w-4" />
                            <div>
                                <div className="font-medium">{song.title || 'Untitled'}</div>
                                <div className="text-sm text-muted-foreground">
                                    {song.artist || 'Unknown Artist'}
                                    {song.year && ` • ${song.year}`}
                                    {song.category !== undefined && ` • ${SongCategory[song.category]}`}
                                </div>
                            </div>
                        </div>
                    </SelectItem>
                ))}
            </SelectContent>
        </Select>
    );
} 