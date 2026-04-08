// GameManager.cs
// Author: Sadikur Rahman
// Description: Manages the overall game state, player list, and round progression.

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : BaseManager
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int TotalRounds = 10;
    public float shuffleDelay = 2f;

    [Header("Players")]
    public List<PlayerData> Players = new List<PlayerData>();
    public PlayerData HumanPlayer => Players.FirstOrDefault(p => p.IsAI == false);
    public PlayerData PolicePlayer => Players.Find(p => p.Role == RoleType.POLICE);

    [Header("Round State")]
    public int CurrentRound { get; private set; } = 0;
    public RoleType CurrentTarget => RoundManager.Instance.currentTarget;
    public PlayerData PickedTarget { get; private set; }
    public bool DidPoliceWin { get; private set; }

    private bool _isInitialized = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (!_isInitialized) Init();
    }

    public override void Init()
    {
        if (_isInitialized) return;
        Debug.Log("GameManager Init()");
        InitializePlayers();
        _isInitialized = true;
    }

    private void InitializePlayers()
    {
        Players.Clear();
        // Create 4 players: 1 human (Police), 3 AI
        Players.Add(new PlayerData(1, "Player") { IsAI = false });
        Players.Add(new PlayerData(2, "AI 1") { IsAI = true });
        Players.Add(new PlayerData(3, "AI 2") { IsAI = true });
        Players.Add(new PlayerData(4, "AI 3") { IsAI = true });

        Debug.Log($"Initialized {Players.Count} players");
    }

    public void StartNewGame()
    {
        CurrentRound = 1;
        ResetScores();
        Debug.Log("New Game Started");
    }

    public void StartNextRound()
    {
        CurrentRound++;
        RoundManager.Instance.EndRound(); // Toggle target
        Debug.Log($"Starting Round {CurrentRound}, Target: {CurrentTarget}");
    }

    public void ProcessPolicePick(PlayerData pickedTarget)
    {
        PickedTarget = pickedTarget;
        DidPoliceWin = (pickedTarget.Role == CurrentTarget);

        Debug.Log($"Police picked {pickedTarget.Name} ({pickedTarget.Role}) - Target was {CurrentTarget}. Win: {DidPoliceWin}");

        CalculateScores();
    }

    private void CalculateScores()
    {
        foreach (var player in Players)
        {
            switch (player.Role)
            {
                case RoleType.BABU:
                    player.Score += ScoreManager.Instance.Config.babuScore;
                    break;

                case RoleType.POLICE:
                    if (DidPoliceWin)
                        player.Score += ScoreManager.Instance.Config.policeWinScore;
                    break;

                case RoleType.CHOR:
                case RoleType.DAKAT:
                    var scorer = new TargetScorer(ScoreManager.Instance.Config);
                    scorer.CalculateScore(player, DidPoliceWin, CurrentTarget);
                    break;
            }
        }
    }

    public List<PlayerData> GetLeaderboard()
    {
        // Sort by score in descending order
        List<PlayerData> sortedPlayers = new List<PlayerData>(Players);
        sortedPlayers.Sort((a, b) => b.Score.CompareTo(a.Score));
        return sortedPlayers;
    }

    public void ResetGame()
    {
        CurrentRound = 0;
        PickedTarget = null;
        DidPoliceWin = false;
        ResetScores();
        RoundManager.Instance.currentTarget = RoleType.CHOR;
    }

    private void ResetScores()
    {
        foreach (var player in Players)
            player.Score = 0;
    }

    public override void ResetManager()
    {
        ResetGame();
    }
}
