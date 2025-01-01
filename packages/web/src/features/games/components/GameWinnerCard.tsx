import { Trophy } from 'lucide-react';
import { GameCard } from '@/shared/components/ui/game-card';
import type { GameSessionResponseDTO } from '@/shared/api/models';

interface GameWinnerCardProps {
    game: GameSessionResponseDTO;
}

export function GameWinnerCard({ game }: GameWinnerCardProps) {
    return (
        <GameCard title="Game Complete" icon={<Trophy className="w-6 h-6" />}>
            <div className="text-center py-8">
                <h3 className="text-2xl font-bold mb-4">Winner</h3>
                <div className="text-4xl font-bold text-yellow-400">
                    {game.teams?.reduce((prev, curr) =>
                        prev.score > curr.score ? prev : curr
                    ).name}
                </div>
                <div className="mt-8 space-y-2">
                    {game.teams?.sort((a, b) => b.score - a.score)
                        .map(team => (
                            <div key={team.id} className="flex justify-between">
                                <span>{team.name}</span>
                                <span className="font-bold">{team.score} points</span>
                            </div>
                        ))
                    }
                </div>
            </div>
        </GameCard>
    );
} 