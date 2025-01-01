import { useNavigate } from 'react-router-dom';
import { Button } from '@/shared/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/shared/components/ui/card';
import { useGetApiTemplates, useDeleteApiTemplatesId } from '@/shared/api/hooks/api';
import { Music, Plus, Trash2 } from 'lucide-react';
import { RoundType } from '../../types';
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from "@/shared/components/ui/alert-dialog";
import { toast } from "sonner";

export function TemplateList() {
    const navigate = useNavigate();
    const { data: templates, refetch } = useGetApiTemplates();
    const { mutateAsync: deleteTemplate } = useDeleteApiTemplatesId();

    const handleDelete = async (id: number) => {
        try {
            await deleteTemplate({ id });
            toast.success("Template deleted successfully");
            refetch();
        } catch {
            toast.error("Failed to delete template");
        }
    };

    return (
        <div className="container mx-auto p-6 space-y-6">
            <div className="flex items-center justify-between">
                <h1 className="text-2xl font-bold">Game Templates</h1>
                <Button onClick={() => navigate('/games/templates/new')}>
                    <Plus className="h-4 w-4 mr-2" />
                    New Template
                </Button>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {templates?.map((template) => (
                    <Card key={template.id} className="group">
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="cursor-pointer" onClick={() => navigate(`/games/templates/${template.id}`)}>
                                <Music className="h-4 w-4" />
                                {template.name}
                            </CardTitle>
                            <AlertDialog>
                                <AlertDialogTrigger asChild>
                                    <Button variant="ghost" size="icon" className="opacity-0 group-hover:opacity-100">
                                        <Trash2 className="h-4 w-4" />
                                    </Button>
                                </AlertDialogTrigger>
                                <AlertDialogContent>
                                    <AlertDialogHeader>
                                        <AlertDialogTitle>Delete Template</AlertDialogTitle>
                                        <AlertDialogDescription>
                                            Are you sure you want to delete this template? This action cannot be undone.
                                        </AlertDialogDescription>
                                    </AlertDialogHeader>
                                    <AlertDialogFooter>
                                        <AlertDialogCancel>Cancel</AlertDialogCancel>
                                        <AlertDialogAction onClick={() => handleDelete(template.id)}>
                                            Delete
                                        </AlertDialogAction>
                                    </AlertDialogFooter>
                                </AlertDialogContent>
                            </AlertDialog>
                        </CardHeader>
                        <CardContent onClick={() => navigate(`/games/templates/${template.id}`)} className="cursor-pointer">
                            <div className="text-sm text-muted-foreground">
                                {template.description}
                            </div>
                            <div className="mt-4 flex items-center gap-2 text-sm">
                                <div className="font-medium">
                                    {template.rounds?.length || 0} rounds
                                </div>
                                •
                                <div className="text-muted-foreground">
                                    {template.rounds?.map(round => RoundType[round.type ?? 0]).join(', ')}
                                </div>
                            </div>
                        </CardContent>
                    </Card>
                ))}
            </div>

            {templates?.length === 0 && (
                <div className="text-center py-12">
                    <div className="text-lg text-muted-foreground">
                        No templates found
                    </div>
                    <Button 
                        variant="outline" 
                        className="mt-4"
                        onClick={() => navigate('/games/templates/new')}
                    >
                        Create your first template
                    </Button>
                </div>
            )}
        </div>
    );
} 