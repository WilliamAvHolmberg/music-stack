# 🎵 Song Stack - An AI-Built Music Quiz Game

> Built with AI pair programming (Cursor + Claude) - a fun experiment in AI-driven development

## 🤖 About This Project

Song Stack is a modern take on music quiz games like "Name That Tune" or "Så ska det låta", built entirely through AI pair programming. This project showcases what's possible when human creativity meets AI capabilities, even if it doesn't always follow traditional best practices.

### 🎮 Game Features

- **Multiple Quiz Types**
  - 🎧 Guess The Melody
  - 📝 First Line (guess song from lyrics)
  - 🎸 Artist Quiz

- **Real-time Multiplayer**
  - Host controls game flow
  - Public display for audience
  - Live score tracking
  - Team management

- **Game Host Features**
  - Create custom game templates
  - Control round progression
  - Manage scores
  - Spotify integration

### 🛠 Tech Stack

- **Frontend**: Vite + React + TypeScript
  - TailwindCSS for styling
  - shadcn/ui for components
  - Real-time updates with SignalR

- **Backend**: .NET 8
  - SQLite database
  - Entity Framework
  - Real-time SignalR hub

## 🚀 Getting Started

1. Clone the repo
\`\`\`bash
git clone https://github.com/yourusername/song-stack.git
cd song-stack
\`\`\`

2. Install dependencies
\`\`\`bash
# Frontend
cd web
npm install

# Backend
cd ../Api
dotnet restore
\`\`\`

3. Run the app
\`\`\`bash
# Frontend
cd web
npm run dev

# Backend
cd ../Api
dotnet run
\`\`\`

## 🤖 AI Development Notes

This project was built entirely through AI pair programming using:
- Cursor AI for code generation and real-time assistance
- Claude for architecture and problem-solving

Some interesting aspects of the AI-driven development:
- Architecture evolved organically based on AI suggestions
- Code patterns might not always follow conventional best practices
- Rapid prototyping and iteration through AI pair programming
- Experimental features added based on AI creativity

## 🎯 Core Game Flow

1. **Setup Phase**
   - Create/select game template
   - Add teams
   - Configure rounds

2. **Game Play**
   - Host controls progression
   - Teams guess songs/artists
   - Real-time score updates
   - Spotify integration for music

3. **Round Types**
   - Guess The Melody: Classic "Name That Tune"
   - First Line: Guess from opening lyrics
   - Artist Quiz: Identify artists from clues

## 🎨 Features & UI

- **Game Builder Interface**
  - Create custom templates
  - Add/remove/reorder rounds
  - Configure scoring rules

- **Host Interface**
  - Mobile-optimized controls
  - Score management
  - Round progression
  - Spotify integration

- **Public Display**
  - Clean, focused UI
  - Real-time updates
  - Score display
  - Round information

## 🤝 Contributing

This is an experimental project built with AI. Feel free to:
- Fork and experiment
- Submit PRs
- Suggest improvements
- Break things and learn

## ⚠️ Disclaimer

This project was built as an experiment in AI-driven development. While functional, it may not follow all best practices and could contain unexpected behaviors. Use in production at your own risk!

## 📝 License

MIT License - feel free to use this as inspiration for your own AI-driven projects! 