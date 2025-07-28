// TargetScorer.cs
// Author: Sadikur Rahman
// Description: Score logic for CHOR or DAKAT role using ScoreConfig.

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
            if (player.Role == RoleType.CHOR && target == RoleType.CHOR)
                player.Score += config.chorSurviveScore;

            if (player.Role == RoleType.DAKAT && target == RoleType.DAKAT)
                player.Score += config.dakatSurviveScore;
        }
    }
}