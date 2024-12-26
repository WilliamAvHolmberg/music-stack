import { useState } from "react";
import { Button } from "@/shared/components/ui/button";
import { Input } from "@/shared/components/ui/input";
import { Card, CardContent, CardHeader, CardTitle } from "@/shared/components/ui/card";
import { usePostApiWebScraperScrape } from "@/shared/api/hooks/api";
import { useToast } from "@/shared/hooks/use-toast";
import type { ScrapeUrlResponseDTO } from "@/shared/api/models";
import { Copy } from "lucide-react";

export function ScrapingTestPage() {
    const [url, setUrl] = useState("");
    const { toast } = useToast();
    const { mutate: scrapeUrl, isPending } = usePostApiWebScraperScrape({
        mutation: {
            onError: () => {
                toast({
                    title: "Error",
                    description: "Failed to scrape URL",
                    variant: "destructive",
                });
            },
        },
    });

    const [result, setResult] = useState<ScrapeUrlResponseDTO | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        scrapeUrl({ data: { url } }, {
            onSuccess: (data) => {
                if (data.html && data.screenshot && data.loadTimeSeconds) {
                    setResult(data);
                }
            },
        });
    };

    const copyHtml = async () => {
        if (result?.html) {
            await navigator.clipboard.writeText(result.html);
            toast({
                title: "Copied!",
                description: "HTML copied to clipboard",
            });
        }
    };

    return (
        <div className="container mx-auto p-4 space-y-4">
            <Card>
                <CardHeader>
                    <CardTitle>Web Scraping Test</CardTitle>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit} className="space-y-4">
                        <div className="flex gap-2">
                            <Input
                                type="url"
                                value={url}
                                onChange={(e) => setUrl(e.target.value)}
                                placeholder="Enter URL to scrape"
                                required
                                className="flex-1"
                            />
                            <Button type="submit" disabled={isPending}>
                                {isPending ? "Scraping..." : "Scrape"}
                            </Button>
                        </div>
                    </form>
                </CardContent>
            </Card>

            {result?.html && result?.screenshot && result?.loadTimeSeconds && (
                <div className="space-y-4">
                    <Card>
                        <CardHeader>
                            <CardTitle>Results</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-4">
                                <div>
                                    <h3 className="font-semibold mb-2">Load Time</h3>
                                    <p>{result.loadTimeSeconds.toFixed(2)} seconds</p>
                                </div>

                                <div>
                                    <h3 className="font-semibold mb-2">Screenshot</h3>
                                    <img
                                        src={`data:image/png;base64,${result.screenshot}`}
                                        alt="Screenshot"
                                        className="max-w-full h-auto border rounded"
                                    />
                                </div>

                                <div>
                                    <div className="flex items-center justify-between mb-2">
                                        <h3 className="font-semibold">HTML</h3>
                                        <Button
                                            variant="outline"
                                            size="sm"
                                            onClick={copyHtml}
                                            className="flex items-center gap-2"
                                        >
                                            <Copy className="h-4 w-4" />
                                            Copy
                                        </Button>
                                    </div>
                                    <pre className="bg-gray-100 p-4 rounded overflow-auto max-h-96">
                                        {result.html}
                                    </pre>
                                </div>
                            </div>
                        </CardContent>
                    </Card>
                </div>
            )}
        </div>
    );
} 