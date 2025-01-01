import { Button } from "@/shared/components/ui/button";
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "@/shared/components/ui/form";
import { Input } from "@/shared/components/ui/input";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";

interface ItemFormProps {
  onSubmit: (data: ItemFormValues) => void;
  onCancel: () => void;
  defaultValues?: ItemFormValues;
}

interface ItemFormValues {
  title: string;
  artist: string;
  points: number;
}

const formSchema = z.object({
  title: z.string().min(3, "Title must be at least 3 characters"),
  artist: z.string().min(3, "Artist must be at least 3 characters"),
  points: z.coerce.number().min(1, "Points must be at least 1"),
});

export function ItemForm({ onSubmit, onCancel, defaultValues }: ItemFormProps) {
  const form = useForm<ItemFormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: defaultValues || {
      title: "",
      artist: "",
      points: 1,
    },
  });

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
        <FormField
          control={form.control}
          name="title"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Title</FormLabel>
              <FormControl>
                <Input placeholder="Enter song title" {...field} />
              </FormControl>
              <FormDescription>The title of the song</FormDescription>
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
                <Input placeholder="Enter artist name" {...field} />
              </FormControl>
              <FormDescription>The artist of the song</FormDescription>
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
                  placeholder="Enter points value"
                  {...field}
                />
              </FormControl>
              <FormDescription>Points awarded for this item</FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="flex justify-end gap-4">
          <Button type="button" variant="outline" onClick={onCancel}>
            Cancel
          </Button>
          <Button type="submit">Save Item</Button>
        </div>
      </form>
    </Form>
  );
} 