import { Music2, ChevronLeft, ChevronRight, Trophy, Music } from 'lucide-react';
import { GameCard } from '@/shared/components/ui/game-card';
import { GameButton } from '@/shared/components/ui/game-button';
import { Button } from '@/shared/components/ui/button';
import type { GameSessionResponseDTO, RoundResponseDTO } from '@/shared/api/models';

interface CurrentRoundCardProps {
    game: GameSessionResponseDTO;
    currentRound?: RoundResponseDTO;
    onPrevItem: () => void;
    onNextItem: () => void;
    onEndRound: () => void;
    onRevealAnswer: () => void;
    onOpenSpotify?: (spotifyId: string) => void;
}

export function CurrentRoundCard({
    game,
    currentRound,
    onPrevItem,
    onNextItem,
    onEndRound,
    onRevealAnswer,
    onOpenSpotify
}: CurrentRoundCardProps) {
    const isAnswerRevealed = currentRound?.items?.[game.currentItemIndex]?.isAnswerRevealed ?? false;
    if (!currentRound || game.status === 2) {
        return (
            <GameCard
                title="Game Complete"
                icon={<Trophy className="w-6 h-6" />}
            >
                <div className="space-y-4">
                    <p className="text-center text-muted-foreground">
                        All rounds have been completed!
                    </p>
                    <GameButton
                        onClick={onEndRound}
                        variant="secondary"
                        fullWidth
                    >
                        End Game
                    </GameButton>
                </div>
            </GameCard>
        );
    }

    return (
        <GameCard
            title={currentRound.title || `Round ${game.currentRoundIndex + 1}`}
            icon={<Music2 className="w-6 h-6" />}
        >
            <div className="space-y-4">
                {/* Timer and Controls Row */}
                <div className="flex items-center justify-between">

                    <div className="flex gap-2">
                        <Button
                            variant="outline"
                            onClick={onRevealAnswer}
                        >
                            {isAnswerRevealed ? "Hide Answer" : "Reveal Answer"}
                        </Button>
                        <Button
                            variant="outline"
                            onClick={onNextItem}
                            disabled={!isAnswerRevealed}
                        >
                            Next Item
                        </Button>
                    </div>
                </div>

                {/* Current Item Section */}
                {currentRound.items?.[game.currentItemIndex] && (
                    <div className="space-y-2">
                        <div className="flex items-center justify-between text-sm text-muted-foreground">
                            <span>Item {game.currentItemIndex + 1} of {currentRound.items.length}</span>
                            <div className="flex gap-2">
                                <Button
                                    variant="outline"
                                    size="icon"
                                    onClick={onPrevItem}
                                    disabled={game.currentItemIndex === 0}
                                >
                                    <ChevronLeft className="h-4 w-4" />
                                </Button>
                                <Button
                                    variant="outline"
                                    size="icon"
                                    onClick={onNextItem}
                                    disabled={game.currentItemIndex >= currentRound.items.length - 1}
                                >
                                    <ChevronRight className="h-4 w-4" />
                                </Button>
                            </div>
                        </div>

                        {/* Current Item Card */}
                        <div className="bg-white/10 p-4 rounded-lg">
                            <div className="flex items-center justify-between">
                                <div>
                                    <div className="text-sm text-muted-foreground mb-1">Current Item</div>
                                    <div className="font-medium">{currentRound?.items?.[game.currentItemIndex]?.title}</div>
                                    <div className="text-sm">{currentRound?.items?.[game.currentItemIndex]?.artist}</div>
                                    {currentRound?.items?.[game.currentItemIndex]?.extraInfo && (
                                        <div className="text-sm text-muted-foreground mt-1">
                                            {currentRound?.items?.[game.currentItemIndex]?.extraInfo}
                                        </div>
                                    )}
                                    {currentRound?.items?.[game.currentItemIndex]?.spotifyId && onOpenSpotify && (
                                        <Button
                                            variant="outline"
                                            size="sm"
                                            className="mt-2"
                                            onClick={() => onOpenSpotify(currentRound?.items?.[game.currentItemIndex]?.spotifyId!)}
                                        >
                                            <Music className="w-4 h-4 mr-2" />
                                            Open in Spotify
                                        </Button>
                                    )}
                                </div>
                                <div className="text-2xl font-bold text-yellow-400">
                                    {currentRound?.items?.[game.currentItemIndex]?.points ?? 0} pts
                                </div>
                            </div>
                        </div>
                    </div>
                )}

                {/* End Round Button */}
                <GameButton
                    onClick={onEndRound}
                    variant="secondary"
                    fullWidth
                >
                    {game.currentRoundIndex === (game.rounds?.length ?? 0) - 1 ? "End Game" : "End Round"}
                </GameButton>
            </div>
        </GameCard>
    );
} 