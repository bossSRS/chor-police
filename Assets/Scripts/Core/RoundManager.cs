// RoundManager.cs
// Author: Sadikur Rahman
// Description: Handles round-specific logic such as target type (CHOR or DAKAT) and timers.

using UnityEngine;

public class RoundManager : BaseManager
{
    public static RoundManager Instance;

    public RoleType currentTarget = RoleType.CHOR;
    public float roundTimer = 60f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartRound()
    {
        Debug.Log("Round Started: Target = " + currentTarget);
    }

    public void EndRound()
    {
        Debug.Log("Round Ended");
        currentTarget = (currentTarget == RoleType.CHOR) ? RoleType.DAKAT : RoleType.CHOR;
    }
}