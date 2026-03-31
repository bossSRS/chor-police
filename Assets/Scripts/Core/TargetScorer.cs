// TargetScorer.cs
// Author: Sadikur Rahman
// Description: Score logic for CHOR or DAKAT role using ScoreConfig.

using UnityEngine;

public class TargetScorer : IScorable
{
    private readonly ScoreConfig config;

    public TargetScorer(ScoreConfig config)
    {
        this.config = config;
    }

    public void CalculateScore(PlayerData player, bool didPoliceWin, RoleType target)
    {
        if (!didPoliceWin)
        {
            // Target survived - full survival points
            if (player.Role == RoleType.CHOR && target == RoleType.CHOR)
                player.Score += config.chorSurviveScore;

            if (player.Role == RoleType.DAKAT && target == RoleType.DAKAT)
                player.Score += config.dakatSurviveScore;
        }
        else
        {
            // Target was caught - 50% chance of partial points
            if (player.Role == RoleType.CHOR && target == RoleType.CHOR)
            {
                if (Random.value < 0.5f)
                {
                    player.Score += config.chorCaughtScorePartial;
                    Debug.Log($"{player.Name} (CHOR) caught but got partial points: {config.chorCaughtScorePartial}");
                }
                else
                {
                    Debug.Log($"{player.Name} (CHOR) caught with 0 points");
                }
            }

            if (player.Role == RoleType.DAKAT && target == RoleType.DAKAT)
            {
                if (Random.value < 0.5f)
                {
                    player.Score += config.dakatCaughtScorePartial;
                    Debug.Log($"{player.Name} (DAKAT) caught but got partial points: {config.dakatCaughtScorePartial}");
                }
                else
                {
                    Debug.Log($"{player.Name} (DAKAT) caught with 0 points");
                }
            }
        }
    }
}
