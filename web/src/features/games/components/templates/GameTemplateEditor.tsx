import { useState, useEffect, useRef } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Button } from '@/shared/components/ui/button';
import { Input } from '@/shared/components/ui/input';
import { Label } from '@/shared/components/ui/label';
import { Switch } from '@/shared/components/ui/switch';
import { Card, CardContent } from '@/shared/components/ui/card';
import {
    Tooltip,
    TooltipContent,
    TooltipProvider,
    TooltipTrigger,
} from "@/shared/components/ui/tooltip";
import { Collapsible, CollapsibleTrigger, CollapsibleContent } from '@/shared/components/ui/collapsible';
import { Music, Pencil } from 'lucide-react';

import { RoundForm } from './RoundForm';
import { RoundItemForm } from './RoundItemForm';
import { RoundType } from '../../types';
import {
    useGetApiTemplatesId,
    usePostApiTemplates,
    usePutApiTemplatesId,
} from '@/shared/api/hooks/api';
import type { 
    CreateGameTemplateRequestDTO, 
    CreateTemplateRoundRequestDTO, 
    CreateTemplateRoundItemRequestDTO,
    RoundResponseDTO 
} from '@/shared/api/models';

interface EditingItem {
    roundIndex: number;
    itemIndex: number | null;
    item: CreateTemplateRoundItemRequestDTO;
}

interface EditingRound {
    index: number;
    title: string;
}

