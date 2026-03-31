// GameState.cs
// Author: Sadikur Rahman
// Description: Enum defining all game phases for the state machine.

public enum GameState
{
    Menu,           // Initial state / main menu
    Shuffling,      // 2-second chit shuffle animation
    RoleReveal,     // Show players their roles
    PolicePick,     // Police selects target (CHOR or DAKAT)
    RevealResult,   // Show who was caught
    Scorecard,      // Show round scores
    GameOver        // Final leaderboard
}
