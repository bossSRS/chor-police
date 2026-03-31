// AIManager.cs
// Author: Sadikur Rahman
// Description: Manages all AI players and triggers their behaviors.

using UnityEngine;
using System.Collections.Generic;

public class AIManager : MonoBehaviour
{
    public static AIManager Instance;

    [Header("AI Settings")]
    [Tooltip("Chance that AI Police picks correctly (0-1)")]
    [Range(0f, 1f)]
    public float globalCorrectPickChance = 0.35f;

    [Tooltip("Min delay for AI decisions")]
    public float minThinkTime = 1f;

    [Tooltip("Max delay for AI decisions")]
    public float maxThinkTime = 2.5f;

    private Dictionary<PlayerData, AIPlayer> aiPlayers = new Dictionary<PlayerData, AIPlayer>();

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
        }
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
        }
    }

    /// <summary>
    /// Register an AI player component for a PlayerData
    /// </summary>
    public void RegisterAIPlayer(PlayerData player, AIPlayer aiComponent)
    {
        if (player == null || aiComponent == null) return;

        aiPlayers[player] = aiComponent;
        aiComponent.Initialize(player);

        // Apply global settings
        // Note: You could expose the AIPlayer settings as public and set them here
    }

    /// <summary>
    /// Initialize AI players from GameManager's player list
    /// </summary>
    public void InitializeAIPlayers()
    {
        foreach (var player in GameManager.Instance.Players)
        {
            if (player.IsAI && !aiPlayers.ContainsKey(player))
            {
                // Create a new AI component for this player
                GameObject aiObj = new GameObject($"AI_{player.Name}");
                AIPlayer aiComponent = aiObj.AddComponent<AIPlayer>();
                RegisterAIPlayer(player, aiComponent);
            }
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.RoleReveal:
                TriggerAIReactions();
                break;

            case GameState.PolicePick:
                CheckIfAIPolice();
                break;

            case GameState.RevealResult:
                TriggerAIResultReactions();
                break;
        }
    }

    private void TriggerAIReactions()
    {
        foreach (var kvp in aiPlayers)
        {
            kvp.Value.OnRoleRevealed();
        }
    }

    private void CheckIfAIPolice()
    {
        // Check if Police player is AI
        PlayerData policePlayer = GameManager.Instance.Players.Find(p => p.Role == RoleType.POLICE);

        if (policePlayer != null && policePlayer.IsAI)
        {
            if (aiPlayers.TryGetValue(policePlayer, out AIPlayer aiPolice))
            {
                aiPolice.MakePolicePick();
            }
            else
            {
                // Create AI component if not exists
                GameObject aiObj = new GameObject($"AI_{policePlayer.Name}");
                AIPlayer aiComponent = aiObj.AddComponent<AIPlayer>();
                RegisterAIPlayer(policePlayer, aiComponent);
                aiComponent.MakePolicePick();
            }
        }
    }

    private void TriggerAIResultReactions()
    {
        foreach (var kvp in aiPlayers)
        {
            kvp.Value.OnResultRevealed();
        }
    }

    /// <summary>
    /// Get the AI component for a specific player
    /// </summary>
    public AIPlayer GetAIPlayer(PlayerData player)
    {
        aiPlayers.TryGetValue(player, out AIPlayer ai);
        return ai;
    }
}
