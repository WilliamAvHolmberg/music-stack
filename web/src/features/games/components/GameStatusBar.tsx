import { Music2, Play, Pause } from 'lucide-react';
import { Button } from "@/shared/components/ui/button";
import { GameTimer } from "@/shared/components/ui/game-timer";
import { RoundType } from '../types';
import type { RoundResponseDTO } from '@/shared/api/models';

interface GameStatusBarProps {
    currentRound: RoundResponseDTO;
    currentItem: { title: string; artist: string };
    currentTeam: string;
    isPaused: boolean;
    onTimerToggle: () => void;
    onNextItem: () => void;
    onPrevItem: () => void;
}

export function GameStatusBar({ 
    currentRound, 
    currentItem,
    currentTeam,
    isPaused,
    onTimerToggle,
    onNextItem,
    onPrevItem 
}: GameStatusBarProps) {
    return (
        <div className="bg-white/5 border-b border-white/10 backdrop-blur-sm">
            <div className="container flex items-center justify-between h-16 px-4">
                {/* Left: Current Item */}
                <div className="flex items-center gap-4">
                    <div className="flex items-center gap-2">
                        <Music2 className="h-5 w-5 text-muted-foreground" />
                        <div>
                            <div className="font-medium">{currentItem.title}</div>
                            <div className="text-sm text-muted-foreground">{currentItem.artist}</div>
                        </div>
                    </div>
                </div>

                {/* Center: Timer and Controls */}
                <div className="flex flex-col items-center">
                    <GameTimer
                        seconds={currentRound?.timeLeft ?? currentRound?.timeInMinutes * 60}
                        label={`${currentTeam}'s turn`}
                        isPaused={isPaused}
                        className="scale-90"
                    />
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={onTimerToggle}
                        className="mt-2 px-2 py-1"
                    >
                        {isPaused ? <Play className="h-4 w-4 mr-2" /> : <Pause className="h-4 w-4 mr-2" />}
                        {isPaused ? "Resume" : "Pause"}
                    </Button>
                </div>

                {/* Right: Round Info */}
                <div className="text-sm">
                    <div className="font-medium text-primary">{currentRound.title}</div>
                    <div className="flex items-center gap-2 text-muted-foreground">
                        <span className="px-2 py-1 bg-primary/10 rounded text-primary text-xs">
                            {RoundType[currentRound.type]}
                        </span>
                    </div>
                </div>
            </div>
        </div>
    );
} 