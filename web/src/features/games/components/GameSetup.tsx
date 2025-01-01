import { useState } from 'react';
import { Button } from '@/shared/components/ui/button';
import { Input } from '@/shared/components/ui/input';
import { Label } from '@/shared/components/ui/label';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/shared/components/ui/card';
import { usePostApiGames, usePostApiGamesGameIdTeams, useGetApiTemplates } from '@/shared/api/hooks/api';
import { useNavigate } from 'react-router-dom';
import type { GameTemplate } from '../types';

const TEAM_COLORS = [
    '#FF0000', // Red
    '#0000FF', // Blue
    '#00FF00', // Green
    '#FFD700'  // Gold
];

export function GameSetup() {
    const navigate = useNavigate();
    const [selectedTemplateId, setSelectedTemplateId] = useState<number | null>(null);
    const [teamNames, setTeamNames] = useState<string[]>(['', '']);
    const { data: templates, isLoading } = useGetApiTemplates();
    const { mutateAsync: createGame } = usePostApiGames();
    const { mutateAsync: addTeam } = usePostApiGamesGameIdTeams();

    const handleAddTeam = () => {
        if (teamNames.length < 4) {
            setTeamNames([...teamNames, '']);
        }
    };

    const handleRemoveTeam = (index: number) => {
        if (teamNames.length > 2) {
            const newTeamNames = teamNames.filter((_, i) => i !== index);
            setTeamNames(newTeamNames);
        }
    };

    const handleTeamNameChange = (index: number, value: string) => {
        const newTeamNames = [...teamNames];
        newTeamNames[index] = value;
        setTeamNames(newTeamNames);
    };

    const handleStartGame = async () => {
        if (!selectedTemplateId) return;

        try {
            const game = await createGame({
                data: {
                    gameTemplateId: selectedTemplateId
                }
            });
            
            // Add teams sequentially
            for (let i = 0; i < teamNames.length; i++) {
                if (teamNames[i].trim()) {
                    await addTeam({
                        gameId: game.id,
                        data: {
                            name: teamNames[i],
                            color: TEAM_COLORS[i]
                        }
                    });
                }
            }

            // Navigate to game host view
            navigate(`/games/${game.id}/host`);
        } catch (error) {
            console.error('Failed to create game:', error);
        }
    };

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (!selectedTemplateId) {
        return (
            <div className="container mx-auto p-6">
                <h1 className="text-2xl font-bold text-center mb-6">Select a Game Template</h1>
                <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
                    {templates?.map((template: GameTemplate) => (
                        <Card 
                            key={template.id} 
                            className="cursor-pointer hover:bg-accent transition-colors"
                            onClick={() => setSelectedTemplateId(template.id)}
                        >
                            <CardHeader>
                                <CardTitle>{template.name}</CardTitle>
                                <CardDescription>{template.description}</CardDescription>
                            </CardHeader>
                            <CardContent>
                                <div className="flex justify-between text-sm text-muted-foreground">
                                    <span>{template.rounds?.length || 0} rounds</span>
                                    <span>{template.isPublic ? "Public" : "Private"}</span>
                                </div>
                            </CardContent>
                        </Card>
                    ))}
                </div>
            </div>
        );
    }

    return (
        <div className="max-w-md mx-auto p-6 space-y-6">
            <h1 className="text-2xl font-bold text-center">Game Setup</h1>
            
            <div className="space-y-4">
                {teamNames.map((name, index) => (
                    <div key={index} className="flex items-center gap-2">
                        <div className="flex-1">
                            <Label htmlFor={`team-${index}`}>Team {index + 1}</Label>
                            <Input
                                id={`team-${index}`}
                                value={name}
                                onChange={(e) => handleTeamNameChange(index, e.target.value)}
                                placeholder={`Enter team ${index + 1} name`}
                            />
                        </div>
                        {teamNames.length > 2 && (
                            <Button
                                variant="destructive"
                                size="icon"
                                onClick={() => handleRemoveTeam(index)}
                                className="mt-6"
                            >
                                ×
                            </Button>
                        )}
                    </div>
                ))}
            </div>

            <div className="flex justify-between">
                <Button
                    variant="outline"
                    onClick={handleAddTeam}
                    disabled={teamNames.length >= 4}
                >
                    Add Team
                </Button>
                <div className="space-x-2">
                    <Button
                        variant="outline"
                        onClick={() => setSelectedTemplateId(null)}
                    >
                        Back
                    </Button>
                    <Button
                        onClick={handleStartGame}
                        disabled={!teamNames.every(name => name.trim())}
                    >
                        Start Game
                    </Button>
                </div>
            </div>
        </div>
    );
} 