// GameSceneSetup.cs
// Author: Sadikur Rahman
// Description: Editor script to automatically set up the GameScene GameObjects.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using System;
using System.Reflection;

public class GameSceneSetup : EditorWindow
{
    private static bool tmpAvailable = false;
    private static Type tmpType = null;

    static GameSceneSetup()
    {
        CheckTMPAvailability();
    }

    private static void CheckTMPAvailability()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            tmpType = assembly.GetType("UnityEngine.UI.TextMeshProUGUI");
            if (tmpType != null)
            {
                tmpAvailable = true;
                Debug.Log("TextMeshPro detected - will use for UI setup");
                break;
            }
        }

        if (!tmpAvailable)
        {
            Debug.LogWarning("TextMeshPro not found - falling back to Unity UI Text. Import via Window > TextMeshPro > Import TMP Essentials for better UI.");
        }
    }

    private static Component AddTMPText(GameObject obj)
    {
        if (tmpAvailable && tmpType != null)
        {
            return obj.AddComponent(tmpType);
        }
        // Fallback to Unity UI Text
        return obj.AddComponent<Text>();
    }

    [MenuItem("ChorPolice/Setup Game Scene")]
    public static void SetupGameScene()
    {
        Debug.Log("Setting up Chor Police Game Scene...");

        // Find or create Canvas
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            // Add EventSystem
            EventSystem eventSystem = GameObject.FindFirstObjectByType<EventSystem>();
            if (eventSystem == null)
            {
                GameObject eventSystemObj = new GameObject("EventSystem");
                eventSystemObj.AddComponent<EventSystem>();

                // Try to use the new Input System's input module if available
                var inputModuleType = System.Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem.UI, Version=1.0.0, Culture=neutral, PublicKeyToken=null");
                if (inputModuleType != null)
                {
                    eventSystemObj.AddComponent(inputModuleType);
                }
                else
                {
                    // Fallback to old input module
                    eventSystemObj.AddComponent<StandaloneInputModule>();
                }
            }
        }

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
        uiManagerObj.transform.SetParent(canvas.transform, false);
        UIManager uiManager = uiManagerObj.AddComponent<UIManager>();

        // Create UI Panels
        uiManager.mainMenuPanel = CreatePanel(canvas.transform, "MainMenuPanel");
        uiManager.shufflePanel = CreatePanel(canvas.transform, "ShufflePanel");
        uiManager.roleRevealPanel = CreatePanel(canvas.transform, "RoleRevealPanel");
        uiManager.policePickPanel = CreatePanel(canvas.transform, "PolicePickPanel");
        uiManager.revealResultPanel = CreatePanel(canvas.transform, "RevealResultPanel");
        uiManager.scorecardPanel = CreatePanel(canvas.transform, "ScorecardPanel");
        uiManager.gameOverPanel = CreatePanel(canvas.transform, "GameOverPanel");

        // Add components to panels
        AddRolePanelUI(canvas.transform, "RoleRevealPanel");
        AddPolicePickUI(canvas.transform, "PolicePickPanel");
        AddRevealResultUI(canvas.transform, "RevealResultPanel");
        AddScoreboardUI(canvas.transform, "ScorecardPanel");
        AddGameOverUI(canvas.transform, "GameOverPanel");

        // Create HUD
        CreateHUD(canvas.transform, uiManager);

        // Create Start Button
        CreateStartButton(canvas.transform, uiManager);

        // Select the manager
        Selection.activeGameObject = managerObj;

        Debug.Log("Game Scene setup complete!");
    }

    private static GameObject CreatePanel(Transform parent, string name)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        panel.AddComponent<CanvasRenderer>();

        Image image = panel.AddComponent<Image>();
        image.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

        return panel;
    }

    private static void AddRolePanelUI(Transform parent, string panelName)
    {
        GameObject panel = parent.Find(panelName)?.gameObject;
        if (panel != null)
        {
            RolePanelUI component = panel.GetComponent<RolePanelUI>();
            if (component == null)
                component = panel.AddComponent<RolePanelUI>();

            // Create role cards
            CreateSubPanel(panel.transform, "PlayerRoleCard");
            CreateSubPanel(panel.transform, "AI1RoleCard");
            CreateSubPanel(panel.transform, "AI2RoleCard");
            CreateSubPanel(panel.transform, "AI3RoleCard");

            // Create texts - only if TMP is available, otherwise skip auto-assignment
            if (tmpAvailable)
            {
                CreateTextElement(panel.transform, "PlayerRoleText");
                CreateTextElement(panel.transform, "AI1RoleText");
                CreateTextElement(panel.transform, "AI2RoleText");
                CreateTextElement(panel.transform, "AI3RoleText");
                CreateTextElement(panel.transform, "PlayerNameText");
                CreateTextElement(panel.transform, "AI1NameText");
                CreateTextElement(panel.transform, "AI2NameText");
                CreateTextElement(panel.transform, "AI3NameText");
            }
            CreateButton(panel.transform, "ContinueButton");
        }
    }

    private static GameObject CreateSubPanel(Transform parent, string name)
    {
        GameObject subPanel = new GameObject(name);
        subPanel.transform.SetParent(parent, false);

        RectTransform rect = subPanel.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(150, 200);

        subPanel.AddComponent<CanvasRenderer>();
        Image image = subPanel.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.3f, 0.8f);

        return subPanel;
    }

    private static void AddPolicePickUI(Transform parent, string panelName)
    {
        GameObject panel = parent.Find(panelName)?.gameObject;
        if (panel != null)
        {
            PolicePickUI component = panel.GetComponent<PolicePickUI>();
            if (component == null)
                component = panel.AddComponent<PolicePickUI>();

            if (tmpAvailable)
            {
                CreateTextElement(panel.transform, "InstructionText");
                CreateTextElement(panel.transform, "TargetText");
                CreateTextElement(panel.transform, "Suspect1Name");
                CreateTextElement(panel.transform, "Suspect2Name");
                CreateTextElement(panel.transform, "Suspect3Name");
            }
            CreateButton(panel.transform, "Suspect1Button");
            CreateButton(panel.transform, "Suspect2Button");
            CreateButton(panel.transform, "Suspect3Button");
        }
    }

    private static void AddRevealResultUI(Transform parent, string panelName)
    {
        GameObject panel = parent.Find(panelName)?.gameObject;
        if (panel != null)
        {
            if (tmpAvailable)
            {
                CreateTextElement(panel.transform, "ResultText");
                CreateTextElement(panel.transform, "PickedPlayerText");
            }
        }
    }

    private static void AddScoreboardUI(Transform parent, string panelName)
    {
        GameObject panel = parent.Find(panelName)?.gameObject;
        if (panel != null)
        {
            ScoreboardUI component = panel.GetComponent<ScoreboardUI>();
            if (component == null)
                component = panel.AddComponent<ScoreboardUI>();

            if (tmpAvailable)
            {
                CreateTextElement(panel.transform, "RoundTitleText");
                CreateTextElement(panel.transform, "ResultText");
                CreateTextElement(panel.transform, "PickedPlayerText");
                CreateTextElement(panel.transform, "Player1NameText");
                CreateTextElement(panel.transform, "Player1RoleText");
                CreateTextElement(panel.transform, "Player1ScoreText");
                CreateTextElement(panel.transform, "Player2NameText");
                CreateTextElement(panel.transform, "Player2RoleText");
                CreateTextElement(panel.transform, "Player2ScoreText");
                CreateTextElement(panel.transform, "Player3NameText");
                CreateTextElement(panel.transform, "Player3RoleText");
                CreateTextElement(panel.transform, "Player3ScoreText");
                CreateTextElement(panel.transform, "Player4NameText");
                CreateTextElement(panel.transform, "Player4RoleText");
                CreateTextElement(panel.transform, "Player4ScoreText");
            }
            CreateButton(panel.transform, "NextRoundButton");
        }
    }

    private static void AddGameOverUI(Transform parent, string panelName)
    {
        GameObject panel = parent.Find(panelName)?.gameObject;
        if (panel != null)
        {
            GameOverUI component = panel.GetComponent<GameOverUI>();
            if (component == null)
                component = panel.AddComponent<GameOverUI>();

            if (tmpAvailable)
            {
                CreateTextElement(panel.transform, "GameOverTitleText");
                CreateTextElement(panel.transform, "WinnerText");
                CreateTextElement(panel.transform, "FirstPlaceName");
                CreateTextElement(panel.transform, "FirstPlaceScore");
                CreateTextElement(panel.transform, "SecondPlaceName");
                CreateTextElement(panel.transform, "SecondPlaceScore");
                CreateTextElement(panel.transform, "ThirdPlaceName");
                CreateTextElement(panel.transform, "ThirdPlaceScore");
                CreateTextElement(panel.transform, "FourthPlaceName");
                CreateTextElement(panel.transform, "FourthPlaceScore");
            }
            CreateButton(panel.transform, "PlayAgainButton");
            CreateButton(panel.transform, "MainMenuButton");
        }
    }

    private static void CreateHUD(Transform parent, UIManager uiManager)
    {
        GameObject hud = new GameObject("HUD");
        hud.transform.SetParent(parent, false);
        RectTransform rect = hud.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        // Only create HUD texts if TMP is available
        if (tmpAvailable)
        {
            // Round Text (top left)
            GameObject roundTextObj = CreateTextElement(hud.transform, "RoundText");
            RectTransform roundRect = roundTextObj.GetComponent<RectTransform>();
            roundRect.anchorMin = new Vector2(0, 1);
            roundRect.anchorMax = new Vector2(0, 1);
            roundRect.pivot = new Vector2(0, 1);
            roundRect.anchoredPosition = new Vector2(20, -20);

            // Target Text (top center)
            GameObject targetTextObj = CreateTextElement(hud.transform, "TargetText");
            RectTransform targetRect = targetTextObj.GetComponent<RectTransform>();
            targetRect.anchorMin = new Vector2(0.5f, 1);
            targetRect.anchorMax = new Vector2(0.5f, 1);
            targetRect.pivot = new Vector2(0.5f, 1);
            targetRect.anchoredPosition = new Vector2(0, -20);

            // Shuffle Timer Text (center)
            GameObject shuffleTextObj = CreateTextElement(hud.transform, "ShuffleTimerText");
            RectTransform shuffleRect = shuffleTextObj.GetComponent<RectTransform>();
            shuffleRect.anchorMin = new Vector2(0.5f, 0.5f);
            shuffleRect.anchorMax = new Vector2(0.5f, 0.5f);
            shuffleRect.pivot = new Vector2(0.5f, 0.5f);
            shuffleRect.anchoredPosition = Vector2.zero;

            // Assign to UIManager using reflection
            var roundTextComp = roundTextObj.GetComponent(tmpType);
            var targetTextComp = targetTextObj.GetComponent(tmpType);
            var shuffleTextComp = shuffleTextObj.GetComponent(tmpType);

            typeof(UIManager).GetField("roundText").SetValue(uiManager, roundTextComp);
            typeof(UIManager).GetField("targetText").SetValue(uiManager, targetTextComp);
            typeof(UIManager).GetField("shuffleTimerText").SetValue(uiManager, shuffleTextComp);

            // Assign result texts
            GameObject revealPanel = parent.Find("RevealResultPanel")?.gameObject;
            if (revealPanel != null)
            {
                var resultTextComp = revealPanel.transform.Find("ResultText")?.GetComponent(tmpType);
                var pickedTextComp = revealPanel.transform.Find("PickedPlayerText")?.GetComponent(tmpType);

                typeof(UIManager).GetField("resultText").SetValue(uiManager, resultTextComp);
                typeof(UIManager).GetField("pickedPlayerText").SetValue(uiManager, pickedTextComp);
            }
        }
        else
        {
            Debug.LogWarning("HUD not created - TextMeshPro required. Import via Window > TextMeshPro > Import TMP Essentials");
        }
    }

    private static void CreateStartButton(Transform canvas, UIManager uiManager)
    {
        GameObject menuPanel = canvas.Find("MainMenuPanel")?.gameObject;
        if (menuPanel != null)
        {
            GameObject startButton = CreateButton(menuPanel.transform, "StartButton");

            // Set up button click
            Button button = startButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => uiManager.OnStartGameClicked());
            }
        }
    }

    private static GameObject CreateTextElement(Transform parent, string name)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(200, 50);

        textObj.AddComponent<CanvasRenderer>();

        Component textComp = AddTMPText(textObj);

        // Set text using reflection
        if (tmpAvailable)
        {
            var textProp = textComp.GetType().GetProperty("text");
            var fontSizeProp = textComp.GetType().GetProperty("fontSize");
            var colorProp = textComp.GetType().GetProperty("color");

            if (textProp != null) textProp.SetValue(textComp, name, null);
            if (fontSizeProp != null) fontSizeProp.SetValue(textComp, 18f, null);
            if (colorProp != null) colorProp.SetValue(textComp, Color.white, null);
        }

        return textObj;
    }

    private static GameObject CreateButton(Transform parent, string name)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(160, 40);

        buttonObj.AddComponent<CanvasRenderer>();
        Image image = buttonObj.AddComponent<Image>();
        image.color = new Color(0.2f, 0.6f, 1f);

        Button button = buttonObj.AddComponent<Button>();

        // Create button text child
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        textObj.AddComponent<CanvasRenderer>();

        Component textComp = AddTMPText(textObj);

        if (tmpAvailable)
        {
            var textProp = textComp.GetType().GetProperty("text");
            var fontSizeProp = textComp.GetType().GetProperty("fontSize");
            var colorProp = textComp.GetType().GetProperty("color");
            var alignmentProp = textComp.GetType().GetProperty("alignment");

            if (textProp != null) textProp.SetValue(textComp, name, null);
            if (fontSizeProp != null) fontSizeProp.SetValue(textComp, 16f, null);
            if (colorProp != null) colorProp.SetValue(textComp, Color.white, null);

            // Set alignment to center
            if (alignmentProp != null && alignmentProp.PropertyType.IsEnum)
            {
                alignmentProp.SetValue(textComp, Enum.Parse(alignmentProp.PropertyType, "Center"), null);
            }
        }

        return buttonObj;
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

    [MenuItem("ChorPolice/Check TextMeshPro Status")]
    public static void CheckTextMeshProStatus()
    {
        CheckTMPAvailability();
        if (tmpAvailable)
        {
            EditorUtility.DisplayDialog("TextMeshPro Status", "TextMeshPro is AVAILABLE", "OK");
        }
        else
        {
            bool result = EditorUtility.DisplayDialog("TextMeshPro Status",
                "TextMeshPro NOT FOUND!\n\nImport it via:\nWindow > TextMeshPro > Import TMP Essentials",
                "Import Now", "Cancel");

            if (result)
            {
                // Try to open the TextMeshPro import window
                typeof(TextEditor).GetMethod("OpenProject", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)?.Invoke(null, new object[] { "Project/TextMeshPro" });
            }
        }
    }
}
