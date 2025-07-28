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
        List<RoleType> roles = new List<RoleType> { RoleType.CHOR, RoleType.POLICE, RoleType.BABU, RoleType.DAKAT };
        roles.Sort((a, b) => Random.Range(-1, 2)); // simple shuffle logic

        for (int i = 0; i < players.Count; i++)
        {
            players[i].AssignRole(roles[i]);
            Debug.Log($"Player {players[i].Name} assigned role: {players[i].Role}");
        }
    }
}