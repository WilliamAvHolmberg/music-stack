export const AI_MODELS: AIModel[] = [
    { id: 'o1-mini', name: 'o1-mini', provider: 'openai' },
    { id: 'chatgpt-4o-latest', name: 'chatgpt-4o-latest', provider: 'openai' },
    { id: 'gpt-4o-mini', name: 'gpt-4o-mini', provider: 'openai' },
    { id: 'claude-3-5-sonnet-latest', name: 'Claude 3.5 Sonnet', provider: 'claude' },
    { id: 'claude-3-5-haiku-latest', name: 'Claude 3.5 Haiku', provider: 'claude' },
];

export interface AIModel {
    id: string;
    name: string;
    provider: 'openai' | 'claude';
}
