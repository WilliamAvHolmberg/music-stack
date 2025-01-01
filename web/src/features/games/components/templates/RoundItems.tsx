import { Plus, GripVertical } from "lucide-react";
import { Button } from "@/shared/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/shared/components/ui/card";
import { toast } from "sonner";
import type { RoundItemResponseDTO, CreateRoundItemRequestDTO } from "@/shared/api/models";
import { ItemForm } from "./ItemForm";
import { useState } from "react";
import { DragDropContext, Droppable, Draggable, DropResult, DroppableProvided, DraggableProvided } from "@hello-pangea/dnd";

interface RoundItemsProps {
  roundId: number;
  items: RoundItemResponseDTO[];
}

interface ItemFormValues {
  title: string;
  artist: string;
  points: number;
}

export function RoundItems({ roundId, items }: RoundItemsProps) {
  const [isCreating, setIsCreating] = useState(false);

  async function handleCreateItem(data: ItemFormValues) {
    try {
      const request: CreateRoundItemRequestDTO = {
        title: data.title,
        artist: data.artist,
        points: data.points,
        orderIndex: items.length,
      };

  
      toast.success("Item created successfully");
      setIsCreating(false);
    } catch (error) {
      toast.error("Failed to create item");
    }
  }

  const handleDragEnd = async (result: DropResult) => {
    if (!result.destination) return;

    const sourceIndex = result.source.index;
    const destinationIndex = result.destination.index;

    if (sourceIndex === destinationIndex) return;

    try {
      const itemId = items[sourceIndex].id;
      toast.success("Item reordered successfully");
    } catch (error) {
      toast.error("Failed to reorder item");
    }
  };

  return (
    <div className="space-y-4">
      <div className="flex justify-between items-center">
        <div>
          <h3 className="text-lg font-semibold">Items</h3>
          <p className="text-sm text-muted-foreground">Manage the items in this round</p>
        </div>
        {!isCreating && (
          <Button onClick={() => setIsCreating(true)} variant="outline" size="sm">
            <Plus className="mr-2 h-4 w-4" />
            Add Item
          </Button>
        )}
      </div>

      {isCreating && (
        <Card>
          <CardHeader>
            <CardTitle>New Item</CardTitle>
            <CardDescription>Configure your new item</CardDescription>
          </CardHeader>
          <CardContent>
            <ItemForm
              onSubmit={handleCreateItem}
              onCancel={() => setIsCreating(false)}
            />
          </CardContent>
        </Card>
      )}

      <DragDropContext onDragEnd={handleDragEnd}>
        <Droppable droppableId="items">
          {(provided: DroppableProvided) => (
            <div
              {...provided.droppableProps}
              ref={provided.innerRef}
              className="space-y-2"
            >
              {items.map((item, index) => (
                <Draggable key={item.id} draggableId={item.id.toString()} index={index}>
                  {(provided: DraggableProvided) => (
                    <div
                      ref={provided.innerRef}
                      {...provided.draggableProps}
                    >
                      <Card>
                        <CardHeader className="py-3">
                          <div className="flex items-center justify-between">
                            <div className="flex-1">
                              <div className="flex items-center justify-between">
                                <div>
                                  <CardTitle className="text-base">{item.title}</CardTitle>
                                  <CardDescription>{item.artist}</CardDescription>
                                </div>
                                <div className="flex items-center gap-4">
                                  <span className="text-sm text-muted-foreground">{item.points} points</span>
                                  <div {...provided.dragHandleProps}>
                                    <GripVertical className="h-5 w-5 text-muted-foreground cursor-move" />
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </CardHeader>
                      </Card>
                    </div>
                  )}
                </Draggable>
              ))}
              {provided.placeholder}
            </div>
          )}
        </Droppable>
      </DragDropContext>

      {items.length === 0 && !isCreating && (
        <Card className="p-6 text-center">
          <CardContent>
            <p className="text-muted-foreground">
              No items yet. Add your first item to get started!
            </p>
            <Button onClick={() => setIsCreating(true)} variant="outline" className="mt-4">
              Add Item
            </Button>
          </CardContent>
        </Card>
      )}
    </div>
  );
} 