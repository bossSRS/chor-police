// GameManager.cs
// Author: Sadikur Rahman
// Description: Manages the overall game state, player list, and round progression.

using UnityEngine;
using System.Collections.Generic;

public class GameManager : BaseManager
{
    public static GameManager Instance;

    public List<PlayerData> players = new List<PlayerData>();
    public bool isRoundActive = false;
    public int currentRound = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public override void Init()
    {
        Debug.Log("GameManager Init()");
    }

    public void StartGame()
    {
        currentRound = 1;
        isRoundActive = true;
        Debug.Log("Game Started");

        RoleManager.Instance.AssignRoles(players);
    }

    public void EndRound()
    {
        isRoundActive = false;
        currentRound++;
    }

    public override void ResetManager()
    {
        currentRound = 1;
        foreach (var player in players)
            player.Score = 0;
    }
}