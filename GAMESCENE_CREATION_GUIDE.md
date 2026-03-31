# 🎮 GameScene Creation Guide

## ✅ Create Your Complete GameScene

### Quick Start (3 Steps)

```
1. Open Unity (6000.3.9f1 or later)
2. Open project: D:/PROJECTS/UNITY/chor-police
3. Menu: ChorPolice > Create GameScene
4. Press Play → Click "START GAME"
5. Enjoy Chor Police!
```

---

## 🎨 What You'll Get

### Complete GameScene.unity

When you run `Create GameScene`, you'll get a fully functional game scene with:

### 🎯 Core System
- ✅ Main Camera (proper background color)
- ✅ Canvas with CanvasScaler (1920x1080)
- ✅ EventSystem with StandaloneInputModule
- ✅ Directional Light

### 🎮 All Managers (Setup)
- ✅ GameManager (game coordinator)
- ✅ RoleManager (role assignment)
- ✅ RoundManager (round logic)
- ✅ ScoreManager (scoring system)
- ✅ GameStateManager (state machine - 7 states)
- ✅ AIManager (AI player management)
- ✅ UIManager_TMP (TextMeshPro UI controller)
- ✅ GameInitializer (initializes all managers)

### 🖼 Complete UI System (8 Panels)

1. **Main Menu Panel**
   - Beautiful title: "🎮 CHOR POLICE"
   - Subtitle: "A Game of Deception"
   - Start button with color
   - Instructions text

2. **Shuffle Panel**
   - Title: "🎲 SHUFFLING ROLES..."
   - Timer display (2-second countdown)
   - 4 card placeholder cards

3. **Role Reveal Panel**
   - Title: "📜 ROLES REVEALED"
   - 4 role cards with player names
   - Role-based colors:
     * Police: Blue (0.2, 0.4, 1.0)
     * Chor: Yellow (1.0, 0.8, 0.0)
     * Dakat: Red (0.8, 0.2, 0.2)
     * Babu: Green (0.3, 0.8, 0.3)
   - Continue button

4. **Police Pick Panel**
   - Instruction: "You are the POLICE!"
   - Target text: "Find the CHOR!" or "Find the DAKAT!"
   - 3 suspect buttons (AI 1, AI 2, AI 3)
   - Dynamic target color

5. **Reveal Result Panel**
   - Result title (win/loss with emoji)
   - Picked player display
   - Color-coded result (green=win, red=loss)

6. **Scorecard Panel**
   - Round title ("Round X Results")
   - Result display
   - 4 player rows with:
     * Name
     * Role (with role color)
     * Score (yellow)
   - Next Round button

7. **Game Over Panel**
   - Title: "🏆 GAME OVER 🏆"
   - Winner announcement
   - 4-place leaderboard:
     * 1st Place: Yellow (1.0, 1.0, 0.0)
     * 2nd Place: Silver (0.75, 0.75, 0.75)
     * 3rd Place: Bronze (0.8, 0.5, 0.2)
     * 4th Place: White (1.0, 1.0, 1.0)
   - Play Again button
   - Main Menu button

8. **HUD Panel**
   - Round counter ("Round: X/10")
   - Target indicator ("Find the CHOR!" or "Find the DAKAT!")
   - Transparent background

### 🤖 Single Player Gameplay

- ✅ 1 Human Player
- ✅ 3 AI Players
- ✅ 10 Rounds per game
- ✅ Alternating targets (CHOR ⇄ DAKAT)
- ✅ Proper role distribution (Fisher-Yates shuffle)

### 📊 Complete Scoring System

| Role     | Points              | Condition |
|----------|--------------------|------------|
| Babu     | 1000               | Always |
| Police   | 800                 | If catches target |
| Chor     | 700                 | If escapes target round |
| Dakat    | 600                 | If escapes target round |

**Partial Points**: If caught, 50% chance of partial points (400/600)

---

## 📝 What the Script Does

### CreateGameScene.cs

Generates the complete scene via Unity Editor API:

1. **Creates New Scene**
   - Clears default scene
   - Sets up scene with proper settings

2. **Sets Up All Managers**
   - GameManager, RoleManager, RoundManager, ScoreManager
   - GameStateManager, AIManager, UIManager_TMP
   - GameInitializer with all references

