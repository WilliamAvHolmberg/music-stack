import { useNavigate } from "react-router-dom";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import * as z from "zod";
import { Button } from "@/shared/components/ui/button";
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "@/shared/components/ui/form";
import { Input } from "@/shared/components/ui/input";
import { Textarea } from "@/shared/components/ui/textarea";
import { Switch } from "@/shared/components/ui/switch";
import { usePostApiTemplates } from "@/shared/api/hooks/api";
import { toast } from "sonner";
import type { CreateGameTemplateRequest } from "../../types";

const formSchema = z.object({
  name: z.string().min(3, "Name must be at least 3 characters"),
  description: z.string().min(10, "Description must be at least 10 characters"),
  isPublic: z.boolean(),
});

type FormValues = z.infer<typeof formSchema>;

export function TemplateCreate() {
  const navigate = useNavigate();
  const { mutateAsync: createTemplate } = usePostApiTemplates();

  const form = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      name: "",
      description: "",
      isPublic: false,
    },
  });

  async function onSubmit(values: FormValues) {
    try {
      const request: CreateGameTemplateRequest = {
        name: values.name,
        description: values.description,
        isPublic: values.isPublic,
      };

      const response = await createTemplate({ data: request });
      toast.success("Template created successfully");
      navigate(`/games/templates/${response.id}`);
    } catch (error) {
      toast.error("Failed to create template");
    }
  }

  return (
    <div className="container mx-auto py-8 max-w-2xl">
      <div className="mb-8">
        <h1 className="text-3xl font-bold">Create Game Template</h1>
        <p className="text-muted-foreground">Create a new game template to get started</p>
      </div>

      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
          <FormField
            control={form.control}
            name="name"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Name</FormLabel>
                <FormControl>
                  <Input placeholder="Enter template name" {...field} />
                </FormControl>
                <FormDescription>
                  A descriptive name for your game template
                </FormDescription>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="description"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Description</FormLabel>
                <FormControl>
                  <Textarea
                    placeholder="Enter template description"
                    className="resize-none"
                    {...field}
                  />
                </FormControl>
                <FormDescription>
                  A brief description of what this template is about
                </FormDescription>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="isPublic"
            render={({ field }) => (
              <FormItem className="flex flex-row items-center justify-between rounded-lg border p-4">
                <div className="space-y-0.5">
                  <FormLabel className="text-base">Public Template</FormLabel>
                  <FormDescription>
                    Make this template available to other users
                  </FormDescription>
                </div>
                <FormControl>
                  <Switch
                    checked={field.value}
                    onCheckedChange={field.onChange}
                  />
                </FormControl>
              </FormItem>
            )}
          />

          <div className="flex justify-end gap-4">
            <Button
              type="button"
              variant="outline"
              onClick={() => navigate("/games/templates")}
            >
              Cancel
            </Button>
            <Button type="submit">Create Template</Button>
          </div>
        </form>
      </Form>
    </div>
  );
} 