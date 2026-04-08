// GameSceneSetup.cs
// Author: Sadikur Rahman
// Description: Editor script to automatically set up the GameScene GameObjects.

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class GameSceneSetup : EditorWindow
{
    [MenuItem("ChorPolice/Setup Game Scene")]
    public static void SetupGameScene()
    {
        Debug.Log("Setting up Chor Police Game Scene (UI Toolkit)...");

        // Create Manager GameObject
        GameObject managerObj = new GameObject("GameManager");
        managerObj.AddComponent<GameManager>();
        managerObj.AddComponent<GameStateManager>();
        managerObj.AddComponent<RoleManager>();
        managerObj.AddComponent<RoundManager>();
        managerObj.AddComponent<ScoreManager>();
        managerObj.AddComponent<AIManager>();

        // Create UIManager
        GameObject uiManagerObj = new GameObject("UIManager");
        UIDocument uiDocument = uiManagerObj.AddComponent<UIDocument>();
        UIManager uiManager = uiManagerObj.AddComponent<UIManager>();
        uiManager.uiDocument = uiDocument;

        // Load UXML assets
        uiManager.mainMenuAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/MainMenuUI.uxml");
        uiManager.gameHUDAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/GameHUDUI.uxml");
        uiManager.roleRevealAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/RoleRevealUI.uxml");
        uiManager.policePickAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/PolicePickUI.uxml");
        uiManager.revealResultAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/ResultOverlayUI.uxml");
        uiManager.scoreboardAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/ScoreboardUI.uxml");
        uiManager.gameOverAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/GameOverUI.uxml");

        // Add a default PanelSettings if needed (Unity usually creates one if null when playing, 
        // but it's good practice. For now, leaving it null will auto-generate one at runtime or 
        // user can assign a custom one).

        // Select the manager
        Selection.activeGameObject = managerObj;

        Debug.Log("Game Scene setup complete!");
    }

    [MenuItem("ChorPolice/Create ScoreConfig")]
    public static void CreateScoreConfig()
    {
        ScoreConfig config = ScriptableObject.CreateInstance<ScoreConfig>();

        // Set default values
        config.babuScore = 1000;
        config.policeWinScore = 800;
        config.chorSurviveScore = 700;
        config.chorCaughtScorePartial = 400;
        config.dakatSurviveScore = 600;
        config.dakatCaughtScorePartial = 600;

        // Create the asset
        string path = "Assets/Resources/ScoreConfig.asset";
        System.IO.Directory.CreateDirectory("Assets/Resources");
        AssetDatabase.CreateAsset(config, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Select the asset
        Selection.activeObject = config;
        EditorUtility.FocusProjectWindow();

        Debug.Log($"ScoreConfig created at {path}");
    }
}