3. **Creates Complete UI System**
   - All 8 panels with TextMeshPro
   - Proper hierarchy (all panels under Canvas)
   - All UI components properly linked

4. **Configures Game System**
   - Manager initialization order
   - Event subscriptions
   - AI player setup

5. **Saves Scene**
   - Saves to: `Assets/Scenes/GameScene.unity`
   - Automatically refreshes AssetDatabase

6. **Opens Scene**
   - Automatically becomes active scene
   - Ready to play!

---

## 🎯 UI Features

### TextMeshPro
- ✅ Beautiful text rendering
- ✅ Proper font loading
- ✅ Text alignment options
- ✅ Overflow handling

### Color System
- **Background**: Dark Blue-Gray (0.08, 0.08, 0.12, 0.95)
- **Police**: Blue (0.2, 0.4, 1.0)
- **Chor**: Yellow (1.0, 0.8, 0.0)
- **Dakat**: Red (0.8, 0.2, 0.2)
- **Babu**: Green (0.3, 0.8, 0.3)
- **Buttons**: Normal/Hover/Pressed colors

### Layout
- CanvasScaler for 1920x1080 resolution
- All panels properly anchored
- Text centered where appropriate
- Buttons with proper size and positioning

---

## 🎮 Game Flow

### Complete Gameplay Loop

```
1. Main Menu
   ↓ (click START)
2. Shuffle (2 seconds)
   ├─ Role assignment
   ├─ Animation countdown
   └─ Cards shuffle
3. Role Reveal
   ├─ Show all 4 player roles
   ├─ Police sees all, others see own
   └─ (click CONTINUE)
4. Police Pick
   ├─ "Find the CHOR!" or "Find the DAKAT!"
   ├─ 3 suspect buttons displayed
   ├─ (if human Police) Wait for selection
   ├─ (if AI Police) Auto-pick after 1-2.5s
   └─ (click suspect)
5. Reveal Result
   ├─ Did Police catch target?
   ├─ "🎉 Police Caught CHOR!" or "❌ Police Missed!"
   ├─ Show who was picked
   └─ (2-second delay)
6. Scorecard
   ├─ Round results
   ├─ All 4 players' scores
   ├─ Role colors shown
   └─ (click NEXT ROUND)
7. (Repeat x10)
8. Game Over
   ├─ Final leaderboard (1st to 4th)
   ├─ Winner announcement
   ├─ Trophy for winner
   └─ (click PLAY AGAIN / MAIN MENU)
```

---

## 🚀 How to Use

### Method 1: Quick Setup (Recommended)

```
1. Open Unity (6000.3.9f1 or later)
2. Menu: ChorPolice > Create GameScene
3. Wait 5-10 seconds for scene generation
4. Click "OK" on success dialog
5. Press Play in Unity Editor
6. Click "START GAME" on main menu
7. Enjoy Chor Police!
```

### Method 2: Generate Complete Scene

```
1. Menu: ChorPolice > Generate Complete Game Scene
2. Wait for completion
3. Scene saved to: Assets/Scenes/GameScene.unity
4. Manually open scene if needed
```

### Method 3: Manual Setup (If needed)

```
1. Create new scene in Unity Editor
2. Add all managers (GameManager, RoleManager, etc.)
3. Add Canvas with CanvasScaler
4. Run CreateGameScene script
5. Or manually build UI panels
6. Configure ScoreConfig
7. Save as GameScene.unity
```

---

## 📖 Complete Architecture

### Manager Hierarchy

```
Managers (GameObject)
├── GameManager
│   ├── RoleManager
│   ├── RoundManager
│   ├── ScoreManager
│   ├── GameStateManager
│   ├── AIManager
│   └── UIManager_TMP
└── GameInitializer
```

### UI Hierarchy

