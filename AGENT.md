# Chor Police - AI Agent Guidelines

## Project Overview

**Chor Police** is a 4-player social deduction board game built in Unity (6000.3.9f1), based on the popular Bangladeshi game of the same name. The project follows solid OOP principles, ScriptableObject architecture, and supports both AI and multiplayer gameplay via Photon Fusion 2.

### Game Concept

- **Players:** 4 (1 human + 3 AI in single-player mode)
- **Roles:** Police, Chor (Thief), Babu (Innocent), Dakat (Robber)
- **Objective:** Police must catch either the Chor or Dakat depending on the round target
- **Round Logic:** Target alternates between CHOR ⇄ DAKAT each round
- **Total Rounds:** 10 rounds per game

### Scoring System

| Role     | Condition              | Points |
|----------|------------------------|--------|
| Babu     | Survives               | 1000   |
| Police   | Catches target         | 800    |
| Chor     | Escapes target round   | 700    |
| Dakat    | Escapes target round   | 600    |
| Any      | If caught/not target   | 0      |

---

## Architecture & Patterns

### Core Architecture

```
GameManager (Central Hub)
├── RoleManager (Role Assignment)
├── RoundManager (Round Logic & Target Toggle)
├── ScoreManager (Scoring System)
└── GameStateManager (State Machine)
```

### Design Patterns Used

1. **Singleton Pattern**: All managers use singleton for global access (GameManager, RoleManager, RoundManager, GameStateManager, ScoreManager)

2. **Base Class Pattern**: `BaseManager` abstract class provides shared structure for all managers
   ```csharp
   public abstract class BaseManager : MonoBehaviour
   {
       public virtual void Init() {}
       public virtual void ResetManager() {}
   }
   ```

3. **Strategy Pattern**: `IScorable` interface for role-based scoring logic
   - `BabuScorer` - Points for surviving
   - `PoliceScorer` - Points for catching target
   - `TargetScorer` - Points for Chor/Dakat escaping

4. **State Machine**: `GameStateManager` controls game flow phases:
   - Menu → Shuffling → RoleReveal → PolicePick → RevealResult → Scorecard → GameOver

5. **ScriptableObject**: `ScoreConfig` for data-driven score configuration

### Project Structure

```
Assets/
├── Scripts/
│   ├── Core/                   # Core game logic
│   │   ├── BaseManager.cs
│   │   ├── GameManager.cs      # Central game coordinator
│   │   ├── GameState.cs        # Game state enum
│   │   ├── GameStateManager.cs # State machine controller
│   │   ├── IScorable.cs        # Scoring strategy interface
│   │   ├── PlayerData.cs       # Player data structure
│   │   ├── RoleManager.cs      # Role assignment
│   │   ├── RoleType.cs         # Role enum
│   │   ├── RoundManager.cs     # Round logic
│   │   ├── ScoreConfig.cs      # ScriptableObject for scores
│   │   ├── ScoreManager.cs     # Scoring system
│   │   └── TargetScorer.cs     # Chor/Dakat scoring
│   ├── Player/                 # Player logic
│   │   ├── AIManager.cs        # AI system controller
│   │   ├── AIPlayer.cs         # AI behavior for computer players
│   │   └── PlayerController.cs # Human player controller
│   ├── Systems/
│   │   ├── LeaderboardManager.cs
│   │   └── Scoring/
│   │       ├── BabuScorer.cs
│   │       ├── PoliceScorer.cs
│   │       └── ScoringSystem.cs
│   ├── Testing/
│   │   └── TestRoundSimulator.cs
│   ├── UI/
│   │   ├── GameOverUI.cs
│   │   ├── PolicePickUI.cs
│   │   ├── RolePanelUI.cs
│   │   ├── ScoreboardUI.cs
│   │   └── UIManager.cs
│   └── Utilities/
│       └── TMPHelper.cs
├── Docs/
│   └── UML/                    # Architecture diagrams
├── Data/                       # ScriptableObjects
├── Resources/
├── Scenes/
└── Settings/
```

---

## Development Workflow

### Phase 1: Core Logic + AI Prototype (Current)

**Completed:**
- [x] Player & Role system
- [x] Round simulation
- [x] Scoring via ScriptableObject
- [x] State machine implementation
- [x] AI system with configurable difficulty

**In Progress:**
- [ ] UI for player roles, round status, and scores
- [ ] AI behavior refinement (target selection logic)

### Phase 2: Online Multiplayer (Photon Fusion 2)

**Planned:**
- [ ] Sync player roles & scores
- [ ] Room creation & matchmaking
- [ ] Sync game state across clients
- [ ] Networked state machine synchronization

