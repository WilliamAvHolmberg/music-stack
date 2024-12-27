import { Button } from "@/shared/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/shared/components/ui/card";
import { Input } from "@/shared/components/ui/input";
import { useToast } from "@/shared/hooks/use-toast";
import { usePostApiAccessibilityAnalyze } from "@/shared/api/hooks/api";
import { Progress } from "@/shared/components/ui/progress";
import { useState } from "react";
import { ChevronDown, ChevronRight } from "lucide-react";

function ExampleItem({ example, currentValue }: { example: string; currentValue?: string }) {
    const [isShowingMore, setIsShowingMore] = useState(false);
    const MAX_CHARS = 100;
    const shouldTruncate = example.length > MAX_CHARS;
    const displayText = shouldTruncate && !isShowingMore 
        ? example.slice(0, MAX_CHARS) + "..." 
        : example;

    return (
        <div className="p-4 text-sm bg-muted rounded-md space-y-2">
            <code className="whitespace-pre-wrap break-all">{displayText}</code>
            {shouldTruncate && (
                <Button 
                    variant="ghost" 
                    size="sm" 
                    onClick={() => setIsShowingMore(!isShowingMore)}
                    className="text-xs"
                >
                    {isShowingMore ? "Show less" : "Show more"}
                </Button>
            )}
            {currentValue && (
                <p className="text-xs text-muted-foreground">
                    Current value: {currentValue}
                </p>
            )}
        </div>
    );
}

function CollapsibleExamples({ examples, currentValues }: { examples?: string[] | null, currentValues?: string[] | null }) {
    const [isExpanded, setIsExpanded] = useState(false);

    if (!examples?.length) return null;

    return (
        <div className="space-y-2">
            <div className="flex items-center gap-2">
                <Button 
                    variant="ghost" 
                    size="sm" 
                    className="p-0 h-auto" 
                    onClick={() => setIsExpanded(!isExpanded)}
                >
                    {isExpanded ? <ChevronDown className="h-4 w-4" /> : <ChevronRight className="h-4 w-4" />}
                </Button>
                <p className="text-sm font-medium">Examples ({examples.length})</p>
            </div>
            
            {isExpanded && (
                <div className="space-y-2 pl-6">
                    {examples.map((example, k) => (
                        <ExampleItem 
                            key={k} 
                            example={example} 
                            currentValue={currentValues?.[k]} 
                        />
                    ))}
                </div>
            )}
        </div>
    );
}

export function AccessibilityAnalyzerPage() {
    const [url, setUrl] = useState("");
    const { toast } = useToast();

    const { mutate: analyzeUrl, isPending, data: result } = usePostApiAccessibilityAnalyze({
        mutation: {
            onError: () => {
                toast({
                    title: "Error",
                    description: "Failed to analyze URL",
                    variant: "destructive",
                });
            },
        },
    });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!url) return;

        analyzeUrl({ data: { url } });
    };

    return (
        <div className="container py-8 space-y-8">
            <div className="space-y-2">
                <h1 className="text-3xl font-bold">Accessibility Analyzer</h1>
                <p className="text-muted-foreground">
                    Enter a URL to analyze its accessibility compliance
                </p>
            </div>

            <form onSubmit={handleSubmit} className="flex gap-4">
                <Input
                    type="url"
                    placeholder="https://example.com"
                    value={url}
                    onChange={(e) => setUrl(e.target.value)}
                    className="flex-1"
                    required
                />
                <Button type="submit" disabled={isPending}>
                    {isPending ? "Analyzing..." : "Analyze"}
                </Button>
            </form>

            {result && (
                <div className="space-y-8">
                    <Card>
                        <CardHeader>
                            <CardTitle>Accessibility Score</CardTitle>
                            <CardDescription>
                                Based on WCAG guidelines and best practices
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <Progress value={result.score ?? 0} className="h-4" />
                            <p className="text-2xl font-bold">{(result.score ?? 0).toFixed(1)}%</p>
                        </CardContent>
                    </Card>

                    <div className="space-y-8">
                        {result.sections?.map((section, i) => (
                            <Card key={i}>
                                <CardHeader>
                                    <div className="flex items-center justify-between">
                                        <div>
                                            <CardTitle>{section.section} - {section.title}</CardTitle>
                                            <CardDescription>{section.description}</CardDescription>
                                        </div>
                                        <div className="flex items-center gap-2">
                                            <span className="px-2 py-1 text-xs rounded-full bg-destructive/10 text-destructive">
                                                {section.impact}
                                            </span>
                                            <span className="text-sm text-muted-foreground">
                                                {section.totalIssues} issues
                                            </span>
                                        </div>
                                    </div>
                                </CardHeader>
                                <CardContent>
                                    <div className="space-y-6">
                                        {section.issueGroups?.map((group, j) => (
                                            <div key={j} className="space-y-4">
                                                <div className="flex items-center justify-between">
                                                    <h3 className="font-semibold">{group.ruleType}</h3>
                                                    <span className="text-sm text-muted-foreground">
                                                        {group.count} occurrences
                                                    </span>
                                                </div>
                                                <CollapsibleExamples 
                                                    examples={group.examples} 
                                                    currentValues={group.currentValues} 
                                                />
                                                <p className="text-sm font-medium text-primary">
                                                    Suggestion: {group.suggestion}
                                                </p>
                                            </div>
                                        ))}
                                    </div>
                                </CardContent>
                            </Card>
                        ))}
                    </div>
                </div>
            )}
        </div>
    );
} 