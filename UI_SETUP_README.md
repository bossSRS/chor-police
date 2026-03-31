# Chor Police - UI Setup and Gameplay Implementation Guide

## Overview

This guide provides step-by-step instructions for setting up the complete Chor Police game with a fully functional UI system.

---

## Quick Start

### Option 1: Unity Editor (Recommended)

1. Open the project in Unity (6000.3.9f1)
2. Go to `Assets/Editor/QuickSetup.cs`
3. In Unity Editor, click `ChorPolice > Quick Setup` from the menu bar
4. The script will automatically:
   - Generate the complete GameScene with all UI elements
   - Assign ScoreConfig to ScoreManager
   - Set up all managers and components

### Option 2: Manual Setup

1. **Generate Scene**
   - Click `ChorPolice > Generate Complete Game Scene` from the menu bar
   - This creates the full scene with TextMeshPro UI

2. **Assign ScoreConfig**
   - Click `ChorPolice > Assign ScoreConfig to Manager` from the menu bar
   - This links the ScoreConfig asset to the ScoreManager

---

## Game Flow

The game follows this state machine flow:

```
Menu
  ↓ (Start Game)
Shuffling (2 seconds - role assignment animation)
  ↓
RoleReveal (shows all players' roles)
  ↓ (Continue)
PolicePick (Police player selects a suspect)
  ↓
RevealResult (shows who was caught)
  ↓
Scorecard (shows round results and scores)
  ↓ (Next Round or See Final Results)
GameOver (shows leaderboard and winner)
```

---

## UI Panels

### 1. Main Menu Panel
- **Title**: "🎮 CHOR POLICE"
- **Subtitle**: "A Game of Deception"
- **Start Button**: Begins the game
- **Instructions**: Shows game info

### 2. Shuffle Panel
- **Animation**: Shows card shuffling for 2 seconds
- **Timer**: Counts down from 2.0s
- **Purpose**: Creates anticipation before role reveal

### 3. Role Reveal Panel
- **Title**: "📜 ROLES REVEALED"
- **4 Role Cards**: One for each player (Player, AI 1, AI 2, AI 3)
- **Continue Button**: Advances to Police Pick phase

### 4. Police Pick Panel
- **Instruction**: Tells the Police player what to find
- **Target Text**: Shows current target (CHOR or DAKAT)
- **3 Suspect Buttons**: AI players that can be selected
- **AI Support**: Automatically handles AI Police picks

### 5. Reveal Result Panel
- **Result Text**: Shows whether Police caught the target
- **Picked Player**: Shows who was selected and their role

### 6. Scorecard Panel
- **Round Title**: Current round number
- **Result**: Shows if Police won or lost
- **Score Rows**: Displays all 4 players with:
  - Name
  - Role
  - Current Score
- **Next Round Button**: Advances to next round or final results

### 7. Game Over Panel
- **Title**: "🏆 GAME OVER 🏆"
- **Winner**: Shows the winning player
- **Leaderboard**: Rankings of all 4 players
- **Play Again Button**: Starts a new game
- **Main Menu Button**: Returns to main menu

### 8. HUD Panel
- **Round Counter**: Shows current round/total rounds
- **Target Indicator**: Shows current target during gameplay

---

## Color Scheme

- **Police**: Blue `(0.2, 0.4, 1.0)`
- **Chor**: Yellow `(1.0, 0.8, 0.0)`
- **Dakat**: Red `(0.8, 0.2, 0.2)`
- **Babu**: Green `(0.3, 0.8, 0.3)`
- **Win**: Green
- **Lose**: Red
- **Neutral**: White
- **Background**: Dark Blue-Gray `(0.08, 0.08, 0.12)`

---

## Testing

### Automated Play Test

1. Click `ChorPolice > Run Automated Play Test` from the menu bar
2. The test will:
   - Load the GameScene
   - Start the game
   - Simulate gameplay through all states
   - Verify role assignment
   - Simulate police pick
   - Check scoring
   - Exit play mode

### Manual Play Test

1. Open `Assets/Scenes/GameScene.unity`
2. Press Play in Unity Editor
3. Click "START GAME"
4. Follow the game flow
5. Test different scenarios:
   - When human is Police
   - When human is Babu
   - When human is Chor/Dakat
   - Multiple rounds

---

## Key Scripts

### Core Scripts
- `GameManager.cs` - Central game coordinator
- `GameStateManager.cs` - State machine controller
- `RoleManager.cs` - Role assignment
- `RoundManager.cs` - Round logic
- `ScoreManager.cs` - Scoring system
- `AIManager.cs` - AI player management
- `GameInitializer.cs` - Initializes all managers

