import { GameTimer } from '@/shared/components/ui/game-timer';
import type { GameSessionResponseDTO, RoundResponseDTO } from '@/shared/api/models';
import { cn } from '@/shared/utils/utils';
import { getRoundDisplay } from './display/getRoundDisplay';
import { Trophy, Loader2, Music2 } from 'lucide-react';
import { AnimatedScore } from './display/AnimatedScore';
import { useEffect, useState } from 'react';

interface PublicDisplayProps {
    game: GameSessionResponseDTO;
    currentRound?: RoundResponseDTO;
    currentTeamIndex: number;
}

function AnimatedDots() {
    const [dots, setDots] = useState('');
    
    useEffect(() => {
        const interval = setInterval(() => {
            setDots(prev => prev.length >= 3 ? '' : prev + '.');
        }, 500);
        
        return () => clearInterval(interval);
    }, []);
    
    return <span className="inline-block w-8">{dots}</span>;
}

export function PublicDisplay({ game, currentRound, currentTeamIndex }: PublicDisplayProps) {
    // Add this effect to hide/show header
    useEffect(() => {
        // Hide header when component mounts
        document.querySelector('header')?.classList.add('hidden');
        
        // Show header when component unmounts
        return () => {
            document.querySelector('header')?.classList.remove('hidden');
        };
    }, []);

    // Handle game completion

    if (game.status === 0) {
        return (
            <div className="fixed inset-0 bg-gradient-to-br from-blue-900 via-indigo-900 to-purple-900 text-white">
                <div className="flex flex-col items-center justify-center h-full space-y-12">
                    {/* Animated icon with float and pulse */}
                    <div className="relative animate-float">
                        <Music2 className="w-32 h-32 text-white/50 animate-pulse" />
                        <div className="animate-pulse-ring absolute inset-0 rounded-full" />
                    </div>
                    
                    {/* Main title with gradient and float */}
                    <h1 className="text-6xl font-bold text-transparent bg-clip-text bg-gradient-to-r from-blue-400 to-purple-400 animate-pulse animate-float">
                        Musik Quiz
                    </h1>
                    
                    {/* Subtitle with animated dots */}
                    <div className="text-2xl text-white/80 flex items-center animate-fade-in" 
                         style={{ animationDelay: '300ms' }}>
                        Gör er redo att spela<AnimatedDots />
                    </div>
                    
                    {/* Teams preview with enhanced animations */}
                    {game.teams && game.teams.length > 0 && (
                        <div className="space-y-6 animate-fade-in" style={{ animationDelay: '600ms' }}>
                            <div className="text-xl text-white/60 text-center">
                                Dagens magnifika lag
                            </div>
                            <div className="flex gap-8">
                                {game.teams.map((team, index) => (
                                    <div 
                                        key={team.id}
                                        className={cn(
                                            "px-8 py-4 rounded-xl backdrop-blur-md",
                                            "transform transition-all duration-500 ease-out",
                                            "hover-lift hover:bg-white/20",
                                            "animate-fade-in"
                                        )}
                                        style={{ 
                                            backgroundColor: team.color ? `${team.color}20` : 'rgba(255,255,255,0.1)',
                                            borderColor: team.color!,
                                            borderWidth: '2px',
                                            animationDelay: `${800 + index * 200}ms`
                                        }}
                                    >
                                        <span className="text-2xl font-bold block text-center">
                                            {team.name}
                                        </span>
                                        <span className="text-lg text-white/60 block text-center mt-1">
                                            Team {index + 1}
                                        </span>
                                    </div>
                                ))}
                            </div>
                        </div>
                    )}
                </div>
            </div>
        );
    }

    if (!currentRound || game.status === 2) {
        const sortedTeams = game.teams?.sort((a, b) => b.score - a.score);
        
        return (
            <div className="fixed inset-0 bg-gradient-to-br from-blue-900 via-indigo-900 to-purple-900 text-white">
                <div className="flex flex-col items-center justify-center h-full space-y-8">
                    <Trophy className="w-24 h-24 text-yellow-400" />
                    <h1 className="text-6xl font-bold">Game Complete!</h1>
                    <div className="space-y-6">
                        {sortedTeams?.map((team, index) => (
                            <div 
                                key={team.id} 
                                className={cn(
                                    "text-center p-4 rounded-lg",
                                    index === 0 && "bg-yellow-500/20"
                                )}
                            >
                                <div className={cn(
                                    "text-4xl font-bold",
                                    index === 0 ? "text-yellow-400" : "text-white"
                                )}>
                                    {team.name}
                                </div>
                                <div className="text-3xl mt-2">
                                    {team.score} points
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        );
    }

    const currentItem = currentRound.items?.[game.currentItemIndex];

    return (
        <div className="fixed inset-0 bg-gradient-to-br from-blue-900 via-indigo-900 to-purple-900 text-white">
            {/* Header with Teams */}
            <div className="flex justify-between p-6">
                {game.teams?.map((team, index) => (
                    <div 
                        key={team.id}
                        className={cn(
                            "p-6 rounded-lg",
                            "flex flex-col items-center",
                            currentTeamIndex === index && "bg-white/10"
                        )}
                    >
                        <div className="text-2xl font-bold">{team.name}</div>
                        <AnimatedScore 
                            score={team.score} 
                            isActive={currentTeamIndex === index}
                        />
                    </div>
                ))}
            </div>

            {/* Main Content */}
            <div className="flex flex-col items-center justify-center flex-1 p-8">
                {/* Round Title */}
                <h1 className="text-4xl font-bold mb-8">{currentRound.title}</h1>

                {/* Timer */}
                {/* <div className="scale-150 mb-12">
                    <GameTimer
                        seconds={currentRound?.timeLeft ?? currentRound?.timeInMinutes * 60}
                        label={`${game.teams?.[currentTeamIndex]?.name}'s turn`}
                        isPaused={currentRound?.isPaused}
                    />
                </div> */}

                {/* Current Item */}
                {currentItem && getRoundDisplay(currentRound.type, {
                    title: currentItem.title,
                    artist: currentItem.artist,
                    points: currentItem.points,
                    isRevealed: currentItem.isAnswerRevealed,
                    extraInfo: currentItem.extraInfo
                })}
            </div>
        </div>
    );
} 