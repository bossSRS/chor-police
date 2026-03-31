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

        List<RoleType> roles = new List<RoleType> { RoleType.CHOR, RoleType.POLICE, RoleType.BABU, RoleType.DAKAT };

        // Fisher-Yates shuffle for proper randomization
        for (int i = roles.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (roles[i], roles[j]) = (roles[j], roles[i]);
        }

        for (int i = 0; i < players.Count; i++)
        {
            players[i].AssignRole(roles[i]);
            Debug.Log($"Player {players[i].Name} assigned role: {players[i].Role}");
        }
    }
}