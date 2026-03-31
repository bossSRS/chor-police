# 🎮 Chor Police - Start Here

> ✅ **All compilation errors have been fixed!**
> See [QUICK_FIX_GUIDE.md](QUICK_FIX_GUIDE.md) for details.

---

## Quick Start Guide

### Step 1: Open the Project
1. Open Unity Hub
2. Open project: `D:/PROJECTS/UNITY/chor-police`
3. Use Unity 6000.3.9f1 (or later)
4. Wait for Unity to open the project

### Step 2: Verify Setup (Optional but Recommended)
1. In Unity Editor, go to the menu bar
2. Click: **ChorPolice > Verify Setup**
3. Check Console for green ✅ checks
4. Fix any warnings if needed

### Step 3: Run Quick Setup
1. In Unity Editor, go to the menu bar
2. Click: **ChorPolice > Quick Setup**
3. Wait for scene generation to complete (5-10 seconds)
4. You should see "Setup Complete!" dialog

### Step 4: Play the Game
1. Press **Play** button in Unity Editor
2. Click **START GAME** on the main menu
3. Enjoy playing Chor Police!

---

## What You'll See

### Main Menu
- Title: "🎮 CHOR POLICE"
- Subtitle: "A Game of Deception"
- Green Start Button

### Game Flow

1. **Shuffle** (2 seconds) - Cards shuffling
2. **Role Reveal** - See your role (and others if Police)
3. **Police Pick** - Select who you think is target
4. **Reveal Result** - See if Police caught target
5. **Scorecard** - See everyone's scores
6. **Repeat** for 10 rounds
7. **Game Over** - See winner and leaderboard

---

## Controls

- **Mouse Click**: Select suspects, continue, start game
- No keyboard controls required

---

## Testing

### Automated Test
- Menu: **ChorPolice > Run Automated Play Test**
- Watch Console for results

### Manual Test
- Just press Play and play through the game!

---

## Troubleshooting

### Scene Won't Generate
- Ensure `Assets/Editor/CompleteSceneGenerator.cs` exists
- Check Console for compilation errors
- Try Unity menu: `Assets > Reimport All`
- See [QUICK_FIX_GUIDE.md](QUICK_FIX_GUIDE.md) for help

### ScoreConfig Warning
- Run: **ChorPolice > Assign ScoreConfig to Manager**
- This will link the ScoreConfig automatically

### Game Won't Start
- Run: **ChorPolice > Verify Setup**
- Check Console for any missing components
- See [QUICK_FIX_GUIDE.md](QUICK_FIX_GUIDE.md) for detailed help

### AI or UI Not Working
- Run: **ChorPolice > Verify Setup**
- Check if AIManager and UIManager_TMP are found
- If missing, run **ChorPolice > Generate Complete Game Scene**

---

## Game Rules

### Roles
- 👮 **Police** - Must catch Chor or Dakat
- 🕵️ **Chor (Thief)** - Must escape when target
- 💼 **Babu (Innocent)** - Gets free points for surviving
- 💣 **Dakat (Robber)** - Must escape when target

### Scoring
| Role     | Points               |
|----------|----------------------|
| Babu     | 1000 (always)        |
| Police   | 800 (if catches)     |
| Chor     | 700 (if escapes)     |
| Dakat    | 600 (if escapes)     |

### How to Win
- Play 10 rounds
- Accumulate the highest total score
- The player with the most points wins!

---

## Customization

### Change AI Difficulty
1. Select `Managers > AIManager` GameObject in scene
2. In Inspector, change `Global Correct Pick Chance`
3. 0.1 = Very Easy, 0.5 = Medium, 0.9 = Very Hard

### Change Scores
1. Open `Assets/Resources/ScoreConfig.asset`
2. Change values in Inspector
3. Save (Ctrl+S)

---

## Documentation

- **QUICK_FIX_GUIDE.md** - ⭐ Start here! Fix details and usage
- **UI_SETUP_README.md** - Detailed UI setup guide
- **IMPLEMENTATION_SUMMARY.md** - What was implemented
- **AGENT.md** - Architecture and coding guidelines
- **ASSEMBLY_FIXES.md** - Technical fix details

---

## Need Help?

1. Check **Console** for error messages
2. Run **ChorPolice > Verify Setup** to check components
3. Read **QUICK_FIX_GUIDE.md** for detailed help
4. Read **ASSEMBLY_FIXES.md** for technical details
5. Check code comments in scripts

## Quick Commands

```
ChorPolice > Quick Setup                    ← One-click setup
ChorPolice > Verify Setup                  ← Check all components
ChorPolice > Run Automated Play Test        ← Test game flow
ChorPolice > Assign ScoreConfig to Manager  ← Link ScoreConfig
ChorPolice > List All Components           ← Debug all components
```

---

## File Locations

### Main Scene
- `Assets/Scenes/GameScene.unity`

### Scripts
- `Assets/Scripts/Core/` - Game logic
- `Assets/Scripts/UI/` - UI components
- `Assets/Scripts/Player/` - Player and AI
- `Assets/Editor/` - Setup and test tools

### Data
- `Assets/Resources/ScoreConfig.asset` - Score configuration

---

## Enjoy Playing! 🎉

Built with ❤️ to revive our childhood games using modern multiplayer tech.

**Author**: Sadikur Rahman
**Unity Version**: 6000.3.9f1
**Status**: ✅ Ready to Play!
