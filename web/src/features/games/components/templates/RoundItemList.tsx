import { Music2, Plus, Trash2 } from "lucide-react";
import { Button } from "@/shared/components/ui/button";
import { Card, CardContent } from "@/shared/components/ui/card";
import { RoundItemResponse } from "@/shared/api/models";
import { cn } from "@/shared/utils/utils";

interface RoundItemListProps {
    items: RoundItemResponse[];
    onAddItem: () => void;
    onRemoveItem: (itemId: number) => void;
}

export function RoundItemList({ items, onAddItem, onRemoveItem }: RoundItemListProps) {
    return (
        <div className="space-y-4">
            <div className="flex items-center justify-between">
                <h3 className="text-lg font-semibold">Items</h3>
                <Button
                    variant="outline"
                    size="sm"
                    onClick={onAddItem}
                >
                    <Plus className="h-4 w-4 mr-2" />
                    Add Item
                </Button>
            </div>

            <div className="space-y-2">
                {items.map((item) => (
                    <Card key={item.id}>
                        <CardContent className="p-4">
                            <div className="flex items-center justify-between">
                                <div className="flex items-center gap-3">
                                    <Music2 className="h-4 w-4 text-muted-foreground" />
                                    <div>
                                        <div className="font-medium">
                                            {item.title}
                                        </div>
                                        <div className="text-sm text-muted-foreground">
                                            {item.artist} • {item.points} points
                                        </div>
                                        {item.extraInfo && (
                                            <div className="text-sm text-muted-foreground mt-1">
                                                {item.extraInfo}
                                            </div>
                                        )}
                                    </div>
                                </div>
                                <Button
                                    variant="ghost"
                                    size="sm"
                                    onClick={() => onRemoveItem(item.id)}
                                >
                                    <Trash2 className="h-4 w-4" />
                                </Button>
                            </div>
                        </CardContent>
                    </Card>
                ))}
            </div>
        </div>
    );
} 