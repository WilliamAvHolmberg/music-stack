import { Button } from "@/shared/components/ui/button";
import { Tooltip, TooltipContent, TooltipProvider, TooltipTrigger } from "@/shared/components/ui/tooltip";
import { cn } from "@/shared/utils/utils";

interface QuickScoreButtonProps {
    value: number;
    onClick: () => void;
}

export function QuickScoreButton({ value, onClick }: QuickScoreButtonProps) {
    return (
        <TooltipProvider>
            <Tooltip>
                <TooltipTrigger asChild>
                    <Button
                        variant="outline"
                        size="sm"
                        onClick={onClick}
                        className={cn(
                            "px-2 py-1",
                            value > 0 ? "text-green-500 hover:text-green-400" : "text-red-500 hover:text-red-400"
                        )}
                    >
                        {value > 0 ? `+${value}` : value}
                    </Button>
                </TooltipTrigger>
                <TooltipContent>
                    <p>{value === 1 ? "1" : value === 2 ? "2" : value === -1 ? "-" : ""}</p>
                </TooltipContent>
            </Tooltip>
        </TooltipProvider>
    );
} 