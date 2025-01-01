import { useState } from "react";
import { Plus, GripVertical, Music2 } from "lucide-react";
import { Button } from "@/shared/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/shared/components/ui/card";
import { toast } from "sonner";
import { RoundTypeDTO } from "@/shared/api/models";
import type { RoundResponseDTO, CreateRoundRequestDTO } from "@/shared/api/models";
import { RoundForm } from "./RoundForm";
import { RoundItemList } from "./RoundItemList";
import { RoundItemForm } from "./RoundItemForm";
import { DragDropContext, Droppable, Draggable, DropResult, DroppableProvided, DraggableProvided } from "@hello-pangea/dnd";
import { Collapsible, CollapsibleContent, CollapsibleTrigger } from "@/shared/components/ui/collapsible";

interface RoundListProps {
    templateId: number;
    rounds: RoundResponseDTO[];
}

export function RoundList({ templateId, rounds }: RoundListProps) {
    const [isCreating, setIsCreating] = useState(false);
    const [expandedRoundId, setExpandedRoundId] = useState<number | null>(null);
    const [isAddingItem, setIsAddingItem] = useState(false);

    const handleAddItem = (roundId: number) => {
        setExpandedRoundId(roundId);
        setIsAddingItem(true);
    };

    const handleRemoveItem = async (roundId: number, itemId: number) => {
        // TODO: Implement remove item API call
    };

    const handleDragEnd = async (result: DropResult) => {
        if (!result.destination) return;
        const sourceIndex = result.source.index;
        const destinationIndex = result.destination.index;
        if (sourceIndex === destinationIndex) return;

        try {
            const roundId = rounds[sourceIndex].id;
            toast.success("Round reordered successfully");
        } catch (error) {
            toast.error("Failed to reorder round");
        }
    };

    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h2 className="text-2xl font-bold">Rounds</h2>
                    <p className="text-muted-foreground">Manage the rounds in your template</p>
                </div>
                {!isCreating && (
                    <Button onClick={() => setIsCreating(true)}>
                        <Plus className="mr-2 h-4 w-4" />
                        Add Round
                    </Button>
                )}
            </div>

            {isCreating && (
                <Card>
                    <CardHeader>
                        <CardTitle>New Round</CardTitle>
                        <CardDescription>Configure your new round</CardDescription>
                    </CardHeader>
                    <CardContent>
                        <RoundForm
                            onSubmit={handleCreateRound}
                            onCancel={() => setIsCreating(false)}
                        />
                    </CardContent>
                </Card>
            )}

            <DragDropContext onDragEnd={handleDragEnd}>
                <Droppable droppableId="rounds">
                    {(provided: DroppableProvided) => (
                        <div {...provided.droppableProps} ref={provided.innerRef} className="space-y-4">
                            {rounds.map((round, index) => (
                                <Draggable key={round.id} draggableId={round.id.toString()} index={index}>
                                    {(provided: DraggableProvided) => (
                                        <div ref={provided.innerRef} {...provided.draggableProps}>
                                            <Collapsible>
                                                <Card>
                                                    <CardHeader>
                                                        <CollapsibleTrigger className="w-full">
                                                            <div className="flex items-center justify-between">
                                                                <div className="flex items-center gap-3">
                                                                    <Music2 className="h-5 w-5" />
                                                                    <div>
                                                                        <CardTitle>{round.title}</CardTitle>
                                                                        <CardDescription>
                                                                            {RoundTypeDTO[round.type]} • {round.timeInMinutes} min • {round.items?.length || 0} items
                                                                        </CardDescription>
                                                                    </div>
                                                                </div>
                                                                <div {...provided.dragHandleProps}>
                                                                    <GripVertical className="h-5 w-5 text-muted-foreground cursor-move" />
                                                                </div>
                                                            </div>
                                                        </CollapsibleTrigger>
                                                    </CardHeader>
                                                    <CollapsibleContent>
                                                        <CardContent>
                                                            <RoundItemList
                                                                items={round.items || []}
                                                                onAddItem={() => handleAddItem(round.id)}
                                                                onRemoveItem={(itemId) => handleRemoveItem(round.id, itemId)}
                                                            />
                                                            {isAddingItem && expandedRoundId === round.id && (
                                                                <RoundItemForm
                                                                    roundId={round.id}
                                                                    onCancel={() => setIsAddingItem(false)}
                                                                    onSave={() => {
                                                                        setIsAddingItem(false);
                                                                        // TODO: Refresh rounds data
                                                                    }}
                                                                />
                                                            )}
                                                        </CardContent>
                                                    </CollapsibleContent>
                                                </Card>
                                            </Collapsible>
                                        </div>
                                    )}
                                </Draggable>
                            ))}
                            {provided.placeholder}
                        </div>
                    )}
                </Droppable>
            </DragDropContext>
        </div>
    );
} 