### Phase 3: Local Multiplayer (WiFi/Bluetooth)

**Planned:**
- [ ] Local peer-to-peer discovery
- [ ] Sync actions over LAN/Bluetooth

---

## Coding Standards & Conventions

### File Naming

- **C# Scripts**: PascalCase (e.g., `GameManager.cs`)
- **Variables**: camelCase (e.g., `currentRound`, `didPoliceWin`)
- **Properties**: PascalCase (e.g., `CurrentRound`, `PickedTarget`)
- **Methods**: PascalCase (e.g., `StartNewGame()`, `ProcessPolicePick()`)
- **Constants**: UpperSnakeCase (not currently used, but preferred)
- **Private Fields**: camelCase with underscore prefix recommended (e.g., `_currentState`)

### Code Organization

1. **Header Comments**: Each script includes author and purpose description
   ```csharp
   // GameManager.cs
   // Author: Sadikur Rahman
   // Description: Manages the overall game state, player list, and round progression.
   ```

2. **Region Organization**: Group related code with `#region` where appropriate

3. **XML Documentation**: Use XML doc comments for public methods and classes

### Manager Pattern

All managers follow this structure:
- Singleton instance in `Awake()`
- `Init()` method for initialization
- `ResetManager()` method for cleanup
- Public methods for game logic

---

## Anti-Patterns & Gotchas

### ❌ Anti-Pattern 1: Heavy Logic in `Update()`

**What it looks like:**
- Performing `FindObjectOfType`, `GetComponent`, or heavy calculations every frame

**Why it fails:**
- Kills CPU performance
- Drains battery on mobile

**Correct approach:**
- Cache references in `Awake()` or `Start()`
- Use `Invoke`, Coroutines, or state-driven updates for non-frame-critical logic
- Use event-driven architecture (e.g., `OnStateChanged` event in `GameStateManager`)

### ❌ Anti-Pattern 2: Direct Manager Dependencies

**What it looks like:**
- Components directly calling multiple managers in `Update()`

**Why it fails:**
- Creates tight coupling
- Hard to test and maintain

**Correct approach:**
- Use event-based communication (`OnStateChanged`, `OnRevealResult`)
- UI components subscribe to events rather than polling state
- Let `GameManager` coordinate manager interactions

### ❌ Anti-Pattern 3: God Classes

**What it looks like:**
- `GameManager.cs` exceeding 500 lines with multiple responsibilities

**Why it fails:**
- Difficult to maintain and debug
- Violates Single Responsibility Principle

**Correct approach:**
- Use composition: Split responsibilities into `RoleManager`, `RoundManager`, `ScoreManager`
- Each manager handles its specific domain
- `GameManager` orchestrates but doesn't implement

### ❌ Anti-Pattern 4: Hard-Referenced Assets in Code

**What it looks like:**
- Hardcoded paths, direct references to GameObjects scattered across code

**Why it fails:**
- Scene dependencies make testing difficult
- Asset refactoring becomes error-prone

**Correct approach:**
- Use ScriptableObjects (`ScoreConfig`) for configuration
- Use `Resources.Load()` or Addressables for dynamic loading
- Keep scene references minimal and centralized

### ❌ Anti-Pattern 5: Ignoring AI Difficulty Scaling

**What it looks like:**
- Fixed AI pick probability across all difficulty levels

**Why it fails:**
- Game becomes too easy or too hard
- Poor player experience

**Correct approach:**
- Use `correctPickChance` parameter in `AIPlayer.cs`
- Scale difficulty based on game progress or user settings
- Consider different AI personalities (aggressive, cautious, random)

---

## Quality Checklist

### Performance

- [ ] **Frame Rate**: Stable 60fps on target hardware
- [ ] **GC Alloc**: 0 bytes allocated per frame in main gameplay loop
- [ ] **UI Updates**: Use event-driven updates instead of polling
- [ ] **AI Throttling**: AI decisions use delays, not per-frame calculations

### Code Architecture

- [ ] **Decoupled**: Systems communicate via Events/Interfaces, not hard dependencies
- [ ] **Clean**: No "God Classes" > 500 lines
- [ ] **Singleton Safety**: Proper null checks and destroy on duplicate
- [ ] **State Machine**: All state transitions go through `GameStateManager`

### Multiplayer Readiness (for Phase 2)

- [ ] **Network-Safe**: No client-authoritative game logic
- [ ] **State Sync**: All state changes are replicable
- [ ] **Predicted Movement**: Client-side prediction for smooth gameplay
- [ ] **Lag Compensation**: Interpolation/extrapolation for network smoothing