export function GameTemplateEditor() {
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();
    const isEditing = !!id;

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [isPublic, setIsPublic] = useState(false);
    const [rounds, setRounds] = useState<CreateTemplateRoundRequestDTO[]>([]);
    const [isAddingRound, setIsAddingRound] = useState(false);
    const [editingItem, setEditingItem] = useState<EditingItem | null>(null);
    const [editingRound, setEditingRound] = useState<EditingRound | null>(null);

    const { data: template } = useGetApiTemplatesId(isEditing ? parseInt(id) : 0, {
        query: { enabled: isEditing }
    });
    const { mutateAsync: createTemplate } = usePostApiTemplates();
    const { mutateAsync: updateTemplate } = usePutApiTemplatesId();

    // Load template data
    useEffect(() => {
        if (template) {
            setName(template.name ?? '');
            setDescription(template.description ?? '');
            setIsPublic(template.isPublic ?? false);
            setRounds(template.rounds?.map((round: RoundResponseDTO) => ({
                title: round.title ?? '',
                type: round.type ?? RoundType.GuessTheMelody,
                timeInMinutes: round.timeInMinutes ?? 3,
                orderIndex: round.orderIndex,
                instructions: round.instructions ?? '',
                items: round.items?.map(item => ({
                    title: item.title ?? '',
                    artist: item.artist ?? '',
                    points: item.points ?? 1,
                    extraInfo: item.extraInfo ?? undefined,
                    spotifyId: item.spotifyId ?? undefined,
                    orderIndex: item.orderIndex
                })) ?? []
            })) ?? []);
        }
    }, [template]);

    const handleSave = async () => {
        const templateData: CreateGameTemplateRequestDTO = {
            name,
            description,
            isPublic,
            rounds: rounds.map((round, roundIndex) => ({
                ...round,
                orderIndex: roundIndex,
                items: round.items?.map((item, itemIndex) => ({
                    ...item,
                    orderIndex: itemIndex
                }))
            }))
        };

        try {
            if (isEditing) {
                await updateTemplate({
                    id: parseInt(id),
                    data: templateData
                });
            } else {
                await createTemplate({
                    data: templateData
                });
            }
            navigate('/games/templates');
        } catch (error) {
            console.error('Failed to save template:', error);
        }
    };

    const handleAddRound = (round: CreateTemplateRoundRequestDTO) => {
        setRounds([...rounds, round]);
        setIsAddingRound(false);
    };

    const handleRemoveRound = (index: number) => {
        setRounds(rounds.filter((_, i) => i !== index));
    };

    const handleAddItem = (roundIndex: number) => {
        setEditingItem({
            roundIndex,
            itemIndex: null,
            item: {
                title: "",
                artist: "",
                points: 1,
                extraInfo: ""
            }
        });
    };

    const handleRemoveItem = (roundIndex: number, itemIndex: number) => {
        setRounds(rounds.map((round, index) => 
            index === roundIndex
                ? { ...round, items: round.items?.filter((_, i) => i !== itemIndex) }
                : round
        ));
    };

    const handleSaveItem = (item: CreateTemplateRoundItemRequestDTO) => {
        if (!editingItem) return;

        const { roundIndex, itemIndex } = editingItem;
        
        setRounds(rounds.map((round, i) => {
            if (i !== roundIndex) return round;

            const items = [...(round.items ?? [])];
            if (itemIndex === null) {
                items.push(item);
            } else {
                items[itemIndex] = item;
            }

            return { ...round, items };
        }));

        setEditingItem(null);
    };

    const handleEditItem = (roundIndex: number, itemIndex: number, item: CreateTemplateRoundItemRequestDTO) => {
        setEditingItem({
            roundIndex,
            itemIndex,
            item
        });
    };

    const handleUpdateRound = (index: number, updates: Partial<CreateTemplateRoundRequestDTO>) => {
        setRounds(rounds.map((round, i) => 
            i === index ? { ...round, ...updates } : round
        ));
    };

    const getDisabledReason = () => {
        if (!name) return "Please enter a template name";
        if (rounds.length === 0) return "Add at least one round";
        return null;
    };

    return (
        <div className="container mx-auto p-6 space-y-6">
            <h1 className="text-2xl font-bold mb-6">
                {isEditing ? 'Edit Game Template' : 'New Game Template'}
            </h1>

            {/* Template Details */}
            <Card>
                <CardContent className="space-y-4">
                    <div className="space-y-2">
                        <Label htmlFor="name">Name</Label>
                        <Input
                            id="name"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            placeholder="Enter template name..."
                        />
                    </div>

                    <div className="space-y-2">
                        <Label htmlFor="description">Description</Label>
                        <Input
                            id="description"
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            placeholder="Enter template description..."
                        />
                    </div>

                    <div className="flex items-center space-x-2">
                        <Switch
                            id="isPublic"
                            checked={isPublic}
                            onCheckedChange={setIsPublic}
                        />
                        <Label htmlFor="isPublic">Make this template available to other users</Label>
                    </div>
                </CardContent>
            </Card>

            {/* Rounds Section */}
            <div className="space-y-4">
                <div className="flex justify-between items-center">
                    <h2 className="text-lg font-semibold">Rounds</h2>
                    <Button onClick={() => setIsAddingRound(true)}>
                        Add Round
                    </Button>
                </div>

                {isAddingRound && (
                    <Card>
                        <CardContent className="py-4">
                            <RoundForm
                                onSave={handleAddRound}
                                onCancel={() => setIsAddingRound(false)}
                            />
                        </CardContent>
                    </Card>
                )}

                {/* Rounds List */}
                <div className="space-y-4">
                    {rounds.map((round, index) => (
                        <Card key={index}>
                            <CardContent>
                                <Collapsible>
                                    <CollapsibleTrigger className="w-full">
                                        <div className="flex justify-between items-center p-2">
                                            <div className="flex items-center gap-2">
                                                {editingRound?.index === index ? (
                                                    <Input
                                                        value={editingRound.title}
                                                        onChange={(e) => setEditingRound({ 
                                                            ...editingRound, 
                                                            title: e.target.value 
                                                        })}
                                                        onKeyDown={(e) => {
                                                            if (e.key === 'Enter') {
                                                                handleUpdateRound(index, { title: editingRound.title });
                                                                setEditingRound(null);
                                                            } else if (e.key === 'Escape') {
                                                                setEditingRound(null);
                                                            }
                                                        }}
                                                        onBlur={() => {
                                                            handleUpdateRound(index, { title: editingRound.title });
                                                            setEditingRound(null);
                                                        }}
                                                        autoFocus
                                                        className="w-48"
                                                    />
                                                ) : (
                                                    <>
                                                        <div className="font-medium">{round.title}</div>
                                                        <Button
                                                            variant="ghost"
                                                            size="sm"
                                                            className="h-6 w-6 p-0"
                                                            onClick={(e) => {
                                                                e.stopPropagation();
                                                                setEditingRound({ 
                                                                    index, 
                                                                    title: round.title 
                                                                });
                                                            }}
                                                        >
                                                            <Pencil className="h-3 w-3" />
                                                        </Button>
                                                    </>
                                                )}
                                            </div>
                                            <div className="flex items-center gap-2">
                                                <div className="text-sm text-muted-foreground">
                                                    {round.items?.length ?? 0} items • {round.timeInMinutes} minutes
                                                </div>
                                                <Button
                                                    variant="ghost"
                                                    size="sm"
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        handleRemoveRound(index);
                                                    }}
                                                >
                                                    Remove
                                                </Button>
                                            </div>
                                        </div>
                                    </CollapsibleTrigger>

                                    <CollapsibleContent>
                                        {/* Round Items */}
                                        <div className="mt-4 space-y-4">
                                            <div className="flex justify-between items-center">
                                                <h3 className="text-sm font-medium">Items</h3>
                                                <Button
                                                    variant="outline"
                                                    size="sm"
                                                    onClick={() => handleAddItem(index)}
                                                >
                                                    Add Item
                                                </Button>
                                            </div>
                                            {round.items?.map((item, itemIndex) => (
                                                <Card 
                                                    key={itemIndex} 
                                                    className="cursor-pointer hover:bg-accent/50 transition-colors"
                                                    onClick={() => handleEditItem(index, itemIndex, item)}
                                                >
                                                    <CardContent className="py-2">
                                                        <div className="flex items-center justify-between">
                                                            <div>
                                                                <div className="font-medium">{item.title}</div>
                                                                <div className="text-sm text-muted-foreground">
                                                                    {item.artist} • {item.points} points
                                                                </div>
                                                                {item.extraInfo && (
                                                                    <div className="text-sm text-muted-foreground mt-1">
                                                                        {item.extraInfo}
                                                                    </div>
                                                                )}
                                                                {item.spotifyId && (
                                                                    <div className="text-sm text-muted-foreground flex items-center gap-1 mt-1">
                                                                        <Music className="h-3 w-3" />
                                                                        {item.spotifyId}
                                                                    </div>
                                                                )}
                                                            </div>
                                                            <Button
                                                                variant="ghost"
                                                                size="sm"
                                                                onClick={(e) => {
                                                                    e.stopPropagation();
                                                                    handleRemoveItem(index, itemIndex);
                                                                }}
                                                            >
                                                                Remove
                                                            </Button>
                                                        </div>
                                                    </CardContent>
                                                </Card>
                                            ))}
                                        </div>
                                        
                                        {editingItem && editingItem.roundIndex === index && (
                                            <Card className="mt-4">
                                                <CardContent className="py-4">
                                                    <RoundItemForm
                                                        initialValues={editingItem.item as any}
                                                        onSave={handleSaveItem}
                                                        onCancel={() => setEditingItem(null)}
                                                    />
                                                </CardContent>
                                            </Card>
                                        )}
                                    </CollapsibleContent>
                                </Collapsible>
                            </CardContent>
                        </Card>
                    ))}
                </div>
            </div>

            {/* Save/Cancel Buttons */}
            <div className="flex justify-end gap-2 pt-4 border-t">
                <Button
                    variant="outline"
                    onClick={() => navigate('/games/templates')}
                >
                    Cancel
                </Button>
                <TooltipProvider>
                    <Tooltip>
                        <TooltipTrigger asChild>
                            <span>
                                <Button
                                    onClick={handleSave}
                                    disabled={!name || rounds.length === 0}
                                >
                                    {isEditing ? 'Update' : 'Create'} Template
                                </Button>
                            </span>
                        </TooltipTrigger>
                        {getDisabledReason() && (
                            <TooltipContent>
                                <p>{getDisabledReason()}</p>
                            </TooltipContent>
                        )}
                    </Tooltip>
                </TooltipProvider>
            </div>
        </div>
    );
} 