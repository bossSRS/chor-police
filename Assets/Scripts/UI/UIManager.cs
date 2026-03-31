// UIManager.cs
// Author: Sadikur Rahman
// Description: Controls the UI updates for roles, timers, results, and scores.

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject shufflePanel;
    public GameObject roleRevealPanel;
    public GameObject policePickPanel;
    public GameObject revealResultPanel;
    public GameObject scorecardPanel;
    public GameObject gameOverPanel;

    [Header("HUD Elements")]
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI shuffleTimerText;

    [Header("Result Elements")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI pickedPlayerText;

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

        // Hide all panels initially
        HideAllPanels();
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
            GameStateManager.Instance.OnRevealResult -= ShowRevealResult;
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        HideAllPanels();
        ShowPanelForState(newState);
        UpdateHUD(newState);
    }

    private void HideAllPanels()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (shufflePanel) shufflePanel.SetActive(false);
        if (roleRevealPanel) roleRevealPanel.SetActive(false);
        if (policePickPanel) policePickPanel.SetActive(false);
        if (revealResultPanel) revealResultPanel.SetActive(false);
        if (scorecardPanel) scorecardPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    private void ShowPanelForState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                if (mainMenuPanel) mainMenuPanel.SetActive(true);
                break;
            case GameState.Shuffling:
                if (shufflePanel) shufflePanel.SetActive(true);
                break;
            case GameState.RoleReveal:
                if (roleRevealPanel) roleRevealPanel.SetActive(true);
                break;
            case GameState.PolicePick:
                if (policePickPanel) policePickPanel.SetActive(true);
                break;
            case GameState.RevealResult:
                if (revealResultPanel) revealResultPanel.SetActive(true);
                break;
            case GameState.Scorecard:
                if (scorecardPanel) scorecardPanel.SetActive(true);
                break;
            case GameState.GameOver:
                if (gameOverPanel) gameOverPanel.SetActive(true);
                break;
        }
    }

    private void UpdateHUD(GameState state)
    {
        if (roundText)
        {
            // Only show round counter after game has started
            if (state != GameState.Menu)
                roundText.text = $"Round: {GameManager.Instance.CurrentRound}/{GameManager.Instance.TotalRounds}";
            else
                roundText.text = "";
        }

        // Only show target text after roles are revealed (Police knows their role)
        if (targetText)
        {
            if (state == GameState.PolicePick || state == GameState.RevealResult || state == GameState.Scorecard)
            {
                RoleType target = GameManager.Instance.CurrentTarget;
                targetText.text = target == RoleType.CHOR ? "Find the CHOR!" : "Find the DAKAT!";
            }
            else
            {
                targetText.text = "";
            }
        }
    }

    public void UpdateShuffleTimer(float timeRemaining)
    {
        if (shuffleTimerText)
            shuffleTimerText.text = $"Shuffling... {timeRemaining:F1}s";
    }

    public void ShowRevealResult(bool didWin, PlayerData pickedTarget, RoleType target)
    {
        if (resultText)
        {
            if (didWin)
                resultText.text = $"Police Caught the {target}!";
            else
                resultText.text = $"Police Missed! It was {pickedTarget.Role}!";
        }

        if (pickedPlayerText)
            pickedPlayerText.text = $"Picked: {pickedTarget.Name}";
    }

    public void OnStartGameClicked()
    {
        GameStateManager.Instance.StartGame();
    }

    public void OnPlayAgainClicked()
    {
        GameStateManager.Instance.ReturnToMenu();
        GameStateManager.Instance.StartGame();
    }

    public void OnMainMenuClicked()
    {
        GameStateManager.Instance.ReturnToMenu();
    }
}
