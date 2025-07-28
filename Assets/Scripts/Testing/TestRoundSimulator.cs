// TestRoundSimulator.cs
// Author: Sadikur Rahman
// Description: Simulates a single round of Chor Police with dummy players.

using UnityEngine;
using System.Collections.Generic;

public class TestRoundSimulator : MonoBehaviour
{
    public ScoringSystem scoringSystem;
    public ScoreConfig scoreConfig;

    void Start()
    {
        SimulateRound();
    }

    void SimulateRound()
    {
        Debug.Log("🌀 Starting Round Simulation...");

        // Step 1: Create dummy players
        List<PlayerData> players = new List<PlayerData>
        {
            new PlayerData(0, "Sadia"),
            new PlayerData(1, "Nayeem"),
            new PlayerData(2, "Rakib"),
            new PlayerData(3, "Sumaiya")
        };

        // Step 2: Assign roles
        RoleManager.Instance.AssignRoles(players);

        // Step 3: Define target (e.g., CHOR this round)
        RoleType target = RoleType.CHOR;
        bool policeGuessedCorrect = Random.Range(0, 2) == 0; // Random win/loss

        Debug.Log($"🎯 Round Target: {target}");
        Debug.Log($"👮 Did Police win this round? => {policeGuessedCorrect}");

        // Step 4: Calculate score for each player
        foreach (var player in players)
        {
            scoringSystem.CalculateScore(player, policeGuessedCorrect, target);
        }

        // Step 5: Display scores
        Debug.Log("🏁 Round End - Final Scores:");
        foreach (var player in players)
        {
            Debug.Log($"{player.Name} ({player.Role}) => Score: {player.Score}");
        }
    }
}