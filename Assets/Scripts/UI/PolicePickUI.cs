// PolicePickUI.cs
// Author: Sadikur Rahman
// Description: UI for Police player to select their target suspect.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PolicePickUI : MonoBehaviour
{
    public static PolicePickUI Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI targetText;

    [Header("Suspect Buttons")]
    public Button suspect1Button;
    public Button suspect2Button;
    public Button suspect3Button;

    [Header("Suspect Names")]
    public TextMeshProUGUI suspect1Name;
    public TextMeshProUGUI suspect2Name;
    public TextMeshProUGUI suspect3Name;

    private List<Button> suspectButtons = new List<Button>();
    private List<PlayerData> suspects = new List<PlayerData>();

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

        // Setup suspect buttons
        SetupSuspectButtons();
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
        }
    }

    private void SetupSuspectButtons()
    {
        suspectButtons.Clear();

        if (suspect1Button != null)
        {
            suspectButtons.Add(suspect1Button);
            suspect1Button.onClick.AddListener(() => OnSuspectPicked(0));
        }

        if (suspect2Button != null)
        {
            suspectButtons.Add(suspect2Button);
            suspect2Button.onClick.AddListener(() => OnSuspectPicked(1));
        }

        if (suspect3Button != null)
        {
            suspectButtons.Add(suspect3Button);
            suspect3Button.onClick.AddListener(() => OnSuspectPicked(2));
        }

        // Disable buttons initially
        SetButtonsInteractable(false);
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.PolicePick)
        {
            SetupPolicePickUI();
        }
    }

    private void SetupPolicePickUI()
    {
        // Get all players except the Police
        suspects.Clear();
        var allPlayers = GameManager.Instance.Players;

        foreach (var player in allPlayers)
        {
            if (player.Role != RoleType.POLICE)
            {
                suspects.Add(player);
            }
        }

        // Update instruction text
        RoleType currentTarget = GameManager.Instance.CurrentTarget;
        if (instructionText != null)
        {
            instructionText.text = $"You are the POLICE!\nSelect who you think is the {currentTarget}.";
        }

        if (targetText != null)
        {
            targetText.text = currentTarget == RoleType.CHOR ? "Find the CHOR!" : "Find the DAKAT!";
            targetText.color = currentTarget == RoleType.CHOR ? Color.yellow : Color.red;
        }

        // Update suspect names
        if (suspects.Count >= 3)
        {
            if (suspect1Name != null) suspect1Name.text = suspects[0].Name;
            if (suspect2Name != null) suspect2Name.text = suspects[1].Name;
            if (suspect3Name != null) suspect3Name.text = suspects[2].Name;
        }

        // Enable buttons for interaction
        SetButtonsInteractable(true);

        Debug.Log("Police Pick UI setup complete");
    }

    private void OnSuspectPicked(int index)
    {
        if (index < 0 || index >= suspects.Count)
        {
            Debug.LogError($"Invalid suspect index: {index}");
            return;
        }

        PlayerData pickedSuspect = suspects[index];
        Debug.Log($"Police picked suspect: {pickedSuspect.Name}");

        // Disable buttons to prevent multiple picks
        SetButtonsInteractable(false);

        // Notify game manager
        GameStateManager.Instance.OnPolicePickComplete(pickedSuspect);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        foreach (var button in suspectButtons)
        {
            if (button != null)
                button.interactable = interactable;
        }
    }

    // For AI Police (future implementation)
    public void MakeAIPick(PlayerData policePlayer)
    {
        if (!policePlayer.IsAI) return;

        // AI randomly picks a suspect
        int randomIndex = Random.Range(0, suspects.Count);
        PlayerData pickedSuspect = suspects[randomIndex];

        Debug.Log($"AI Police {policePlayer.Name} picked {pickedSuspect.Name}");

        // Simulate delay for realism
        Invoke(nameof(DelayedAIPick), 1.5f);
    }

    private void DelayedAIPick()
    {
        if (suspects.Count > 0)
        {
            int randomIndex = Random.Range(0, suspects.Count);
            GameStateManager.Instance.OnPolicePickComplete(suspects[randomIndex]);
        }
    }
}