```
Canvas
├── MainMenuPanel
│   ├── Title (🎮 CHOR POLICE)
│   ├── Subtitle (A Game of Deception)
│   ├── Start Button
│   └── Instructions
├── ShufflePanel
│   ├── Title (🎲 SHUFFLING ROLES...)
│   ├── Timer Text
│   └── 4 Card Placeholders
├── RoleRevealPanel
│   ├── Title (📜 ROLES REVEALED)
│   ├── PlayerRoleCard (Blue)
│   ├── AI1RoleCard (Yellow)
│   ├── AI2RoleCard (Green)
│   └── AI3RoleCard (Red)
├── PolicePickPanel
│   ├── Instruction Text
│   ├── Target Text
│   ├── Suspect1Button
│   ├── Suspect2Button
│   └── Suspect3Button
├── RevealResultPanel
│   ├── Result Text
│   └── Picked Player Text
├── ScorecardPanel
│   ├── Round Title
│   ├── Result Text
│   ├── Player1Row
│   ├── Player2Row
│   ├── Player3Row
│   ├── Player4Row
│   └── Next Round Button
├── GameOverPanel
│   ├── Title (🏆 GAME OVER 🏆)
│   ├── Winner Text
│   ├── 1st Place
│   ├── 2nd Place
│   ├── 3rd Place
│   ├── 4th Place
│   ├── Play Again Button
│   └── Main Menu Button
└── HUDPanel
    ├── Round Counter
    └── Target Indicator
```

---

## 🎨 Visual Quality

### Colors Used
- **Main Menu**: Dark background with colorful title
- **Role Cards**: Role-based distinctive colors
- **Buttons**: 3-state color system (Normal, Hover, Pressed)
- **Panels**: Dark semi-transparent backgrounds
- **Text**: White text with proper contrast
- **Score Colors**: Yellow for points, color-coded roles

### Layout
- **Screen Resolution**: Optimized for 1920x1080
- **Canvas Scale**: Maintains aspect ratio
- **Anchoring**: Proper UI element positioning
- **Hierarchy**: Clean and organized

---

## 🎯 Key Features

### ✅ Core System
- Complete state machine (7 states)
- Proper role assignment (Fisher-Yates shuffle)
- Round management with target toggling
- Scoring system with partial points
- Leaderboard sorting and display

### ✅ AI System
- 3 AI players with configurable difficulty
- Think time delays (1-2.5 seconds)
- Role-based reactions
- AI Police support (automatic picks)

### ✅ UI System
- Full TextMeshPro integration
- All 8 panels properly linked
- Beautiful color scheme
- Responsive layout
- Event-driven updates

### ✅ Editor Tools
- One-click scene generation
- Automated setup
- Component verification
- Play test runner

---

## 📊 Game Rules

### Objective
- Police must catch the target (CHOR or DAKAT)
- Other players must avoid being caught

### Scoring
- **Babu**: Always gets 1000 points
- **Police**: Gets 800 points if catches target
- **Chor**: Gets 700 points if escapes target round
  - 50% chance of 400 partial points if caught
- **Dakat**: Gets 600 points if escapes target round
  - 50% chance of 600 partial points if caught

### How to Win
- Play all 10 rounds
- Accumulate highest total score
- Player with most points wins!

---

## 🔧 Customization

### Change AI Difficulty

```
1. Select "Managers" GameObject in scene
2. Select "AIManager" component
3. In Inspector, adjust "Global Correct Pick Chance"
4. 0.1 = Very Easy (10% correct)
5. 0.3 = Easy (25% correct)
6. 0.5 = Medium (50% correct)
7. 0.7 = Hard (75% correct)
8. 0.9 = Very Hard (90% correct)
```

### Change Scoring

```
1. Open "Assets/Resources/ScoreConfig.asset"
2. Adjust values in Inspector:
   - Babu Score (default: 1000)
   - Police Win Score (default: 800)
   - Chor Survive Score (default: 700)
   - Chor Caught Score (default: 400)
   - Dakat Survive Score (default: 600)
   - Dakat Caught Score (default: 600)
3. Save (Ctrl+S)
```

### Change Round Count

```
1. Select "Managers" GameObject in scene
2. Select "GameManager" component
3. In Inspector, adjust "Total Rounds" (default: 10)
4. Can set 5-20 rounds per game
```

---

## 📝 Troubleshooting

### Scene Won't Generate

```
1. Check Console for errors
2. Verify CreateGameScene.cs exists
3. Try: Assets > Reimport All
4. Restart Unity
```

### UI Not Showing

