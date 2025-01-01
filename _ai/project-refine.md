Bra input! Nu gör vi en mer flexibel specifikation där spelledaren kan bygga sitt eget spel.

GAME STRUCTURE SPECIFICATION:

```
Core Concepts:
1. Game Builder (Admin/Host)
- Create rounds in any order
- Select number of items per round
- Save game templates for reuse

2. Game Runtime
- Follow the custom-built structure
- Real-time scoring
- Clear host/display separation
```

DATA MODELS:
```
GameTemplate
- Id: int
- Name: string
- Description: string
- CreatedAt: DateTime
- IsPublic: bool

Round
- Id: int
- GameTemplateId: int
- Type: enum (GuessTheMelody, FirstLine, ArtistQuiz)
- OrderIndex: int
- Title: string (custom round name)
- TimeInMinutes: int
- Instructions: string

RoundItem
- Id: int
- RoundId: int
- OrderIndex: int
- Title: string
- Artist: string
- Points: int
- ExtraInfo: string (lyrics/clues depending on type)

ActiveGame
- Id: int
- GameTemplateId: int
- Status: enum (NotStarted, InProgress, Finished)
- CurrentRoundIndex: int
- CurrentItemIndex: int
- CreatedAt: DateTime

Team
- Id: int
- ActiveGameId: int
- Name: string
- Score: int
```

ROUND TYPES:

```
1. Guess The Melody
Config options:
- Number of songs
- Points per correct guess
- Time limit per song
- Custom instructions

2. First Line
Config options:
- Number of songs
- Points per line
- Max lines per song
- Custom instructions

3. Artist Quiz
Config options:
- Number of artists
- Points per difficulty level
- Number of clues
- Custom instructions
```

USER INTERFACES:

```
1. Game Builder Interface
- Create new game template
- Add/remove/reorder rounds
- Add items to each round
- Save templates
- Clone existing templates

2. Host Game Interface (Mobile/Tablet)
- Select game template
- Register teams
- Control game flow
- Award points
- Timer controls

3. Display Interface (TV/Projector)
- Current round info
- Active content
- Team scores
- Timer when applicable
```

GAME FLOW:

```
Setup Phase:
1. Host either:
   - Selects existing template
   - Creates new game structure
2. Enters team names
3. Starts game

During Game:
1. Round Introduction
   - Shows round type
   - Displays instructions
   
2. Round Execution
   - Host controls progression
   - Teams score points
   - Flexible timing

3. Round Transition
   - Score summary
   - Next round preview
```

API ENDPOINTS:

```
Game Templates:
POST /api/templates
GET /api/templates
GET /api/templates/{id}
PUT /api/templates/{id}
DELETE /api/templates/{id}

Rounds:
POST /api/templates/{templateId}/rounds
PUT /api/templates/{templateId}/rounds/{roundId}/reorder
DELETE /api/templates/{templateId}/rounds/{roundId}

Round Items:
POST /api/rounds/{roundId}/items
PUT /api/rounds/{roundId}/items/{itemId}/reorder
DELETE /api/rounds/{roundId}/items/{itemId}

Active Games:
POST /api/games/start
PUT /api/games/{gameId}/status
PUT /api/games/{gameId}/scores
GET /api/games/{gameId}/state
```

TECHNICAL CONSIDERATIONS:

```
1. State Management
- Real-time game state updates
- Concurrent access handling
- State recovery on connection loss

2. Performance
- Minimal latency for game controls
- Efficient state updates
- Mobile-friendly interface

3. Data Integrity
- Validation of game structures
- Score tracking accuracy
- Error handling
```

IMPLEMENTATION PHASES:

```
Phase 1: Template Builder
- Basic CRUD for game templates
- Round management
- Item management
- Template saving/loading

Phase 2: Game Runtime
- Game state management
- Score tracking
- Basic display interface
- Host controls

Phase 3: Enhancement
- Timer functionality
- Improved UI/UX
- Template sharing
- Game statistics
```

This specification provides a flexible framework where:
1. Hosts can create custom game structures
2. Rounds can be arranged in any order
3. Content is added during template creation
4. Templates can be saved and reused
5. The game flow follows the custom structure

Would you like me to elaborate on any part of this specification?