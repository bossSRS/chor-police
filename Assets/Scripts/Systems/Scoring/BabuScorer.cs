// BabuScorer.cs
// Author: Sadikur Rahman
// Description: Score logic for BABU role using ScoreConfig.

public class BabuScorer : IScorable
{
    private readonly ScoreConfig config;

    public BabuScorer(ScoreConfig config)
    {
        this.config = config;
    }

    public void CalculateScore(PlayerData player, bool didPoliceWin, RoleType target)
    {
        player.Score += config.babuScore;
    }
}