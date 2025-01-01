import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { Button } from "@/shared/components/ui/button";
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/shared/components/ui/form";
import { Input } from "@/shared/components/ui/input";
import { SongSelector } from "./SongSelector";

const formSchema = z.object({
    title: z.string().min(1, "Title is required"),
    artist: z.string().min(1, "Artist is required"),
    points: z.number().min(1).max(5),
    extraInfo: z.string().optional(),
    spotifyId: z.string().optional()
});

type FormValues = z.infer<typeof formSchema>;

interface RoundItemFormProps {
    initialValues?: FormValues;
    onSave: (values: FormValues) => void;
    onCancel: () => void;
}

export function RoundItemForm({ initialValues, onSave, onCancel }: RoundItemFormProps) {
    const form = useForm<FormValues>({
        resolver: zodResolver(formSchema),
        defaultValues: initialValues || {
            title: "",
            artist: "",
            points: 1,
            extraInfo: "",
            spotifyId: ""
        },
    });

    const handleSongSelect = (song: { 
        title: string; 
        artist: string; 
        firstLine?: string | null;
        spotifyId?: string | null;
    }) => {
        form.setValue("title", song.title);
        form.setValue("artist", song.artist);
        if (song.firstLine) {
            form.setValue("extraInfo", song.firstLine);
        }
        if (song.spotifyId) {
            form.setValue("spotifyId", song.spotifyId);
        }
    };

    return (
        <Form {...form}>
            <form onSubmit={form.handleSubmit(onSave)} className="space-y-4">
                <div className="mb-4">
                    <FormLabel>Select Song</FormLabel>
                    <SongSelector onSelect={handleSongSelect} />
                </div>

                <FormField
                    control={form.control}
                    name="title"
                    render={({ field }) => (
                        <FormItem>
                            <FormLabel>Title</FormLabel>
                            <FormControl>
                                <Input placeholder="Song title..." {...field} />
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
                                <Input placeholder="Artist name..." {...field} />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={form.control}
                    name="points"
                    render={({ field }) => (
                        <FormItem>
                            <FormLabel>Points</FormLabel>
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
                    name="extraInfo"
                    render={({ field }) => (
                        <FormItem>
                            <FormLabel>Extra Info</FormLabel>
                            <FormControl>
                                <Input placeholder="Additional information..." {...field} />
                            </FormControl>
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
                                <Input 
                                    placeholder="Optional Spotify track ID..." 
                                    {...field} 
                                    value={field.value || ''}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <div className="flex justify-end gap-2">
                    <Button type="button" variant="outline" onClick={onCancel}>
                        Cancel
                    </Button>
                    <Button type="submit">Save Item</Button>
                </div>
            </form>
        </Form>
    );
} 