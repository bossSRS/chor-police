// PoliceScorer.cs
// Author: Sadikur Rahman
// Description: Score logic for POLICE role using ScoreConfig.

public class PoliceScorer : IScorable
{
    private readonly ScoreConfig config;

    public PoliceScorer(ScoreConfig config)
    {
        this.config = config;
    }

    public void CalculateScore(PlayerData player, bool didPoliceWin, RoleType target)
    {
        player.Score += didPoliceWin ? config.policeWinScore : 0;
    }
}