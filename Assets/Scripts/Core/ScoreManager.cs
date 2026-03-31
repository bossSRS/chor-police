// ScoreManager.cs
// Author: Sadikur Rahman
// Description: Manages scoring configuration and provides access to ScoreConfig.

using UnityEngine;

public class ScoreManager : BaseManager
{
    public static ScoreManager Instance;

    [Header("Score Configuration")]
    public ScoreConfig Config;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public override void Init()
    {
        Debug.Log("ScoreManager Init()");

        // Load ScoreConfig from Resources if not assigned
        if (Config == null)
        {
            Config = Resources.Load<ScoreConfig>("ScoreConfig");
            if (Config != null)
            {
                Debug.Log("ScoreManager: Loaded ScoreConfig from Resources");
            }
            else
            {
                Debug.LogWarning("ScoreConfig not assigned to ScoreManager. Using default values.");
                // Note: In Unity, the ScoreConfig should be assigned via Inspector or in Resources folder
            }
        }
    }
}
