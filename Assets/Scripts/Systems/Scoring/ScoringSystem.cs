// ScoringSystem.cs
// Author: Sadikur Rahman
// Description: Delegates score logic to IScorable implementations using polymorphism.

using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public ScoreConfig scoreConfig;

    public void CalculateScore(PlayerData player, bool didPoliceWin, RoleType target)
    {
        IScorable scorer = null;

        switch (player.Role)
        {
            case RoleType.BABU:
                scorer = new BabuScorer(scoreConfig);
                break;
            case RoleType.POLICE:
                scorer = new PoliceScorer(scoreConfig);
                break;
            case RoleType.CHOR:
            case RoleType.DAKAT:
                scorer = new TargetScorer(scoreConfig);
                break;
        }

        scorer?.CalculateScore(player, didPoliceWin, target);

        Debug.Log($"Player {player.Name} | Role: {player.Role} | Score: {player.Score}");
    }
}