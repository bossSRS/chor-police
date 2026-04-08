// RoleManager.cs
// Author: Sadikur Rahman
// Description: Assigns roles randomly to players and tracks role distribution.

using UnityEngine;
using System.Collections.Generic;

public class RoleManager : MonoBehaviour
{
    public static RoleManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AssignRoles(List<PlayerData> players)
    {
        if (players == null || players.Count == 0)
        {
            Debug.LogError("RoleManager: No players to assign roles to!");
            return;
        }

        // Initialize seed to ensure different results every time
        Random.InitState(System.DateTime.Now.Millisecond + (int)Time.realtimeSinceStartup);

        List<RoleType> roles = new List<RoleType> { 
            RoleType.CHOR, 
            RoleType.POLICE, 
            RoleType.BABU, 
            RoleType.DAKAT 
        };

        // Shuffle the roles list
        for (int i = roles.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            RoleType temp = roles[i];
            roles[i] = roles[j];
            roles[j] = temp;
        }

        // Also shuffle a copy of the players list to ensure position-independent assignment
        List<PlayerData> shuffledPlayers = new List<PlayerData>(players);
        for (int i = shuffledPlayers.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            PlayerData temp = shuffledPlayers[i];
            shuffledPlayers[i] = shuffledPlayers[j];
            shuffledPlayers[j] = temp;
        }

        // Assign roles
        for (int i = 0; i < shuffledPlayers.Count; i++)
        {
            shuffledPlayers[i].AssignRole(roles[i]);
            Debug.Log($"RoleManager: Player {shuffledPlayers[i].Name} (IsAI: {shuffledPlayers[i].IsAI}) assigned role: {shuffledPlayers[i].Role}");
        }
    }
}