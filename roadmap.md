# Song Stack Roadmap

## Core Concepts
- Game Templates: Reusable configurations for music quiz games
- Rounds: Different types of challenges within a game (e.g., Guess The Melody, First Line, Artist Quiz)
- Items: Individual questions/songs within a round
- Teams: Groups of players competing in a game
- Game Sessions: Active instances of a game template being played

## Data Models
- GameTemplate
  - Title
  - Description
  - IsPublic
  - Rounds[]
- Round
  - Title
  - Type (Guess The Melody, First Line, Artist Quiz)
  - TimeInMinutes
  - Instructions
  - OrderIndex
  - Items[]
- RoundItem
  - Title
  - Artist
  - Points
  - OrderIndex
- Game
  - TemplateId
  - Teams[]
  - CurrentRoundIndex
  - CurrentItemIndex
  - Status
- Team
  - Name
  - Score

## Implementation Plan

### Phase 1: Template Builder ✅
1. Backend Infrastructure
   - Entity models and database configuration ✅
   - API endpoints for templates and games ✅
   - Authentication and authorization setup ✅

2. Frontend Base Setup
   - Project setup with Vite + React + TypeScript ✅
   - TailwindCSS and shadcn/ui integration ✅
   - Basic routing structure ✅

3. Game Template Management UI
   - Template list view with search and filtering ✅
   - Template creation/edit form ✅
   - Round configuration with drag-and-drop reordering ✅
   - Item management within rounds ✅
     - Drag-and-drop reordering ✅
     - Form validation ✅
     - Empty state handling ✅

### Phase 2: Game Flow UI 🚧
1. Template Selection
   - Browse available templates
   - Filter by category/type
   - Preview template details
   - Template duplication
   - Quick start options

2. Team Registration
   - Add/remove teams
   - Set team names
   - Randomize team order
   - Team color selection
   - Team avatar/emoji selection

3. Game Host Interface
   - Start/pause/end game
   - Control round progression
   - Score management
     - Point tracking
     - Score adjustments
     - Bonus points
   - Timer controls
     - Start/pause/reset
     - Time adjustments
     - Visual countdown
   - Item navigation
     - Next/previous item
     - Skip item
     - Mark item as completed

4. Public Display Interface
   - Current round/item display
     - Round title and type
     - Item details (when revealed)
     - Progress indicator
   - Team scores
     - Real-time updates
     - Score animations
     - Team rankings
   - Timer display
     - Countdown visualization
     - Time remaining alerts
   - Round instructions
     - Clear formatting
     - Visual guides
     - Transition animations

5. Real-time Updates
   - WebSocket integration
   - Score updates
   - Game state synchronization
   - Connection status handling
   - Reconnection logic

### Phase 3: Enhancements
1. Template Management
   - Template categories/tags
   - Template sharing
   - Import/export templates
   - Version history
   - Template previews

2. Game Features
   - Sound effects
     - Correct/incorrect answers
     - Timer alerts
     - Round transitions
   - Animations
     - Score changes
     - Team position changes
     - Round transitions
   - Round variations
     - Multiple choice
     - Speed rounds
     - Team battles
   - Power-ups
     - Double points
     - Time bonuses
     - Hints

3. Analytics
   - Game history
     - Detailed session logs
     - Score breakdowns
     - Time analytics
   - Team statistics
     - Win/loss records
     - Average scores
     - Favorite rounds
   - Popular templates
     - Usage metrics
     - Rating system
     - Success rates
   - Usage metrics
     - Active games
     - Player engagement
     - Feature adoption

4. Social Features
   - User profiles
     - Game history
     - Favorite templates
     - Statistics
   - Template ratings/reviews
   - Community templates
     - Featured templates
     - Most popular
     - Recently added
   - Leaderboards
     - Global rankings
     - Weekly/monthly contests
     - Achievement system

## Next Steps
1. Game Flow UI Implementation
   - Create TemplateSelector component
     - Grid/list view toggle
     - Search and filter controls
     - Template preview cards
   - Build TeamRegistration component
     - Team form with validation
     - Team list with actions
     - Order management
   - Develop GameHost interface
     - Game controls layout
     - Score management panel
     - Timer component
   - Design PublicDisplay component
     - Responsive layouts
     - Score board design
     - Timer visualization

2. Real-time Infrastructure
   - Set up SignalR hub
   - Define real-time events
   - Implement client connections
   - Add reconnection handling

3. Game State Management
   - Define game state structure
   - Implement state transitions
   - Add persistence layer
   - Handle edge cases

4. Testing and Refinement
   - Unit tests for game logic
   - Integration tests for real-time features
   - Performance testing
   - User acceptance testing 