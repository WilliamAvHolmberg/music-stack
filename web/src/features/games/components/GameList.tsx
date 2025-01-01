import { useNavigate } from "react-router-dom";
import { Plus, Trash2, Users, Timer, MonitorPlay, Laptop2 } from "lucide-react";
import { useGetApiGames, useDeleteApiGamesId } from "@/shared/api/hooks/api";
import { Button } from "@/shared/components/ui/button";
import { Card, CardHeader, CardTitle, CardContent } from "@/shared/components/ui/card";
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
import { GameStatus } from "../types";

export function GameList() {
    const navigate = useNavigate();
    const { data: games, refetch } = useGetApiGames();
    const { mutateAsync: deleteGame } = useDeleteApiGamesId();

    const handleDelete = async (id: number) => {
        try {
            await deleteGame({ id });
            toast.success("Game deleted successfully");
            refetch();
        } catch (error: any) {
            toast.error(error.response?.data || "Failed to delete game");
        }
    };

    return (
        <div className="container mx-auto p-6 space-y-6">
            <div className="flex items-center justify-between">
                <h1 className="text-2xl font-bold">Games</h1>
                <Button onClick={() => navigate('/games/new')}>
                    <Plus className="h-4 w-4 mr-2" /> New Game
                </Button>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {games?.map((game) => (
                    <Card key={game.id} className="group">
                        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                            <CardTitle className="cursor-pointer">
                                Game #{game.id}
                            </CardTitle>
                            <AlertDialog>
                                <AlertDialogTrigger asChild>
                                    <Button 
                                        variant="ghost" 
                                        size="icon" 
                                        className="opacity-0 group-hover:opacity-100"
                                    >
                                        <Trash2 className="h-4 w-4" />
                                    </Button>
                                </AlertDialogTrigger>
                                <AlertDialogContent>
                                    <AlertDialogHeader>
                                        <AlertDialogTitle>Delete Game</AlertDialogTitle>
                                        <AlertDialogDescription>
                                            Are you sure you want to delete this game? This action cannot be undone.
                                        </AlertDialogDescription>
                                    </AlertDialogHeader>
                                    <AlertDialogFooter>
                                        <AlertDialogCancel>Cancel</AlertDialogCancel>
                                        <AlertDialogAction onClick={() => handleDelete(game.id)}>
                                            Delete
                                        </AlertDialogAction>
                                    </AlertDialogFooter>
                                </AlertDialogContent>
                            </AlertDialog>
                        </CardHeader>
                        <CardContent className="cursor-pointer">
                            <div className="flex items-center gap-4 text-sm text-muted-foreground">
                                <div className="flex items-center gap-1">
                                    <Users className="h-4 w-4" />
                                    <span>{game.teams?.length || 0} teams</span>
                                </div>
                                <div className="flex items-center gap-1">
                                    <Timer className="h-4 w-4" />
                                    <span>{game.rounds?.length || 0} rounds</span>
                                </div>
                            </div>
                            <div className="mt-2 text-sm font-medium">
                                {GameStatus[game.status]}
                            </div>
                            <div className="mt-4 flex gap-2">
                                <Button 
                                    variant="secondary" 
                                    className="flex-1"
                                    onClick={() => navigate(`/games/${game.id}/host`)}
                                >
                                    <Laptop2 className="h-4 w-4 mr-2" />
                                    Control Panel
                                </Button>
                                <Button 
                                    variant="secondary" 
                                    className="flex-1"
                                    onClick={() => navigate(`/games/${game.id}/display`)}
                                >
                                    <MonitorPlay className="h-4 w-4 mr-2" />
                                    Public View
                                </Button>
                            </div>
                        </CardContent>
                    </Card>
                ))}
            </div>
        </div>
    );
} 