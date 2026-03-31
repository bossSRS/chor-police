// GameInitializer.cs
// Author: Sadikur Rahman
// Description: Initializes all game managers and sets up the game scene.

using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Managers")]
    public GameManager gameManager;
    public RoleManager roleManager;
    public RoundManager roundManager;
    public ScoreManager scoreManager;
    public GameStateManager gameStateManager;

    [Header("Managers Order (for Init)")]
    [Tooltip("Managers will be initialized in this order")]
    public BaseManager[] initOrder;

    private void Awake()
    {
        Debug.Log("GameInitializer: Starting initialization...");

        // Ensure singletons exist
        EnsureSingleton(ref gameManager, "GameManager");
        EnsureSingleton(ref roleManager, "RoleManager");
        EnsureSingleton(ref roundManager, "RoundManager");
        EnsureSingleton(ref scoreManager, "ScoreManager");
        EnsureSingleton(ref gameStateManager, "GameStateManager");

        // Load ScoreConfig from Resources
        LoadScoreConfig();

        Debug.Log("GameInitializer: All managers ensured");
    }

    private void Start()
    {
        // Initialize managers in specified order
        if (initOrder != null && initOrder.Length > 0)
        {
            foreach (var manager in initOrder)
            {
                if (manager != null)
                {
                    manager.Init();
                }
            }
        }
        else
        {
            // Default init order if not specified
            if (gameManager != null) gameManager.Init();
            if (scoreManager != null) scoreManager.Init();
            if (roundManager != null) roundManager.Init();
        }

        // Initialize AI players - find and initialize at runtime
        InitializeAIPlayers();

        Debug.Log("GameInitializer: All managers initialized");
    }

    private void EnsureSingleton<T>(ref T manager, string managerName) where T : MonoBehaviour
    {
        if (manager == null)
        {
            // Try to find existing instance
            manager = FindObjectOfType<T>();

            if (manager == null)
            {
                // Create new GameObject with manager
                GameObject managerObj = new GameObject(managerName);
                manager = managerObj.AddComponent<T>();
                Debug.Log($"GameInitializer: Created {managerName}");
            }
        }
    }

    private void InitializeAIPlayers()
    {
        // Find AIManager by component name to avoid assembly reference issues
        // Using reflection to avoid compile-time type dependency
        MonoBehaviour[] allComponents = FindObjectsOfType<MonoBehaviour>();
        foreach (var component in allComponents)
        {
            if (component != null && component.GetType().Name == "AIManager")
            {
                // Use reflection to call InitializeAIPlayers method without compile-time reference
                System.Reflection.MethodInfo method = component.GetType().GetMethod("InitializeAIPlayers");
                if (method != null)
                {
                    method.Invoke(component, null);
                    Debug.Log("GameInitializer: AI Manager initialized successfully");
                }
                return;
            }
        }
        Debug.LogWarning("GameInitializer: AIManager not found in scene. AI features will not work.");
    }

    private void LoadScoreConfig()
    {
        if (scoreManager != null && scoreManager.Config == null)
        {
            // Load from Resources folder
            ScoreConfig config = Resources.Load<ScoreConfig>("ScoreConfig");

            if (config != null)
            {
                scoreManager.Config = config;
                Debug.Log("GameInitializer: Loaded ScoreConfig from Resources");
            }
            else
            {
                Debug.LogWarning("GameInitializer: ScoreConfig not found in Resources folder!");
            }
        }
    }
}
