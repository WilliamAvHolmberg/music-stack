import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Button } from '@/shared/components/ui/button';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/shared/components/ui/form';
import { Input } from '@/shared/components/ui/input';
import { Textarea } from '@/shared/components/ui/textarea';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '@/shared/components/ui/select';
import { useGetApiSongsId, usePostApiSongs, usePutApiSongsId } from '@/shared/api/hooks/api';
import { SongCategory, SongLanguage } from '../../games/types';
import type { CreateSongRequestDTO } from '@/shared/api/models';

const formSchema = z.object({
    title: z.string().min(1, 'Title is required'),
    artist: z.string().min(1, 'Artist is required'),
    firstLine: z.string().min(1, 'First line is required'),
    year: z.number().min(1900).max(new Date().getFullYear()),
    difficulty: z.number().min(1).max(5),
    category: z.number().min(0),
    language: z.number().min(0),
    spotifyId: z.string().optional()
});

export function SongForm() {
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();
    const isEditing = !!id;
    const [jsonError, setJsonError] = useState<string | null>(null);
    
    const { data: song } = useGetApiSongsId(isEditing ? parseInt(id) : 0, {
        query: { enabled: isEditing }
    });
    const { mutateAsync: createSong } = usePostApiSongs();
    const { mutateAsync: updateSong } = usePutApiSongsId();

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            title: '',
            artist: '',
            firstLine: '',
            year: new Date().getFullYear(),
            difficulty: 3,
            category: 0,
            language: 0,
            spotifyId: ''
        },
    });

    const [jsonInput, setJsonInput] = useState('');

    // Update JSON when form changes
    useEffect(() => {
        const subscription = form.watch((value) => {
            setJsonInput(JSON.stringify(value, null, 2));
        });
        return () => subscription.unsubscribe();
    }, [form.watch]);

    // Update form when JSON changes
    const handleJsonChange = (value: string) => {
        setJsonInput(value);
        try {
            const parsed = JSON.parse(value);
            setJsonError(null);
            
            // Validate against schema
            const result = formSchema.safeParse(parsed);
            if (result.success) {
                form.reset(result.data);
            } else {
                setJsonError(result.error.errors[0]?.message ?? 'Invalid JSON format');
            }
        } catch {
            setJsonError('Invalid JSON format');
        }
    };

    useEffect(() => {
        if (song) {
            const values = {
                title: song.title ?? '',
                artist: song.artist ?? '',
                firstLine: song.firstLine ?? '',
                year: song.year ?? new Date().getFullYear(),
                difficulty: song.difficulty ?? 3,
                category: song.category ?? 0,
                language: song.language ?? 0,
                spotifyId: song.spotifyId ?? ''
            };
            form.reset(values);
            setJsonInput(JSON.stringify(values, null, 2));
        }
    }, [song, form]);

    const onSubmit = async (values: z.infer<typeof formSchema>) => {
        try {
            if (isEditing) {
                await updateSong({
                    id: parseInt(id),
                    data: values as CreateSongRequestDTO
                });
            } else {
                await createSong({
                    data: values as CreateSongRequestDTO
                });
            }
            navigate('/songs');
        } catch {
            console.error('Failed to save song');
        }
    };

    return (
        <div className="container mx-auto p-6">
            <h1 className="text-2xl font-bold mb-6">{isEditing ? 'Edit' : 'Add'} Song</h1>
            
            <div className="grid grid-cols-2 gap-8">
                {/* Form Side */}
                <div>
                    <h2 className="text-lg font-semibold mb-4">Form Input</h2>
                    <Form {...form}>
                        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
                            <FormField
                                control={form.control}
                                name="title"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Title</FormLabel>
                                        <FormControl>
                                            <Input {...field} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="artist"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Artist</FormLabel>
                                        <FormControl>
                                            <Input {...field} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="firstLine"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>First Line</FormLabel>
                                        <FormControl>
                                            <Input {...field} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="year"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Year</FormLabel>
                                        <FormControl>
                                            <Input 
                                                type="number" 
                                                {...field} 
                                                onChange={e => field.onChange(parseInt(e.target.value))}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="difficulty"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Difficulty (1-5)</FormLabel>
                                        <FormControl>
                                            <Input 
                                                type="number" 
                                                min={1} 
                                                max={5} 
                                                {...field}
                                                onChange={e => field.onChange(parseInt(e.target.value))}
                                            />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="category"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Category</FormLabel>
                                        <Select 
                                            onValueChange={val => field.onChange(parseInt(val))}
                                            value={field.value.toString()}
                                        >
                                            <FormControl>
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Select a category" />
                                                </SelectTrigger>
                                            </FormControl>
                                            <SelectContent>
                                                {Object.entries(SongCategory)
                                                    .filter(([key]) => isNaN(Number(key)))
                                                    .map(([key, value]) => (
                                                        <SelectItem key={value} value={value.toString()}>
                                                            {key}
                                                        </SelectItem>
                                                    ))
                                                }
                                            </SelectContent>
                                        </Select>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="language"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Language</FormLabel>
                                        <Select 
                                            onValueChange={val => field.onChange(parseInt(val))}
                                            value={field.value.toString()}
                                        >
                                            <FormControl>
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Select a language" />
                                                </SelectTrigger>
                                            </FormControl>
                                            <SelectContent>
                                                {Object.entries(SongLanguage)
                                                    .filter(([key]) => isNaN(Number(key)))
                                                    .map(([key, value]) => (
                                                        <SelectItem key={value} value={value.toString()}>
                                                            {key}
                                                        </SelectItem>
                                                    ))
                                                }
                                            </SelectContent>
                                        </Select>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="spotifyId"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Spotify ID</FormLabel>
                                        <FormControl>
                                            <Input {...field} placeholder="Optional Spotify track ID" />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />

                            <div className="flex gap-4">
                                <Button type="submit">
                                    {isEditing ? 'Update' : 'Create'} Song
                                </Button>
                                <Button type="button" variant="outline" onClick={() => navigate('/songs')}>
                                    Cancel
                                </Button>
                            </div>
                        </form>
                    </Form>
                </div>

                {/* JSON Side */}
                <div>
                    <h2 className="text-lg font-semibold mb-4">JSON Input</h2>
                    <div className="space-y-4">
                        <Textarea 
                            value={jsonInput}
                            onChange={(e) => handleJsonChange(e.target.value)}
                            className="font-mono h-[600px] bg-slate-900 text-slate-50"
                            placeholder="Paste your JSON here..."
                        />
                        {jsonError && (
                            <div className="text-red-500 text-sm">
                                {jsonError}
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
} 