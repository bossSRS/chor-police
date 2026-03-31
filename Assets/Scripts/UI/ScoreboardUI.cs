// ScoreboardUI.cs
// Author: Sadikur Rahman
// Description: Displays round results and current scores.

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreboardUI : MonoBehaviour
{
    public static ScoreboardUI Instance;

    [Header("Round Info")]
    public TextMeshProUGUI roundTitleText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI pickedPlayerText;

    [Header("Score Rows")]
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player1RoleText;
    public TextMeshProUGUI player1ScoreText;

    public TextMeshProUGUI player2NameText;
    public TextMeshProUGUI player2RoleText;
    public TextMeshProUGUI player2ScoreText;

    public TextMeshProUGUI player3NameText;
    public TextMeshProUGUI player3RoleText;
    public TextMeshProUGUI player3ScoreText;

    public TextMeshProUGUI player4NameText;
    public TextMeshProUGUI player4RoleText;
    public TextMeshProUGUI player4ScoreText;

    [Header("Buttons")]
    public Button nextRoundButton;

    [Header("Colors")]
    public Color winColor = Color.green;
    public Color loseColor = Color.red;
    public Color neutralColor = Color.white;

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

        // Setup next round button
        if (nextRoundButton != null)
            nextRoundButton.onClick.AddListener(OnNextRoundClicked);
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
        }

        if (nextRoundButton != null)
            nextRoundButton.onClick.RemoveListener(OnNextRoundClicked);
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Scorecard)
        {
            DisplayRoundResults();
        }
    }

    private void DisplayRoundResults()
    {
        // Update round title
        if (roundTitleText != null)
        {
            int currentRound = GameManager.Instance.CurrentRound;
            int totalRounds = GameManager.Instance.TotalRounds;
            roundTitleText.text = $"Round {currentRound} Results";
        }

        // Display result
        bool didPoliceWin = GameManager.Instance.DidPoliceWin;
        if (resultText != null)
        {
            if (didPoliceWin)
            {
                resultText.text = "Police Caught the Target!";
                resultText.color = winColor;
            }
            else
            {
                resultText.text = "Police Missed!";
                resultText.color = loseColor;
            }
        }

        // Display picked player
        var pickedTarget = GameManager.Instance.PickedTarget;
        if (pickedPlayerText != null && pickedTarget != null)
        {
            pickedPlayerText.text = $"Picked: {pickedTarget.Name} ({GetRoleShortName(pickedTarget.Role)})";
        }

        // Display all player scores
        var players = GameManager.Instance.Players;
        if (players.Count >= 4)
        {
            UpdatePlayerRow(player1NameText, player1RoleText, player1ScoreText, players[0]);
            UpdatePlayerRow(player2NameText, player2RoleText, player2ScoreText, players[1]);
            UpdatePlayerRow(player3NameText, player3RoleText, player3ScoreText, players[2]);
            UpdatePlayerRow(player4NameText, player4RoleText, player4ScoreText, players[3]);
        }

        // Update button text for last round
        if (nextRoundButton != null)
        {
            bool isLastRound = GameManager.Instance.CurrentRound >= GameManager.Instance.TotalRounds;
            var buttonText = nextRoundButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = isLastRound ? "See Final Results" : "Next Round";
            }
        }
    }

    private void UpdatePlayerRow(TextMeshProUGUI nameText, TextMeshProUGUI roleText, TextMeshProUGUI scoreText, PlayerData player)
    {
        if (nameText != null) nameText.text = player.Name;
        if (roleText != null) roleText.text = GetRoleShortName(player.Role);
        if (scoreText != null) scoreText.text = player.Score.ToString();

        // Highlight based on role and outcome
        if (roleText != null)
        {
            bool didPoliceWin = GameManager.Instance.DidPoliceWin;
            RoleType target = GameManager.Instance.CurrentTarget;

            if (player.Role == RoleType.POLICE)
            {
                roleText.color = didPoliceWin ? winColor : loseColor;
            }
            else if (player.Role == target)
            {
                roleText.color = didPoliceWin ? loseColor : winColor;
            }
            else if (player.Role == RoleType.BABU)
            {
                roleText.color = neutralColor; // Babu always gets points
            }
            else
            {
                roleText.color = neutralColor;
            }
        }
    }

    private string GetRoleShortName(RoleType role)
    {
        switch (role)
        {
            case RoleType.CHOR: return "CHOR";
            case RoleType.POLICE: return "POLICE";
            case RoleType.BABU: return "BABU";
            case RoleType.DAKAT: return "DAKAT";
            default: return "???";
        }
    }

    private void OnNextRoundClicked()
    {
        GameStateManager.Instance.OnScorecardComplete();
    }
}
