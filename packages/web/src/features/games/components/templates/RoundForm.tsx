import { useState } from 'react';
import { Button } from '@/shared/components/ui/button';
import { Input } from '@/shared/components/ui/input';
import { Label } from '@/shared/components/ui/label';
import { Card, CardContent, CardHeader, CardTitle } from '@/shared/components/ui/card';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/shared/components/ui/select';
import { RoundType } from '../../types';
import { RoundItemForm } from './RoundItemForm';
import { Music, Timer } from 'lucide-react';

interface RoundItem {
    title: string;
    artist: string;
    points: number;
    extraInfo?: string;
}

interface RoundFormProps {
    onSave: (round: {
        title: string;
        type: RoundType;
        timeInMinutes: number;
        items: RoundItem[];
    }) => void;
    onCancel: () => void;
}

export function RoundForm({ onSave, onCancel }: RoundFormProps) {
    const [title, setTitle] = useState('');
    const [type, setType] = useState<RoundType>(RoundType.GuessTheMelody);
    const [timeInMinutes, setTimeInMinutes] = useState(3);
    const [items, setItems] = useState<RoundItem[]>([]);
    const [isAddingItem, setIsAddingItem] = useState(false);

    const handleSave = () => {
        onSave({
            title,
            type,
            timeInMinutes,
            items
        });
    };

    const handleAddItem = (item: RoundItem) => {
        setItems([...items, item]);
        setIsAddingItem(false);
    };

    const handleRemoveItem = (index: number) => {
        setItems(items.filter((_, i) => i !== index));
    };

    return (
        <div className="space-y-6">
            <Card>
                <CardHeader>
                    <CardTitle>New Round</CardTitle>
                </CardHeader>
                <CardContent className="space-y-4">
                    <div className="space-y-2">
                        <Label>Title</Label>
                        <Input
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            placeholder="Enter round title..."
                        />
                    </div>

                    <div className="space-y-2">
                        <Label>Type</Label>
                        <Select
                            value={type.toString()}
                            onValueChange={(value) => setType(parseInt(value))}
                        >
                            <SelectTrigger>
                                <SelectValue />
                            </SelectTrigger>
                            <SelectContent>
                                {Object.entries(RoundType)
                                    .filter(([key]) => isNaN(Number(key)))
                                    .map(([key, value]) => (
                                        <SelectItem key={value} value={value.toString()}>
                                            {key}
                                        </SelectItem>
                                    ))}
                            </SelectContent>
                        </Select>
                    </div>

                    <div className="space-y-2">
                        <Label>Time (minutes)</Label>
                        <div className="flex items-center gap-2">
                            <Timer className="h-4 w-4" />
                            <Input
                                type="number"
                                min={1}
                                value={timeInMinutes}
                                onChange={(e) => setTimeInMinutes(parseInt(e.target.value))}
                                className="w-20"
                            />
                        </div>
                    </div>
                </CardContent>
            </Card>

            <div className="space-y-4">
                <div className="flex items-center justify-between">
                    <h3 className="text-lg font-semibold">Items</h3>
                    <Button 
                        variant="outline" 
                        onClick={() => setIsAddingItem(true)}
                        disabled={isAddingItem}
                    >
                        Add Item
                    </Button>
                </div>

                {isAddingItem && (
                    <RoundItemForm
                        onSave={handleAddItem}
                        onCancel={() => setIsAddingItem(false)}
                    />
                )}

                <div className="space-y-2">
                    {items.map((item, index) => (
                        <Card key={index}>
                            <CardContent className="flex items-center justify-between py-4">
                                <div className="flex items-center gap-3">
                                    <Music className="h-4 w-4" />
                                    <div>
                                        <div className="font-medium">
                                            {item.title} - {item.artist}
                                        </div>
                                        {item.extraInfo && (
                                            <div className="text-sm text-muted-foreground">
                                                {item.extraInfo}
                                            </div>
                                        )}
                                    </div>
                                </div>
                                <div className="flex items-center gap-4">
                                    <div className="text-sm font-medium">
                                        {item.points} points
                                    </div>
                                    <Button
                                        variant="ghost"
                                        size="sm"
                                        onClick={() => handleRemoveItem(index)}
                                    >
                                        Remove
                                    </Button>
                                </div>
                            </CardContent>
                        </Card>
                    ))}
                </div>

                {items.length > 0 && (
                    <div className="flex justify-end gap-2 pt-4">
                        <Button variant="outline" onClick={onCancel}>
                            Cancel
                        </Button>
                        <Button onClick={handleSave}>
                            Save Round
                        </Button>
                    </div>
                )}
            </div>
        </div>
    );
} 