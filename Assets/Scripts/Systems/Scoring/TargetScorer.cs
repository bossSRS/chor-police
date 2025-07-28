// TargetScorer.cs
// Author: Sadikur Rahman
// Description: Handles scoring for CHOR and DAKAT roles with updated survival logic.

public class TargetScorer : IScorable
{
    private readonly ScoreConfig config;

    public TargetScorer(ScoreConfig config)
    {
        this.config = config;
    }

    public void CalculateScore(PlayerData player, bool didPoliceWin, RoleType target)
    {
        bool isChor = player.Role == RoleType.CHOR;
        bool isDakat = player.Role == RoleType.DAKAT;

        if (!isChor && !isDakat) return; // Not a target role

        if (didPoliceWin)
        {
            // Police caught the target
            if (player.Role != target)
            {
                // Other suspect survived
                player.Score += (isChor ? config.chorSurviveScore : config.dakatSurviveScore);
            }
            // else target caught => 0
        }
        else
        {
            // Police failed, both suspects get survival score
            player.Score += (isChor ? config.chorSurviveScore : config.dakatSurviveScore);
        }
    }
}