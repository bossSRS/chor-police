// AIPlayer.cs
// Author: Sadikur Rahman
// Description: AI behavior for computer-controlled players in single-player mode.

using UnityEngine;
using System.Collections;

public class AIPlayer : MonoBehaviour
{
    [Header("AI Settings")]
    [Tooltip("Minimum delay before AI makes a decision (seconds)")]
    public float minThinkTime = 1f;

    [Tooltip("Maximum delay before AI makes a decision (seconds)")]
    public float maxThinkTime = 2.5f;

    [Tooltip("Chance that AI picks correctly (0-1)")]
    [Range(0f, 1f)]
    public float correctPickChance = 0.35f; // 35% chance to pick correctly

    private PlayerData aiPlayer;

    public void Initialize(PlayerData player)
    {
        aiPlayer = player;
    }

    /// <summary>
    /// Called when this AI is the Police and needs to pick a target.
    /// </summary>
    public void MakePolicePick()
    {
        if (aiPlayer == null || !aiPlayer.IsAI)
        {
            Debug.LogError("AIPlayer not properly initialized or not an AI player");
            return;
        }

        float thinkTime = Random.Range(minThinkTime, maxThinkTime);
        StartCoroutine(DelayedPolicePick(thinkTime));
    }

    private IEnumerator DelayedPolicePick(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Get all non-police players as potential suspects
        var suspects = GameManager.Instance.Players.FindAll(p => p.Role != RoleType.POLICE);

        if (suspects.Count == 0)
        {
            Debug.LogError("No suspects available for AI to pick");
            yield break;
        }

        PlayerData pickedSuspect;

        // AI has a chance to pick the correct target
        if (Random.value < correctPickChance)
        {
            // Pick the actual target (CHOR or DAKAT)
            RoleType currentTarget = GameManager.Instance.CurrentTarget;
            pickedSuspect = suspects.Find(p => p.Role == currentTarget);

            // If target not found in suspects (shouldn't happen), pick random
            if (pickedSuspect == null)
            {
                pickedSuspect = suspects[Random.Range(0, suspects.Count)];
            }

            Debug.Log($"AI {aiPlayer.Name} picked correctly: {pickedSuspect.Name}");
        }
        else
        {
            // Pick a random suspect (likely wrong)
            pickedSuspect = suspects[Random.Range(0, suspects.Count)];
            Debug.Log($"AI {aiPlayer.Name} picked randomly: {pickedSuspect.Name}");
        }

        // Submit the pick
        GameStateManager.Instance.OnPolicePickComplete(pickedSuspect);
    }

    public void OnPolicePickRequired()
    {
        MakePolicePick();
    }

    /// <summary>
    /// Called when this AI is not Police - used for reaction animations etc.
    /// </summary>
    public void OnRoleRevealed()
    {
        // AI can react based on their role
        if (aiPlayer.Role == RoleType.BABU)
        {
            Debug.Log($"AI {aiPlayer.Name} (BABU) is happy with free points!");
        }
        else if (aiPlayer.Role == RoleType.CHOR || aiPlayer.Role == RoleType.DAKAT)
        {
            RoleType currentTarget = GameManager.Instance.CurrentTarget;
            if (aiPlayer.Role == currentTarget)
            {
                Debug.Log($"AI {aiPlayer.Name} ({aiPlayer.Role}) is nervous - they're the target!");
            }
            else
            {
                Debug.Log($"AI {aiPlayer.Name} ({aiPlayer.Role}) is safe - not the target");
            }
        }
    }

    /// <summary>
    /// Called after police pick is revealed.
    /// </summary>
    public void OnResultRevealed()
    {
        bool didPoliceWin = GameManager.Instance.DidPoliceWin;
        RoleType currentTarget = GameManager.Instance.CurrentTarget;

        if (aiPlayer.Role == RoleType.POLICE)
        {
            if (didPoliceWin)
                Debug.Log($"AI {aiPlayer.Name} celebrates catching the {currentTarget}!");
            else
                Debug.Log($"AI {aiPlayer.Name} is disappointed for missing");
        }
        else if (aiPlayer.Role == currentTarget)
        {
            if (didPoliceWin)
                Debug.Log($"AI {aiPlayer.Name} was caught!");
            else
                Debug.Log($"AI {aiPlayer.Name} escaped!");
        }
    }
}
