import { useParams } from 'react-router-dom';
import {
    useGetApiGamesId,
    usePostApiGamesIdStart,
    usePostApiGamesGameIdRounds,
    usePostApiGamesGameIdRoundsEnd,
    usePutApiGamesGameIdTeamsTeamIdScore,
    usePutApiGamesGameIdItemsNext,
    usePutApiGamesGameIdItemsPrevious,
    usePutApiGamesGameIdItemsItemIdRevealToggle
} from '@/shared/api/hooks/api';
import type { TeamResponseDTO } from '@/shared/api/models';
import { Input } from '@/shared/components/ui/input';
import { GameLayout } from '@/shared/components/ui/game-layout';
import { GameCard } from '@/shared/components/ui/game-card';
import { GameButton } from '@/shared/components/ui/game-button';
import { useToast } from "@/shared/hooks/use-toast";
import { QuickScoreButton } from './QuickScoreButton';
import { GameWinnerCard } from './GameWinnerCard';
import { CurrentRoundCard } from './CurrentRoundCard';
import { RoundOverview } from './RoundOverview';
import { Users, ExternalLink } from 'lucide-react';
import { PublicDisplay } from './PublicDisplay';
import { useGameSubscription } from '@/shared/hooks/useGameSubscription';

interface GameHostProps {
    mode?: 'default' | 'public';
}

const openInSpotify = (spotifyId: string) => {
    const isMobile = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);

    if (isMobile) {
        // Mobile: Open in new window first, then redirect to Spotify app
        window.open(`spotify://track/${spotifyId}?play=true`, '_blank');
    } else {
        // Desktop: Open in new window
        window.open(`spotify:track:${spotifyId}?play=true`, '_blank');
    }
};

