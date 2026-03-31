// RolePanelUI.cs
// Author: Sadikur Rahman
// Description: Displays role cards for all players with reveal animation.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RolePanelUI : MonoBehaviour
{
    [Header("Role Cards")]
    public GameObject playerRoleCard;
    public GameObject ai1RoleCard;
    public GameObject ai2RoleCard;
    public GameObject ai3RoleCard;

    [Header("Card Elements")]
    public TextMeshProUGUI playerRoleText;
    public TextMeshProUGUI ai1RoleText;
    public TextMeshProUGUI ai2RoleText;
    public TextMeshProUGUI ai3RoleText;

    [Header("Card Names")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI ai1NameText;
    public TextMeshProUGUI ai2NameText;
    public TextMeshProUGUI ai3NameText;

    [Header("Buttons")]
    public Button continueButton;

    [Header("Animation Settings")]
    public float revealDelay = 0.5f;
    public float cardFlipDuration = 0.3f;

    private void OnEnable()
    {
        // Subscribe to state changes
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
        }

        // Setup continue button
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
        }

        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueClicked);
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.RoleReveal)
        {
            DisplayRoles();
        }
    }

    private void DisplayRoles()
    {
        var players = GameManager.Instance.Players;
        if (players.Count < 4) return;

        // Hide all cards initially
        HideAllCards();

        // Set player names
        if (playerNameText) playerNameText.text = players[0].Name;
        if (ai1NameText) ai1NameText.text = players[1].Name;
        if (ai2NameText) ai2NameText.text = players[2].Name;
        if (ai3NameText) ai3NameText.text = players[3].Name;

        // Set role text (hidden initially)
        if (playerRoleText) playerRoleText.text = GetRoleDisplayName(players[0].Role);
        if (ai1RoleText) ai1RoleText.text = GetRoleDisplayName(players[1].Role);
        if (ai2RoleText) ai2RoleText.text = GetRoleDisplayName(players[2].Role);
        if (ai3RoleText) ai3RoleText.text = GetRoleDisplayName(players[3].Role);

        // Start reveal animation
        StartCoroutine(RevealRolesAnimation());
    }

    private IEnumerator RevealRolesAnimation()
    {
        // Show cards one by one
        if (playerRoleCard != null)
        {
            playerRoleCard.SetActive(true);
            yield return new WaitForSeconds(revealDelay);
        }

        if (ai1RoleCard != null)
        {
            ai1RoleCard.SetActive(true);
            yield return new WaitForSeconds(revealDelay);
        }

        if (ai2RoleCard != null)
        {
            ai2RoleCard.SetActive(true);
            yield return new WaitForSeconds(revealDelay);
        }

        if (ai3RoleCard != null)
        {
            ai3RoleCard.SetActive(true);
            yield return new WaitForSeconds(revealDelay);
        }

        // Enable continue button after reveal
        if (continueButton != null)
            continueButton.gameObject.SetActive(true);
    }

    private void HideAllCards()
    {
        if (playerRoleCard) playerRoleCard.SetActive(false);
        if (ai1RoleCard) ai1RoleCard.SetActive(false);
        if (ai2RoleCard) ai2RoleCard.SetActive(false);
        if (ai3RoleCard) ai3RoleCard.SetActive(false);

        if (continueButton) continueButton.gameObject.SetActive(false);
    }

    private string GetRoleDisplayName(RoleType role)
    {
        switch (role)
        {
            case RoleType.CHOR: return "CHOR (Thief)";
            case RoleType.POLICE: return "POLICE";
            case RoleType.BABU: return "BABU (Victim)";
            case RoleType.DAKAT: return "DAKAT (Robber)";
            default: return "Unknown";
        }
    }

    private void OnContinueClicked()
    {
        GameStateManager.Instance.OnRoleRevealComplete();
    }
}
