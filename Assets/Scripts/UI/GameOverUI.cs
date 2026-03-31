// GameOverUI.cs
// Author: Sadikur Rahman
// Description: Final results screen showing leaderboard and winner.

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [Header("Title")]
    public TextMeshProUGUI gameOverTitleText;
    public TextMeshProUGUI winnerText;

    [Header("Leaderboard")]
    public TextMeshProUGUI firstPlaceName;
    public TextMeshProUGUI firstPlaceScore;
    public TextMeshProUGUI secondPlaceName;
    public TextMeshProUGUI secondPlaceScore;
    public TextMeshProUGUI thirdPlaceName;
    public TextMeshProUGUI thirdPlaceScore;
    public TextMeshProUGUI fourthPlaceName;
    public TextMeshProUGUI fourthPlaceScore;

    [Header("Buttons")]
    public Button playAgainButton;
    public Button mainMenuButton;

    [Header("Colors")]
    public Color firstPlaceColor = Color.yellow;
    public Color secondPlaceColor = new Color(0.75f, 0.75f, 0.75f); // Silver
    public Color thirdPlaceColor = new Color(0.8f, 0.5f, 0.2f); // Bronze

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        // Subscribe to state changes
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
        }

        // Setup buttons
        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(OnPlayAgainClicked);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
        }

        if (playAgainButton != null)
            playAgainButton.onClick.RemoveListener(OnPlayAgainClicked);

        if (mainMenuButton != null)
            mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.GameOver)
        {
            DisplayFinalResults();
        }
    }

    private void DisplayFinalResults()
    {
        // Get leaderboard (sorted by score)
        var leaderboard = GameManager.Instance.GetLeaderboard();

        if (leaderboard.Count < 4)
        {
            Debug.LogError("Not enough players to display leaderboard");
            return;
        }

        // Display winner
        PlayerData winner = leaderboard[0];
        if (winnerText != null)
        {
            winnerText.text = $"{winner.Name} Wins!";
            winnerText.color = firstPlaceColor;
        }

        // Display leaderboard
        DisplayLeaderboardRow(1, firstPlaceName, firstPlaceScore, leaderboard[0], firstPlaceColor);
        DisplayLeaderboardRow(2, secondPlaceName, secondPlaceScore, leaderboard[1], secondPlaceColor);
        DisplayLeaderboardRow(3, thirdPlaceName, thirdPlaceScore, leaderboard[2], thirdPlaceColor);
        DisplayLeaderboardRow(4, fourthPlaceName, fourthPlaceScore, leaderboard[3], Color.white);

        Debug.Log($"Game Over! Winner: {winner.Name} with {winner.Score} points");
    }

    private void DisplayLeaderboardRow(int place, TextMeshProUGUI nameText, TextMeshProUGUI scoreText, PlayerData player, Color placeColor)
    {
        if (nameText != null)
        {
            nameText.text = $"{place}. {player.Name}";
            nameText.color = placeColor;
        }

        if (scoreText != null)
        {
            scoreText.text = player.Score.ToString();
            scoreText.color = placeColor;
        }
    }

    private void OnPlayAgainClicked()
    {
        // Reset and start new game
        GameStateManager.Instance.ReturnToMenu();
        GameStateManager.Instance.StartGame();
    }

    private void OnMainMenuClicked()
    {
        GameStateManager.Instance.ReturnToMenu();
    }
}
