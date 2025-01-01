import { createContext, useContext, useState, ReactNode } from 'react';
import { AI_MODELS } from './ai-types';
import { AIModel } from './ai-types';

interface AIModelContextType {
  selectedModel: AIModel;
  setSelectedModel: (model: AIModel) => void;
}

const AIModelContext = createContext<AIModelContextType | undefined>(undefined);

export function AIModelProvider({ children }: { children: ReactNode }) {
  const [selectedModel, setSelectedModel] = useState<AIModel>(() => {
    // Try to get from localStorage first
    const stored = localStorage.getItem('selectedAIModel');
    if (stored) {
      const parsed = JSON.parse(stored);
      const found = AI_MODELS.find(m => m.id === parsed.id);
      if (found) return found;
    }
    return AI_MODELS[0]!;
  });

  const handleSetModel = (model: AIModel) => {
    setSelectedModel(model);
    localStorage.setItem('selectedAIModel', JSON.stringify(model));
  };

  return (
    <AIModelContext.Provider value={{ selectedModel, setSelectedModel: handleSetModel }}>
      {children}
    </AIModelContext.Provider>
  );
}

export function useAIModel() {
  const context = useContext(AIModelContext);
  if (context === undefined) {
    throw new Error('useAIModel must be used within an AIModelProvider');
  }
  return context;
} 