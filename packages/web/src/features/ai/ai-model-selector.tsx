import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/shared/components/ui/select";
import { Brain } from 'lucide-react';
import { AI_MODELS } from './ai-types';
import { useAIModel } from "./ai-model-context";
export function AIModelSelector() {
  const { selectedModel, setSelectedModel } = useAIModel();

  return (
    <div className="flex items-center gap-2">
      <Brain className="h-4 w-4 text-muted-foreground" />
      <Select
        value={selectedModel.id}
        onValueChange={(value) => {
          const model = AI_MODELS.find(m => m.id === value);
          if (model) setSelectedModel(model);
        }}
      >
        <SelectTrigger className="w-[140px] h-8 text-xs">
          <SelectValue placeholder="Select AI Model" />
        </SelectTrigger>
        <SelectContent>
          {AI_MODELS.map((model) => (
            <SelectItem
              key={model.id}
              value={model.id}
              className="text-xs"
            >
              {model.name}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
    </div>
  );
} 