```
1. Verify Canvas exists in scene
2. Verify Canvas has CanvasScaler component
3. Verify all panels are children of Canvas
4. Check UIManager_TMP references are assigned
```

### Game Won't Start

```
1. Verify GameManager.Instance is not null
2. Check all managers are initialized
3. Look at Console for initialization errors
4. Verify ScoreConfig is loaded
```

### AI Not Working

```
1. Verify AIManager.Instance is not null
2. Check InitializeAIPlayers() is called
3. Check AI players have AIPlayer components
4. Check "Global Correct Pick Chance" > 0
```

---

## 📚 Documentation

| File | Purpose | Read When |
|-------|----------|------------|
| **GAMESCENE_CREATION_GUIDE.md** | ⭐ **START HERE** | This file |
| **COMPREHENSIVE_FIX_GUIDE.md** | Quick start | Playing |
| **AGENT.md** | Architecture | Development |
| **README.md** | Project overview | Information |

---

## 🎉 Summary

### What You Get

✅ **Complete GameScene.unity** with all gameplay UI
✅ **All 8 UI panels** properly set up
✅ **Full manager system** with all references
✅ **Complete game flow** from Menu to Game Over
✅ **Single player gameplay** vs 3 AI opponents
✅ **10 rounds** of social deduction
✅ **Scoring system** with partial points
✅ **Leaderboard** with winner announcement

### How to Play

```
1. Open Unity (6000.3.9f1 or later)
2. Menu: ChorPolice > Create GameScene
3. Press Play in Unity Editor
4. Click "START GAME"
5. Enjoy Chor Police!
```

---

## 🚀 Final Status

```
Scene:               ✅ Complete
UI System:           ✅ Complete (8 panels)
Managers:            ✅ All set up
AI System:            ✅ Complete (3 AI)
Game Flow:            ✅ Complete (7 states)
Scoring:             ✅ Complete (role-based)
Ready to Play:        ✅ YES!
```

---

## 📞 Quick Reference

### Unity Menu Commands

```
ChorPolice > Create GameScene           ⭐ Generate complete scene
ChorPolice > Assign ScoreConfig to Manager  ← Link ScoreConfig
ChorPolice > Verify Setup                  ← Check all components
ChorPolice > Run Automated Play Test        ← Test game flow
ChorPolice > List All Components           ← Debug all components
```

### File Locations

```
Assets/Editor/CreateGameScene.cs        ← Scene generator script
Assets/Scenes/GameScene.unity          ← Generated scene (after running)
Assets/Resources/ScoreConfig.asset           ← Score configuration
Assets/Scripts/Core/                    ← Core managers
Assets/Scripts/UI/                        ← UI system
```

---

## 🎯 Success Criteria

### Before Generation
- [ ] ❌ No scene
- [ ] ❌ No UI panels
- [ ] ❌ Managers not set up
- [ ] ❌ Can't play game

### After Generation
- [x] ✅ Complete GameScene.unity
- [x] ✅ All 8 UI panels created
- [x] ✅ All managers initialized
- [x] ✅ Scene links UI to managers
- [x] ✅ Ready to play immediately
- [x] ✅ Full game flow functional
- [x] ✅ AI system working
- [x] ✅ Scoring system working

---

## 🎮 Game Controls

### How to Play

```
Menu → Shuffle (2s) → RoleReveal → PolicePick
  → RevealResult → Scorecard → (x10) → GameOver
```

### Controls
- **Mouse Click**: Select suspects, continue, start game
- **No Keyboard**: Mouse-only game
- **Touch Support**: Works with Unity Touch

---

## 🎨 Enjoy!

**Generate your GameScene.unity and start playing Chor Police!**

The complete scene includes everything you need for a fully functional game.

---

**Author**: Sadikur Rahman
**Unity Version**: 6000.3.9f1
**Status**: ✅ **READY TO GENERATE**
**Scene**: GameScene.unity
**Players**: 1 Human + 3 AI
**Rounds**: 10 (configurable)
**Target**: Alternating CHOR/DAKAT

---

**Happy Gaming! 🎉**

```bash
# Generate your scene now!
# Open Unity and run: ChorPolice > Create GameScene
# Then press Play and enjoy!
```
