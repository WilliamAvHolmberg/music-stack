import { GraduationCap, Library } from 'lucide-react';
import { Link, useLocation } from 'react-router-dom';
import { cn } from '@/shared/utils/utils';
import { AIModelSelector } from '../../features/ai/ai-model-selector';

export function Header() {
  const location = useLocation();

  const navItems = [
    {
      href: "/flashcards",
      label: "Flashcards",
      icon: Library,
      active: location.pathname.startsWith("/flashcards")
    }
  ];

  return (
    <header className="sticky top-0 z-50 w-full border-b bg-white/95 backdrop-blur supports-[backdrop-filter]:bg-white/60">
      <div className="container mx-auto">
        <div className="flex h-16 items-center">
          <Link to="/" className="flex items-center space-x-2">
            <GraduationCap className="h-6 w-6" />
            <span className="font-bold">Workshop Starter</span>
          </Link>
          <nav className="flex-1 flex items-center justify-center space-x-6">
            {navItems.map(({ href, label, icon: Icon, active }) => (
              <Link
                key={href}
                to={href}
                className={cn(
                  "flex items-center space-x-2 text-sm font-medium transition-colors hover:text-gray-900",
                  active ? "text-gray-900" : "text-gray-500"
                )}
              >
                <Icon className="h-4 w-4" />
                <span>{label}</span>
              </Link>
            ))}
          </nav>
          <div className="flex items-center">
            <AIModelSelector />
          </div>
        </div>
      </div>
    </header>
  );
}