### UI Scripts
- `UIManager_TMP.cs` - Main UI controller using TextMeshPro
- `MainMenuPanel.cs` - Main menu UI (if separate)
- `PolicePickUI.cs` - Police selection UI
- `RolePanelUI.cs` - Role reveal UI
- `ScoreboardUI.cs` - Score display UI
- `GameOverUI.cs` - Game over UI

### AI Scripts
- `AIPlayer.cs` - Individual AI player behavior
- `AIManager.cs` - Manages all AI players

### Editor Scripts
- `CompleteSceneGenerator.cs` - Generates the full game scene
- `QuickSetup.cs` - One-click setup
- `AssignScoreConfig.cs` - Assigns ScoreConfig
- `PlayTestRunner.cs` - Automated testing

---

## Scoring System

| Role     | Condition              | Points |
|----------|------------------------|--------|
| Babu     | Survives               | 1000   |
| Police   | Catches target         | 800    |
| Chor     | Escapes target round   | 700    |
| Dakat    | Escapes target round   | 600    |
| Any      | If caught/not target   | 0      |

### Additional Rules
- Chor/Dakat get 50% chance of partial points (400/600) if caught
- Target alternates between CHOR and DAKAT each round

---

## AI Behavior

### AI Police
- Think time: 1-2.5 seconds (random)
- Correct pick chance: 35% (configurable)
- Picks from all non-police suspects

### Non-Police AI
- Reacts when roles are revealed
- Shows appropriate emotions based on role
- Reacts to results (happy if safe, nervous if target)

---

## Troubleshooting

### Scene Won't Generate
- Ensure `Assets/Editor/CompleteSceneGenerator.cs` exists
- Check for compilation errors in console
- Try Unity menu: `Assets > Reimport All`

### ScoreConfig Not Found
- Ensure `ScoreConfig.asset` exists in `Assets/Resources/`
- Click `ChorPolice > Assign ScoreConfig to Manager`
- Manually drag ScoreConfig to ScoreManager in inspector

### UI Elements Not Showing
- Check Canvas and Panel references in UIManager_TMP
- Ensure TextMeshPro is imported
- Verify all UI objects are children of Canvas

### Game Won't Start
- Verify all managers are in scene
- Check ScoreConfig is assigned to ScoreManager
- Ensure GameInitializer is in scene
- Look at Console for error messages

### AI Not Picking
- Check AIManager is in scene
- Verify AI players have AIPlayer components
- Ensure role assignment happened before PolicePick state

---

## Batch Scripts

### generate_scene.bat
Generates the complete game scene using Unity batch mode.

Usage:
```batch
generate_scene.bat
```

### test_game.bat
Runs the automated play test using Unity batch mode.

Usage:
```batch
test_game.bat
```

---

## File Structure

```
Assets/
├── Scripts/
│   ├── Core/              # Core game logic
│   ├── Player/            # Player and AI scripts
│   ├── Systems/           # Game systems
│   ├── UI/                # UI scripts
│   └── Utilities/         # Utility scripts
├── Editor/                # Editor-only scripts
│   ├── CompleteSceneGenerator.cs
│   ├── QuickSetup.cs
│   ├── AssignScoreConfig.cs
│   └── PlayTestRunner.cs
├── Resources/
│   └── ScoreConfig.asset  # Score configuration
├── Scenes/
│   └── GameScene.unity    # Main game scene
└── Data/                  # Data files
```

---

## Customization

### Change AI Difficulty
Edit `AIManager.cs`:
```csharp
[Range(0f, 1f)]
public float globalCorrectPickChance = 0.35f; // 35% = Easy, 70% = Hard
```

### Change Score Values
Edit `Assets/Resources/ScoreConfig.asset` in Inspector or create a new one.

### Change Colors
Edit `UIManager_TMP.cs` color fields in Inspector.

### Modify Round Duration
Edit `GameStateManager.cs` shuffle invoke time:
```csharp
Invoke(nameof(OnShuffleComplete), 2f); // Change 2f to desired seconds
```

---

## Future Enhancements

1. **Animations**: Add flip animations for role cards
2. **Sound Effects**: Add audio for state transitions
3. **Particle Effects**: Add visual feedback
4. **Settings Menu**: Allow customization of AI difficulty
5. **Player Profiles**: Save player stats
6. **Achievements**: Unlockables based on performance
7. **Multiplayer**: Network support via Photon Fusion 2

---

## Support

For issues or questions:
1. Check Console for error messages
2. Review this documentation
3. Check code comments in scripts
4. Verify all prerequisites are met

---

**Author**: Sadikur Rahman
**Last Updated**: April 2025
**Unity Version**: 6000.3.9f1