### UI/UX

- [ ] **State Feedback**: Clear visual feedback for each game state
- [ ] **Role Clarity**: Players clearly see their roles
- [ ] **Score Visibility**: Round and total scores visible
- [ ] **Accessibility**: Color-blind friendly role indicators

### Testing

- [ ] **Unit Tests**: Core logic (scoring, role assignment)
- [ ] **Integration Tests**: State machine transitions
- [ ] **Play Tests**: AI difficulty balance
- [ ] **Network Tests**: (Phase 2) Multiplayer synchronization

---

## Tech Stack

- **Engine**: Unity 6000.3.9f1
- **Language**: C# (.NET Standard 2.1)
- **Rendering**: URP (Universal Render Pipeline)
- **Input**: Unity Input System
- **UI**: Unity UI with TextMeshPro
- **Networking**: Photon Fusion 2 (Planned for Phase 2)
- **Version Control**: Git + GitHub

---

## Specific Implementation Guidelines

### AI System

The AI system uses a probability-based decision model:

```csharp
// Configurable AI parameters
[Range(0f, 1f)]
public float correctPickChance = 0.35f; // 35% chance to pick correctly
```

**AI Behavior Flow:**
1. When AI is Police: MakePolicePick() called
2. Think time: Random delay between minThinkTime and maxThinkTime
3. Decision: Roll against correctPickChance
4. Pick: Either correct target or random suspect
5. Submit: Call GameStateManager.OnPolicePickComplete()

### State Machine Flow

```
Menu
  ↓ (StartGame)
Shuffling (2 seconds)
  ↓ (OnShuffleComplete)
RoleReveal
  ↓ (OnRoleRevealComplete)
PolicePick
  ↓ (OnPolicePickComplete)
RevealResult (2 seconds)
  ↓ (OnResultRevealComplete)
Scorecard
  ↓ (OnScorecardComplete or if last round)
GameOver
```

### Scoring Calculation

Each role uses a different `IScorable` implementation:

```csharp
// Role-based scorer selection
switch (player.Role)
{
    case RoleType.BABU:
        scorer = new BabuScorer(scoreConfig);
        break;
    case RoleType.POLICE:
        scorer = new PoliceScorer(scoreConfig);
        break;
    case RoleType.CHOR:
    case RoleType.DAKAT:
        scorer = new TargetScorer(scoreConfig);
        break;
}
```

---

## Development Notes

### Current Limitations

1. **No Visual Feedback**: Role shuffling and result reveals need animations
2. **Single Player Only**: Multiplayer not yet implemented
3. **Static AI**: AI has no learning or adaptive behavior
4. **No Audio**: Sound effects and music not yet added

### Known Issues

1. State transitions need better validation and rollback support
2. AI difficulty needs more granular control
3. UI needs to handle window resizing gracefully
4. No save/load system for game state

### Future Enhancements

1. **AI Personalities**: Different AI behavior profiles
2. **Power-ups**: Special abilities that affect gameplay
3. **Customization**: Player avatars and role cards
4. **Replay System**: Record and playback games
5. **Achievements**: Unlockables based on performance

---

## Testing Guidelines

### Unit Testing

Test core logic in isolation:
- Role assignment fairness (statistical distribution)
- Scoring calculations for all scenarios
- State transition validity

### Integration Testing

Test system interactions:
- Full game flow from Menu to GameOver
- AI decision making in various game states
- Score calculation and leaderboard ordering

### Play Testing

Focus on:
- Game balance and AI difficulty
- UI clarity and responsiveness
- Fun factor and engagement

---

## Version Control

### Git Workflow

- Use feature branches for new functionality
- Commit frequently with descriptive messages
- Tag releases (v1.0, v1.1, etc.)
- Use `.gitignore` for Unity-generated files

### Large Binary Assets

Use Git LFS for:
- Textures and sprites
- Audio files
- 3D models
- Prefabs with embedded assets

---

## References

- **Project README**: `README.md`
- **UML Diagrams**: `Assets/Docs/UML/`
- **Photon Fusion 2 Docs**: https://doc.photonengine.com/en-us/fusion/current
- **Unity Best Practices**: https://docs.unity3d.com/Manual/BestPracticeUnderstandingPerformanceInUnity.html

---

## Contact & Support

**Author**: Sadikur Rahman  
**Role**: Game Developer | CTO  
**LinkedIn**: https://www.linkedin.com/in/sadikur-rahman-385232159

For questions about the architecture or implementation, refer to the code comments and UML diagrams in `Assets/Docs/UML/`.

---

> Built with ❤️ to revive our childhood games using modern multiplayer tech.