export function GameHost({ mode = 'default' }: GameHostProps) {
    const { id } = useParams<{ id: string }>();
    const gameId = parseInt(id!);

    const { data: game, refetch: refetchGame } = useGetApiGamesId(gameId);
    const { mutateAsync: startGame } = usePostApiGamesIdStart();
    const { mutateAsync: startNewRound } = usePostApiGamesGameIdRounds();
    const { mutateAsync: endCurrentRound } = usePostApiGamesGameIdRoundsEnd();
    const { mutateAsync: updateTeamScore } = usePutApiGamesGameIdTeamsTeamIdScore();
    const { mutateAsync: nextItem } = usePutApiGamesGameIdItemsNext();
    const { mutateAsync: prevItem } = usePutApiGamesGameIdItemsPrevious();
    const { mutateAsync: revealAnswer } = usePutApiGamesGameIdItemsItemIdRevealToggle()

    const currentRound = game?.rounds?.find(r => r.status === 1);

    const { toast } = useToast();

    // Subscribe to game changes
    useGameSubscription({
        gameId,
        onGameChanged: () => {
            console.log('Game changed, refetching...');
            refetchGame();
        }
    });

    // Add reveal handler
    const handleRevealAnswer = async () => {
        if (!game || !currentRound || !currentRound.items || !currentRound.items[game.currentItemIndex]) return;
        try {
            await revealAnswer({ gameId, itemId: currentRound.items[game.currentItemIndex]!.id });
            toast({
                title: "Answer revealed",
                description: "The answer has been revealed to all players"
            });
            refetchGame();
        } catch (error) {
            toast({
                variant: "destructive",
                title: "Error",
                description: "Failed to reveal answer"
            });
            console.error('Failed to reveal answer:', error);
        }
    };

    if (!game) {
        return <div>Loading...</div>;
    }

    const handleStartGame = async () => {
        try {
            // Start the game
            await startGame({ id: gameId });

            // Get fresh game data
            const { data: updatedGame } = await refetchGame();

            // Automatically start first round
            const firstRound = updatedGame?.rounds?.[0];
            if (firstRound) {
                await startNewRound({
                    gameId,
                    data: {
                        type: firstRound.type,
                        title: firstRound.title || '',
                        timeInMinutes: firstRound.timeInMinutes,
                        instructions: firstRound.instructions || ''
                    }
                });
                toast({
                    title: "Game started",
                    description: `Starting ${firstRound.title || 'first round'}`
                });
            }
            refetchGame();
        } catch (error) {
            toast({
                variant: "destructive",
                title: "Error",
                description: "Failed to start game"
            });
            console.error('Failed to start game:', error);
        }
    };

    const handleEndRound = async () => {
        try {
            await endCurrentRound({ gameId });

            // Check if there are more rounds before automatically starting next round
            const { data: updatedGame } = await refetchGame();
            const nextRound = updatedGame?.rounds?.find(r => r.status === 0);

            if (nextRound) {
                await startNewRound({
                    gameId,
                    data: {
                        type: nextRound.type,
                        title: nextRound.title || '',
                        timeInMinutes: nextRound.timeInMinutes,
                        instructions: nextRound.instructions || ''
                    }
                });
                toast({
                    title: "Next round started",
                    description: `Starting ${nextRound.title || 'next round'}`
                });
            } else {
                toast({
                    title: "Game completed",
                    description: "All rounds have been completed"
                });
            }

            refetchGame();
        } catch (error) {
            toast({
                variant: "destructive",
                title: "Error",
                description: "Failed to end round"
            });
            console.error('Failed to end round:', error);
        }
    };

    const handleUpdateScore = async (teamId: number, newScore: number) => {
        try {
            await updateTeamScore({ gameId, teamId, data: newScore });
            refetchGame();
        } catch (error) {
            toast({
                variant: "destructive",
                title: "Error",
                description: "Failed to update score"
            });
            console.error('Failed to update score:', error);
        }
    };

    const currentTeamIndex = game.currentItemIndex % (game.teams?.length || 1);
    const completedRounds = game.rounds?.filter(r => r.status === 2).length || 0;

    // Add navigation handlers
    const handleNextItem = async () => {
        if (!currentRound?.items || game.currentItemIndex >= currentRound.items.length - 1) {
            return;
        }

        try {
            // Reset the reveal state before moving to next item
            if (currentRound.isAnswerRevealed) {
                await handleRevealAnswer(); // This will toggle it back to false
            }
            await nextItem({ gameId });
            toast({
                title: "Next item",
                description: "Moving to next item"
            });
            await refetchGame();
        } catch (error) {
            toast({
                variant: "destructive",
                title: "Error",
                description: "Failed to navigate to next item"
            });
            console.error('Failed to navigate to next item:', error);
        }
    };

    const handlePrevItem = async () => {
        try {
            await prevItem({ gameId });
            refetchGame();
        } catch (error) {
            toast({
                variant: "destructive",
                title: "Error",
                description: "Failed to navigate to previous item"
            });
            console.error('Failed to navigate to previous item:', error);
        }
    };

    const isGameComplete = game.status === 2 || !currentRound;

    if (mode === 'public') {
        return (
            <PublicDisplay
                game={game}
                currentRound={currentRound}
                currentTeamIndex={currentTeamIndex}
            />
        );
    }

    return (
        <GameLayout
            title="Så ska det låta"
            stars={completedRounds}
            maxStars={game.rounds?.length || 0}
            layout="split"
            action={
                <GameButton
                    size="sm"
                    onClick={() => window.open(`/games/${gameId}/display`, '_blank')}
                >
                    <ExternalLink className="w-4 h-4 mr-2" />
                    Public View
                </GameButton>
            }
        >
            <div className="space-y-4">
                {/* Teams Section */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    {game.teams?.map((team: TeamResponseDTO) => (
                        <GameCard
                            key={team.id}
                            title={team.name!}
                            score={team.score}
                            isActive={currentTeamIndex === game.teams?.indexOf(team)}
                            color={team.color || undefined}
                            icon={<Users className="w-6 h-6" />}
                        >
                            <div className="space-y-2">
                                <Input
                                    type="number"
                                    value={team.score}
                                    onChange={(e) => handleUpdateScore(team.id, parseInt(e.target.value))}
                                    className="w-full bg-indigo-900/50 border-indigo-700"
                                />
                                <div className="flex gap-1 justify-center">
                                    {[-2, -1, 1, 2].map((value) => (
                                        <QuickScoreButton
                                            key={value}
                                            value={value}
                                            onClick={() => handleUpdateScore(team.id, team.score + value)}
                                        />
                                    ))}
                                </div>
                            </div>
                        </GameCard>
                    ))}
                </div>

                {/* Game State Cards */}
                {game.status === 0 ? (
                    <GameCard title="Start Game">
                        <GameButton onClick={handleStartGame} variant="primary" size="lg" fullWidth>
                            Start Game
                        </GameButton>
                    </GameCard>
                ) : isGameComplete ? (
                    <GameWinnerCard game={game} />
                ) : (
                    <CurrentRoundCard
                        game={game}
                        currentRound={currentRound}
                        onPrevItem={handlePrevItem}
                        onNextItem={handleNextItem}
                        onEndRound={handleEndRound}
                        onRevealAnswer={handleRevealAnswer}
                        onOpenSpotify={openInSpotify}
                    />
                )}

                {/* Round Overview */}
                <RoundOverview
                    rounds={game.rounds || []}
                    currentRoundIndex={game.currentRoundIndex}
                />
            </div>
        </GameLayout>
    );
} 