# Chor Police - UI Toolkit Design Document

This document outlines the UI design and architecture for the **Chor Police** Unity game using the **UI Toolkit**. 

## 1. Overview
The UI will be built using Unity's UI Toolkit (UXML for structure, USS for styling). This ensures better performance, easier scalability, and a more modern workflow compared to the older UGUI system. 

The UI needs to accommodate the different states defined in `GameStateManager`:
`Menu` → `Shuffling` → `RoleReveal` → `PolicePick` → `RevealResult` → `Scorecard` → `GameOver`

---

## 2. Core UI Screens & UXML Structure

### 2.1 Main Menu (`MainMenuUI.uxml`)
The entry point of the game.
- **Title:** "Chor Police" (Large, stylized typography)
- **Buttons:**
  - `btn-singleplayer`: Start game against 3 AI
  - `btn-multiplayer`: (Phase 2) Online multiplayer
  - `btn-local`: (Phase 3) WiFi/Bluetooth multiplayer
  - `btn-settings`: Open settings panel
  - `btn-quit`: Exit game

### 2.2 Gameplay HUD (`GameHUDUI.uxml`)
Persistent UI during the gameplay phases.
- **Top Bar:**
  - `lbl-round-counter`: "Round 1 / 10"
  - `lbl-current-target`: "Target: CHOR" or "Target: DAKAT" (Highlights visually depending on the target)
- **Player Avatars/Cards (4 total):**
  - Arranged in a circle or 2x2 grid.
  - Each card contains:
    - `lbl-player-name`: e.g., "Player 1", "AI 1"
    - `icon-role`: Hidden initially (shows a question mark `?`), revealed during `RoleReveal` or `RevealResult`.

### 2.3 Shuffling & Role Reveal (`RoleRevealUI.uxml`)
Overlay for the shuffling and role assignment phase.
- **Shuffling Phase:** Fast-swapping role icons in the center of the screen to build anticipation.
- **Role Reveal Phase:** The player's assigned role card pops up in the center.
  - `lbl-assigned-role`: "You are the POLICE 👮"
  - `btn-continue`: To acknowledge and proceed.

### 2.4 Police Pick Phase (`PolicePickUI.uxml`)
Active only when the human player is the Police, or observing when an AI is the Police.
- **Prompt:** "You are the Police! Catch the [CHOR/DAKAT]!"
- **Selection:** The other 3 player cards become selectable buttons.
- **Confirm Button:** `btn-confirm-arrest` (Enabled after selecting a player).

### 2.5 Reveal Result (`ResultOverlayUI.uxml`)
Shows the immediate outcome of the Police's choice.
- **Status Text:** "Police Caught the Chor!" or "Wrong! Innocent Babu was arrested!"
- **Animation:** The selected player's true role is revealed.

### 2.6 Scoreboard (`ScoreboardUI.uxml`)
Displayed at the end of each round.
- **Table Structure:**
  - Columns: Player Name, Role This Round, Round Points, Total Score
  - Rows: Sorted by Total Score (Leaderboard style)
- **Continue Button:** `btn-next-round`

### 2.7 Game Over (`GameOverUI.uxml`)
The final screen after 10 rounds.
- **Winner Announcement:** "Player 1 Wins!"
- **Final Leaderboard:** Showing all 4 players and their final scores.
- **Buttons:**
  - `btn-play-again`: Restarts the game loop.
  - `btn-main-menu`: Returns to the Main Menu.

---

## 3. USS Styling Strategy (`ChorPoliceStyles.uss`)

### 3.1 Color Palette
- **Primary Background:** Dark slate or wooden board texture (to mimic a physical board game).
- **Text Color:** Off-white or light cream for readability.
- **Role Colors (for accents and text):**
  - **Police:** Blue `#1D4ED8`
  - **Chor:** Dark Gray/Black `#374151`
  - **Babu:** Green `#15803D`
  - **Dakat:** Red `#B91C1C`

### 3.2 Typography
- Use a bold, playful font for headers and roles.
- Use a clean sans-serif font for scores and standard text.
- Utilize UI Toolkit's `-unity-font-definition` for TextMeshPro font assets.

### 3.3 Reusable Classes
- `.panel-container`: Center-aligned, semi-transparent dark background with rounded corners.
- `.btn-primary`: Large, stylized buttons with hover and active states (scale up on hover, change color on click).
- `.player-card`: 
  - Standard state: bordered box.
  - `.player-card--selectable`: Adds a glowing border on hover.
  - `.player-card--selected`: Thick colored border indicating the current selection.
- `.text-highlight`: Utility class for emphasizing target names (Chor/Dakat).

---

## 4. Integration with C# Scripts

To connect the UI Toolkit elements with the existing state machine (`GameStateManager`), a central `UIManager` will be used to listen to state events.

```csharp
public class UIManager : MonoBehaviour
{
    public UIDocument uiDocument;
    
    private void OnEnable()
    {
        GameStateManager.OnStateChanged += HandleStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.OnStateChanged -= HandleStateChange;
    }

    private void HandleStateChange(GameState newState)
    {
        // Hide all panels
        HideAllPanels();

        // Show specific UI based on state
        switch (newState)
        {
            case GameState.Menu:
                ShowMainMenu();
                break;
            case GameState.RoleReveal:
                ShowRoleReveal();
                break;
            case GameState.PolicePick:
                ShowPolicePick();
                break;
            case GameState.Scorecard:
                ShowScoreboard();
                break;
            // Handle other states...
        }
    }
}
```

## 5. Next Steps for Implementation
1. Open the UI Builder in Unity.
2. Create the `ChorPoliceStyles.uss` file and define the base palette.
3. Build the `GameHUDUI.uxml` and verify it scales correctly with different aspect ratios using flexbox.
4. Bind the UXML to the `UIManager` and wire up the logic to the `GameStateManager` events.