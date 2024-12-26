import { useState } from "react";
import { Button } from "@/shared/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/shared/components/ui/card";
import { usePostApiW3Validate } from "@/shared/api/hooks/api";
import { useToast } from "@/shared/hooks/use-toast";
import { Textarea } from "@/shared/components/ui/textarea";
import { AlertCircle, CheckCircle2 } from "lucide-react";
import { Alert, AlertDescription, AlertTitle } from "@/shared/components/ui/alert";

export function W3ValidatorPage() {
    const [html, setHtml] = useState("");
    const { toast } = useToast();
    const { mutate: validateHtml, isPending } = usePostApiW3Validate({
        mutation: {
            onError: () => {
                toast({
                    title: "Error",
                    description: "Failed to validate HTML",
                    variant: "destructive",
                });
            },
        },
    });

    const [messages, setMessages] = useState<Array<{ type: string; message: string }>>([]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        validateHtml({ data: { html } }, {
            onSuccess: (data) => {
                if (data.messages) {
                    setMessages(data.messages.map(msg => ({
                        type: msg.type ?? "Unknown",
                        message: msg.message ?? "No message provided"
                    })));
                    if (data.messages.length === 0) {
                        toast({
                            title: "Success",
                            description: "HTML is valid!",
                        });
                    }
                }
            },
        });
    };

    return (
        <div className="container mx-auto p-4 space-y-4">
            <Card>
                <CardHeader>
                    <CardTitle>W3 HTML Validator</CardTitle>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit} className="space-y-4">
                        <Textarea
                            value={html}
                            onChange={(e) => setHtml(e.target.value)}
                            placeholder="Paste your HTML here"
                            required
                            className="min-h-[200px] font-mono"
                        />
                        <Button type="submit" disabled={isPending}>
                            {isPending ? "Validating..." : "Validate HTML"}
                        </Button>
                    </form>
                </CardContent>
            </Card>

            {messages.length > 0 && (
                <div className="space-y-2">
                    {messages.map((msg, index) => (
                        <Alert
                            key={index}
                            variant={msg.type.toLowerCase() === "error" ? "destructive" : "default"}
                        >
                            {msg.type.toLowerCase() === "error" ? (
                                <AlertCircle className="h-4 w-4" />
                            ) : (
                                <CheckCircle2 className="h-4 w-4" />
                            )}
                            <AlertTitle className="capitalize">{msg.type}</AlertTitle>
                            <AlertDescription>{msg.message}</AlertDescription>
                        </Alert>
                    ))}
                </div>
            )}
        </div>
    );
} 