// UIManager_TMP.cs
// Author: Sadikur Rahman
// Description: Complete UI Manager using TextMeshPro for Chor Police game.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class UIManager_TMP : MonoBehaviour
{
    public static UIManager_TMP Instance;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject shufflePanel;
    public GameObject roleRevealPanel;
    public GameObject policePickPanel;
    public GameObject revealResultPanel;
    public GameObject scorecardPanel;
    public GameObject gameOverPanel;
    public GameObject hudPanel;

    [Header("Main Menu")]
    public Button startButton;

    [Header("Shuffle Panel")]
    public TextMeshProUGUI shuffleTimerText;

    [Header("Role Reveal Panel")]
    public Button roleRevealContinueButton;

    [Header("Police Pick Panel")]
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI targetText;
    public Button suspect1Button;
    public Button suspect2Button;
    public Button suspect3Button;
    public TextMeshProUGUI suspect1Name;
    public TextMeshProUGUI suspect2Name;
    public TextMeshProUGUI suspect3Name;

    [Header("Reveal Result Panel")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI pickedPlayerText;

    [Header("Scorecard Panel")]
    public TextMeshProUGUI roundTitleText;
    public TextMeshProUGUI scorecardResultText;
    public TextMeshProUGUI scorecardPickedPlayerText;
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
    public Button nextRoundButton;

    [Header("Game Over Panel")]
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI firstPlaceName;
    public TextMeshProUGUI firstPlaceScore;
    public TextMeshProUGUI secondPlaceName;
    public TextMeshProUGUI secondPlaceScore;
    public TextMeshProUGUI thirdPlaceName;
    public TextMeshProUGUI thirdPlaceScore;
    public TextMeshProUGUI fourthPlaceName;
    public TextMeshProUGUI fourthPlaceScore;
    public Button playAgainButton;
    public Button mainMenuButton;

    [Header("HUD")]
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI hudTargetText;

    [Header("Role Cards")]
    public GameObject playerRoleCard;
    public TextMeshProUGUI playerRoleText;
    public GameObject ai1RoleCard;
    public TextMeshProUGUI ai1RoleText;
    public GameObject ai2RoleCard;
    public TextMeshProUGUI ai2RoleText;
    public GameObject ai3RoleCard;
    public TextMeshProUGUI ai3RoleText;

    [Header("Colors")]
    public Color winColor = Color.green;
    public Color loseColor = Color.red;
    public Color neutralColor = Color.white;
    public Color policeColor = new Color(0.2f, 0.4f, 1f);
    public Color chorColor = new Color(1f, 0.8f, 0f);
    public Color dakatColor = new Color(0.8f, 0.2f, 0.2f);
    public Color babuColor = new Color(0.3f, 0.8f, 0.3f);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Subscribe to state changes
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
            GameStateManager.Instance.OnRevealResult += ShowRevealResult;
        }

        // Setup button listeners
        SetupButtonListeners();

        // Hide all panels initially
        HideAllPanels();
        ShowMainMenu();
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
            GameStateManager.Instance.OnRevealResult -= ShowRevealResult;
        }
    }

    private void SetupButtonListeners()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartGameClicked);

        if (roleRevealContinueButton != null)
            roleRevealContinueButton.onClick.AddListener(OnRoleRevealCompleteClicked);

        if (suspect1Button != null)
            suspect1Button.onClick.AddListener(() => OnSuspectPicked(0));

        if (suspect2Button != null)
            suspect2Button.onClick.AddListener(() => OnSuspectPicked(1));

        if (suspect3Button != null)
            suspect3Button.onClick.AddListener(() => OnSuspectPicked(2));

        if (nextRoundButton != null)
            nextRoundButton.onClick.AddListener(OnNextRoundClicked);

        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(OnPlayAgainClicked);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnGameStateChanged(GameState newState)
    {
        Debug.Log($"UIManager: State changed to {newState}");
        HideAllPanels();
        ShowPanelForState(newState);
        UpdateHUD(newState);
        UpdateStateSpecificUI(newState);
    }

    private void HideAllPanels()
    {
        SetPanelActive(mainMenuPanel, false);
        SetPanelActive(shufflePanel, false);
        SetPanelActive(roleRevealPanel, false);
        SetPanelActive(policePickPanel, false);
        SetPanelActive(revealResultPanel, false);
        SetPanelActive(scorecardPanel, false);
        SetPanelActive(gameOverPanel, false);

        if (hudPanel != null)
        {
            hudPanel.SetActive(true);
        }
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }

    private void ShowMainMenu()
    {
        SetPanelActive(mainMenuPanel, true);
        if (hudPanel != null) hudPanel.SetActive(false);
    }

    private void ShowPanelForState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                SetPanelActive(mainMenuPanel, true);
                if (hudPanel != null) hudPanel.SetActive(false);
                break;
            case GameState.Shuffling:
                SetPanelActive(shufflePanel, true);
                StartShuffleAnimation();
                break;
            case GameState.RoleReveal:
                SetPanelActive(roleRevealPanel, true);
                DisplayRoles();
                break;
            case GameState.PolicePick:
                SetPanelActive(policePickPanel, true);
                SetupPolicePickUI();
                break;
            case GameState.RevealResult:
                SetPanelActive(revealResultPanel, true);
                break;
            case GameState.Scorecard:
                SetPanelActive(scorecardPanel, true);
                DisplayRoundResults();
                break;
            case GameState.GameOver:
                SetPanelActive(gameOverPanel, true);
                DisplayFinalResults();
                break;
        }
    }

    private void UpdateHUD(GameState state)
    {
        if (roundText != null)
        {
            if (state != GameState.Menu)
            {
                int currentRound = GameManager.Instance.CurrentRound;
                int totalRounds = GameManager.Instance.TotalRounds;
                roundText.text = $"Round: {currentRound}/{totalRounds}";
            }
            else
            {
                roundText.text = "";
            }
        }

        if (hudTargetText != null)
        {
            if (state == GameState.PolicePick || state == GameState.RevealResult || state == GameState.Scorecard)
            {
                RoleType target = GameManager.Instance.CurrentTarget;
                hudTargetText.text = target == RoleType.CHOR ? "Find the CHOR!" : "Find the DAKAT!";
                hudTargetText.color = target == RoleType.CHOR ? chorColor : dakatColor;
            }
            else
            {
                hudTargetText.text = "";
            }
        }
    }

    private void UpdateStateSpecificUI(GameState state)
    {
        if (state == GameState.Shuffling)
        {
            // Shuffle animation is handled by coroutine
        }
    }

    private void StartShuffleAnimation()
    {
        StartCoroutine(ShuffleAnimationCoroutine());
    }

    private System.Collections.IEnumerator ShuffleAnimationCoroutine()
    {
        float shuffleTime = 2f;
        float timer = shuffleTime;

        while (timer > 0)
        {
            if (shuffleTimerText != null)
            {
                shuffleTimerText.text = $"Shuffling... {timer:F1}s";
            }
            timer -= Time.deltaTime;
            yield return null;
        }

        if (shuffleTimerText != null)
        {
            shuffleTimerText.text = "Shuffling... 0.0s";
        }
    }

    private void DisplayRoles()
    {
        var players = GameManager.Instance.Players;
        if (players.Count < 4) return;

        // Hide all role cards initially
        SetCardActive(playerRoleCard, false);
        SetCardActive(ai1RoleCard, false);
        SetCardActive(ai2RoleCard, false);
        SetCardActive(ai3RoleCard, false);

        // Set role texts
        SetRoleText(playerRoleText, players[0].Role);
        SetRoleText(ai1RoleText, players[1].Role);
        SetRoleText(ai2RoleText, players[2].Role);
        SetRoleText(ai3RoleText, players[3].Role);

        // Update card colors based on roles
        UpdateCardColors(players);

        // Show cards with animation
        StartCoroutine(RevealRolesCoroutine());
    }

    private void SetCardActive(GameObject card, bool active)
    {
        if (card != null)
        {
            card.SetActive(active);
        }
    }

    private void SetRoleText(TextMeshProUGUI text, RoleType role)
    {
        if (text != null)
        {
            text.text = GetRoleDisplayName(role);
        }
    }



    private void UpdateCardColors(List<PlayerData> players)
    {
        // Update background colors based on roles
        if (players.Count < 4) return;

        SetCardBackgroundColor(playerRoleCard, GetRoleColor(players[0].Role));
        SetCardBackgroundColor(ai1RoleCard, GetRoleColor(players[1].Role));
        SetCardBackgroundColor(ai2RoleCard, GetRoleColor(players[2].Role));
        SetCardBackgroundColor(ai3RoleCard, GetRoleColor(players[3].Role));
    }

    private void SetCardBackgroundColor(GameObject card, Color color)
    {
        if (card != null)
        {
            Image cardImage = card.GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.color = color;
            }
        }
    }

    private System.Collections.IEnumerator RevealRolesCoroutine()
    {
        float revealDelay = 0.3f;

        // Reveal cards one by one
        if (playerRoleCard != null)
        {
            SetCardActive(playerRoleCard, true);
            yield return new WaitForSeconds(revealDelay);
        }

        if (ai1RoleCard != null)
        {
            SetCardActive(ai1RoleCard, true);
            yield return new WaitForSeconds(revealDelay);
        }

        if (ai2RoleCard != null)
        {
            SetCardActive(ai2RoleCard, true);
            yield return new WaitForSeconds(revealDelay);
        }

        if (ai3RoleCard != null)
        {
            SetCardActive(ai3RoleCard, true);
            yield return new WaitForSeconds(revealDelay);
        }

        // Show continue button
        if (roleRevealContinueButton != null)
        {
            roleRevealContinueButton.gameObject.SetActive(true);
        }
    }

    private Color GetRoleColor(RoleType role)
    {
        switch (role)
        {
            case RoleType.POLICE: return policeColor;
            case RoleType.CHOR: return chorColor;
            case RoleType.DAKAT: return dakatColor;
            case RoleType.BABU: return babuColor;
            default: return neutralColor;
        }
    }

    private string GetRoleDisplayName(RoleType role)
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

    private void SetupPolicePickUI()
    {
        PlayerData policePlayer = GameManager.Instance.PolicePlayer;

        if (policePlayer == null)
        {
            Debug.LogError("No Police player found!");
            return;
        }

        // Get all suspects (non-police players)
        var suspects = GameManager.Instance.Players.FindAll(p => p.Role != RoleType.POLICE);

        if (suspects.Count < 3)
        {
            Debug.LogError($"Not enough suspects: {suspects.Count}");
            return;
        }

        RoleType currentTarget = GameManager.Instance.CurrentTarget;

        // Update instruction text
        if (instructionText != null)
        {
            if (policePlayer.IsAI)
            {
                instructionText.text = "AI Police is choosing...";
            }
            else
            {
                instructionText.text = $"You are the POLICE!\nSelect who you think is the {currentTarget}.";
            }
        }

        // Update target text
        if (targetText != null)
        {
            targetText.text = currentTarget == RoleType.CHOR ? "Find the CHOR!" : "Find the DAKAT!";
            targetText.color = currentTarget == RoleType.CHOR ? chorColor : dakatColor;
        }

        // Update suspect names
        if (suspect1Name != null) suspect1Name.text = suspects[0].Name;
        if (suspect2Name != null) suspect2Name.text = suspects[1].Name;
        if (suspect3Name != null) suspect3Name.text = suspects[2].Name;

        // Enable/disable buttons based on whether human is Police
        bool humanIsPolice = !policePlayer.IsAI;
        SetButtonInteractable(suspect1Button, humanIsPolice);
        SetButtonInteractable(suspect2Button, humanIsPolice);
        SetButtonInteractable(suspect3Button, humanIsPolice);

        // If AI is Police, trigger AI pick
        if (policePlayer.IsAI && AIManager.Instance != null)
        {
            // AI Manager will handle the pick
            AIPlayer aiPolice = AIManager.Instance.GetAIPlayer(policePlayer);
            if (aiPolice != null)
            {
                aiPolice.MakePolicePick();
            }
        }
    }

    private void SetButtonInteractable(Button button, bool interactable)
    {
        if (button != null)
        {
            button.interactable = interactable;
        }
    }

    private void OnSuspectPicked(int index)
    {
        var suspects = GameManager.Instance.Players.FindAll(p => p.Role != RoleType.POLICE);

        if (index < 0 || index >= suspects.Count)
        {
            Debug.LogError($"Invalid suspect index: {index}");
            return;
        }

        PlayerData pickedSuspect = suspects[index];
        Debug.Log($"Police picked suspect: {pickedSuspect.Name}");

        // Disable buttons
        SetButtonInteractable(suspect1Button, false);
        SetButtonInteractable(suspect2Button, false);
        SetButtonInteractable(suspect3Button, false);

        // Notify game state manager
        GameStateManager.Instance.OnPolicePickComplete(pickedSuspect);
    }

    public void ShowRevealResult(bool didWin, PlayerData pickedTarget, RoleType target)
    {
        if (resultText != null)
        {
            if (didWin)
            {
                resultText.text = $"🎉 Police Caught the {target}!";
                resultText.color = winColor;
            }
            else
            {
                resultText.text = $"❌ Police Missed! It was {pickedTarget.Role}!";
                resultText.color = loseColor;
            }
        }

        if (pickedPlayerText != null)
        {
            pickedPlayerText.text = $"Picked: {pickedTarget.Name} ({GetRoleDisplayName(pickedTarget.Role)})";
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
        if (scorecardResultText != null)
        {
            if (didPoliceWin)
            {
                scorecardResultText.text = "🎉 Police Caught the Target!";
                scorecardResultText.color = winColor;
            }
            else
            {
                scorecardResultText.text = "❌ Police Missed!";
                scorecardResultText.color = loseColor;
            }
        }

        // Display picked player
        var pickedTarget = GameManager.Instance.PickedTarget;
        if (scorecardPickedPlayerText != null && pickedTarget != null)
        {
            scorecardPickedPlayerText.text = $"Picked: {pickedTarget.Name} ({GetRoleDisplayName(pickedTarget.Role)})";
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
        if (roleText != null)
        {
            roleText.text = GetRoleDisplayName(player.Role);
            roleText.color = GetRoleColor(player.Role);
        }
        if (scoreText != null) scoreText.text = player.Score.ToString();
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
            winnerText.text = $"🏆 {winner.Name} Wins! 🏆";
            winnerText.color = chorColor;
        }

        // Display leaderboard
        DisplayLeaderboardRow(1, firstPlaceName, firstPlaceScore, leaderboard[0], chorColor);
        DisplayLeaderboardRow(2, secondPlaceName, secondPlaceScore, leaderboard[1], new Color(0.75f, 0.75f, 0.75f));
        DisplayLeaderboardRow(3, thirdPlaceName, thirdPlaceScore, leaderboard[2], new Color(0.8f, 0.5f, 0.2f));
        DisplayLeaderboardRow(4, fourthPlaceName, fourthPlaceScore, leaderboard[3], neutralColor);

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

    // Button click handlers
    public void OnStartGameClicked()
    {
        Debug.Log("Start Game clicked");
        GameStateManager.Instance.StartGame();
    }

    public void OnRoleRevealCompleteClicked()
    {
        GameStateManager.Instance.OnRoleRevealComplete();
    }

    public void OnNextRoundClicked()
    {
        GameStateManager.Instance.OnScorecardComplete();
    }

    public void OnPlayAgainClicked()
    {
        Debug.Log("Play Again clicked");
        GameStateManager.Instance.ReturnToMenu();
        GameStateManager.Instance.StartGame();
    }

    public void OnMainMenuClicked()
    {
        Debug.Log("Main Menu clicked");
        GameStateManager.Instance.ReturnToMenu();
    }
}
