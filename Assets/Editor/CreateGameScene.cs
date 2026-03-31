// CreateGameScene.cs
// Author: Sadikur Rahman
// Description: Generates complete GameScene.unity with all gameplay UI.

using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreateGameScene
{
    [MenuItem("ChorPolice/Create GameScene")]
    public static void CreateGameSceneMenu()
    {
        Debug.Log("Creating Chor Police GameScene...");

        // Create new scene
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "GameScene";

        // Create main camera
        GameObject cameraObj = new GameObject("Main Camera");
        Camera camera = cameraObj.AddComponent<Camera>();
        camera.backgroundColor = new Color(0.08f, 0.08f, 0.12f);
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.cullingMask = LayerMask.GetMask("UI", "Default");

        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        canvasObj.AddComponent<GraphicRaycaster>();

        // Create EventSystem
        GameObject eventSystemObj = new GameObject("EventSystem");
        eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Create Lighting
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = Color.white;
        light.intensity = 1f;
        lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Create Managers GameObject
        GameObject managersObj = new GameObject("Managers");
        managersObj.transform.SetParent(null);

        // Create GameManager
        GameObject gameManagerObj = new GameObject("GameManager");
        gameManagerObj.transform.SetParent(managersObj.transform);
        GameManager gameManager = gameManagerObj.AddComponent<GameManager>();

        // Create RoleManager
        GameObject roleManagerObj = new GameObject("RoleManager");
        roleManagerObj.transform.SetParent(managersObj.transform);
        roleManagerObj.AddComponent<RoleManager>();

        // Create RoundManager
        GameObject roundManagerObj = new GameObject("RoundManager");
        roundManagerObj.transform.SetParent(managersObj.transform);
        RoundManager roundManager = roundManagerObj.AddComponent<RoundManager>();

        // Create ScoreManager
        GameObject scoreManagerObj = new GameObject("ScoreManager");
        scoreManagerObj.transform.SetParent(managersObj.transform);
        ScoreManager scoreManager = scoreManagerObj.AddComponent<ScoreManager>();

        // Create GameStateManager
        GameObject gameStateManagerObj = new GameObject("GameStateManager");
        gameStateManagerObj.transform.SetParent(managersObj.transform);
        GameStateManager gameStateManager = gameStateManagerObj.AddComponent<GameStateManager>();

        // Create AIManager
        GameObject aiManagerObj = new GameObject("AIManager");
        aiManagerObj.transform.SetParent(managersObj.transform);
        aiManagerObj.AddComponent<AIManager>();

        // Create UIManager_TMP
        GameObject uiManagerObj = new GameObject("UIManager");
        uiManagerObj.transform.SetParent(managersObj.transform);
        UIManager_TMP uiManager = uiManagerObj.AddComponent<UIManager_TMP>();

        // Create GameInitializer
        GameObject gameInitializerObj = new GameObject("GameInitializer");
        gameInitializerObj.transform.SetParent(managersObj.transform);
        GameInitializer gameInitializer = gameInitializerObj.AddComponent<GameInitializer>();

        // Initialize arrays
        gameInitializer.gameManager = gameManager;
        gameInitializer.roleManager = roleManagerObj.GetComponent<RoleManager>();
        gameInitializer.roundManager = roundManager;
        gameInitializer.scoreManager = scoreManager;
        gameInitializer.gameStateManager = gameStateManager;
        gameInitializer.initOrder = new BaseManager[] { gameManager, scoreManager, roundManager };

        // Create UI
        CreateCompleteUI(canvas, uiManager);

        // Save scene
        string scenePath = "Assets/Scenes/GameScene.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        AssetDatabase.Refresh();

        // Set as active scene
        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        Debug.Log("GameScene created successfully!");
        EditorUtility.DisplayDialog("Scene Created", "GameScene has been created with complete gameplay UI!\n\nClick Play to start playing Chor Police!", "OK");
    }

    private static void CreateCompleteUI(GameObject canvas, UIManager_TMP uiManager)
    {
        // Colors
        Color mainMenuBg = new Color(0.08f, 0.08f, 0.12f, 0.95f);
        Color panelBg = new Color(0.1f, 0.1f, 0.15f, 0.9f);
        Color buttonNormal = new Color(0.2f, 0.5f, 0.8f);
        Color buttonPressed = new Color(0.1f, 0.4f, 0.7f);
        Color policeColor = new Color(0.2f, 0.4f, 1f);
        Color chorColor = new Color(1f, 0.8f, 0f);
        Color dakatColor = new Color(0.8f, 0.2f, 0.2f);
        Color babuColor = new Color(0.3f, 0.8f, 0.3f);
        Color green = new Color(0.2f, 0.8f, 0.2f);
        Color red = new Color(0.8f, 0.2f, 0.2f);
        Color yellow = new Color(1f, 0.85f, 0f);
        Color white = Color.white;

        // Create Main Menu Panel
        GameObject mainMenuPanel = CreatePanel(canvas, "MainMenuPanel", mainMenuBg);
        CreateMainMenuUI(mainMenuPanel, buttonNormal);
        uiManager.mainMenuPanel = mainMenuPanel;
        uiManager.startButton = mainMenuPanel.GetComponentInChildren<Button>()[0];

        // Create Shuffle Panel
        GameObject shufflePanel = CreatePanel(canvas, "ShufflePanel", panelBg);
        CreateShuffleUI(shufflePanel);
        uiManager.shufflePanel = shufflePanel;
        uiManager.shuffleTimerText = shufflePanel.GetComponentInChildren<TextMeshProUGUI>()[0];

        // Create Role Reveal Panel
        GameObject roleRevealPanel = CreatePanel(canvas, "RoleRevealPanel", panelBg);
        CreateRoleRevealUI(roleRevealPanel, policeColor, chorColor, dakatColor, babuColor);
        uiManager.roleRevealPanel = roleRevealPanel;

        // Create Police Pick Panel
        GameObject policePickPanel = CreatePanel(canvas, "PolicePickPanel", panelBg);
        CreatePolicePickUI(policePickPanel, buttonNormal, chorColor, dakatColor);
        uiManager.policePickPanel = policePickPanel;
        uiManager.instructionText = policePickPanel.GetComponentInChildren<TextMeshProUGUI>()[0];
        uiManager.targetText = policePickPanel.GetComponentInChildren<TextMeshProUGUI>()[1];
        uiManager.suspect1Button = policePickPanel.GetComponentsInChildren<Button>()[0];
        uiManager.suspect2Button = policePickPanel.GetComponentsInChildren<Button>()[1];
        uiManager.suspect3Button = policePickPanel.GetComponentsInChildren<Button>()[2];
        uiManager.suspect1Name = policePickPanel.GetComponentsInChildren<TextMeshProUGUI>()[2];
        uiManager.suspect2Name = policePickPanel.GetComponentsInChildren<TextMeshProUGUI>()[3];
        uiManager.suspect3Name = policePickPanel.GetComponentsInChildren<TextMeshProUGUI>()[4];

        // Create Reveal Result Panel
        GameObject revealResultPanel = CreatePanel(canvas, "RevealResultPanel", panelBg);
        CreateRevealResultUI(revealResultPanel, green, red);
        uiManager.revealResultPanel = revealResultPanel;
        uiManager.resultText = revealResultPanel.GetComponentInChildren<TextMeshProUGUI>()[0];
        uiManager.pickedPlayerText = revealResultPanel.GetComponentInChildren<TextMeshProUGUI>()[1];

        // Create Scorecard Panel
        GameObject scorecardPanel = CreatePanel(canvas, "ScorecardPanel", panelBg);
        CreateScorecardUI(scorecardPanel, policeColor, chorColor, dakatColor, babuColor);
        uiManager.scorecardPanel = scorecardPanel;
        uiManager.roundTitleText = scorecardPanel.GetComponentInChildren<TextMeshProUGUI>()[0];
        uiManager.scorecardResultText = scorecardPanel.GetComponentInChildren<TextMeshProUGUI>()[1];
        uiManager.scorecardPickedPlayerText = scorecardPanel.GetComponentInChildren<TextMeshProUGUI>()[2];
        uiManager.player1NameText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[3];
        uiManager.player1RoleText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[4];
        uiManager.player1ScoreText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[5];
        uiManager.player2NameText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[6];
        uiManager.player2RoleText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[7];
        uiManager.player2ScoreText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[8];
        uiManager.player3NameText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[9];
        uiManager.player3RoleText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[10];
        uiManager.player3ScoreText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[11];
        uiManager.player4NameText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[12];
        uiManager.player4RoleText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[13];
        uiManager.player4ScoreText = scorecardPanel.GetComponentsInChildren<TextMeshProUGUI>()[14];
        uiManager.nextRoundButton = scorecardPanel.GetComponentsInChildren<Button>()[0];

        // Create Game Over Panel
        GameObject gameOverPanel = CreatePanel(canvas, "GameOverPanel", mainMenuBg);
        CreateGameOverUI(gameOverPanel, yellow, white, policeColor, chorColor, dakatColor, babuColor);
        uiManager.gameOverPanel = gameOverPanel;
        uiManager.winnerText = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>()[0];
        uiManager.firstPlaceName = gameOverPanel.GetComponentsInChildren<TextMeshProUGUI>()[1];
        uiManager.firstPlaceScore = gameOverPanel.GetComponentsInChildren<TextMeshProUGUI>()[2];
        uiManager.secondPlaceName = gameOverPanel.GetComponentsInChildren<TextMeshProUGUI>()[3];
        uiManager.secondPlaceScore = gameOverPanel.GetComponentsInChildren<TextMeshProUGUI>()[4];
        uiManager.thirdPlaceName = gameOverPanel.GetComponentsInChildren<TextMeshProUGUI>()[5];
        uiManager.thirdPlaceScore = gameOverPanel.GetComponentsInChildren<TextMeshProUGUI>()[6];
        uiManager.fourthPlaceName = gameOverPanel.GetComponentsInChildren<TextMeshProUGUI>()[7];
        uiManager.fourthPlaceScore = gameOverPanel.GetComponentsInChildren<TextMeshProUGUI>()[8];
        uiManager.playAgainButton = gameOverPanel.GetComponentsInChildren<Button>()[0];
        uiManager.mainMenuButton = gameOverPanel.GetComponentsInChildren<Button>()[1];

        // Create HUD
        GameObject hudPanel = CreatePanel(canvas, "HUDPanel", new Color(0, 0, 0, 0));
        CreateHUDUI(hudPanel);
        uiManager.hudPanel = hudPanel;
        uiManager.roundText = hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[0];
        uiManager.hudTargetText = hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    private static GameObject CreatePanel(GameObject canvas, string name, Color bgColor)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(canvas.transform, false);

        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = new Vector2(1920, 1080);

        Image image = panel.AddComponent<Image>();
        image.color = bgColor;
        image.raycastTarget = GraphicRaycaster.RaycastTarget.None;
        return panel;
    }

    private static TextMeshProUGUI CreateText(GameObject parent, string text, int fontSize, Color color, Vector2 anchoredPosition, TextAlignmentOptions alignment = TextAlignmentOptions.Center, Vector2 sizeDelta = new Vector2(200, 50))
    {
        GameObject textObj = new GameObject(text);
        textObj.transform.SetParent(parent.transform, false);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = alignment;
        tmp.fontStyle = FontStyles.Bold;
        tmp.overflow = TextOverflowModes.Overflow;
        tmp.font = TMP_FontAsset.defaultFontAsset;
        return tmp;
    }

    private static Button CreateButton(GameObject parent, string name, string text, Color normalColor, Color pressedColor, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent.transform, false);

        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;

        Image image = buttonObj.AddComponent<Image>();
        image.color = normalColor;
        image.type = Image.Type.Sliced;
        image.raycastTarget = GraphicRaycaster.RaycastTarget.None;

        Button button = buttonObj.AddComponent<Button>();

        // Create text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 24;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        tmp.font = TMP_FontAsset.defaultFontAsset;

        ColorBlock colors = button.colors;
        colors.normalColor = normalColor;
        colors.highlightedColor = new Color(normalColor.r + 0.2f, normalColor.g + 0.2f, normalColor.b + 0.2f);
        colors.pressedColor = pressedColor;
        colors.selectedColor = new Color(normalColor.r + 0.3f, normalColor.g + 0.3f, normalColor.b + 0.3f);
        button.colors = colors;

        return button;
    }

    private static GameObject CreateCard(GameObject parent, string name, Color bgColor, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        GameObject card = new GameObject(name);
        card.transform.SetParent(parent.transform, false);

        RectTransform rect = card.AddComponent<RectTransform>();
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;

        Image image = card.AddComponent<Image>();
        image.color = bgColor;

        return card;
    }

    private static void CreateMainMenuUI(GameObject panel, Color buttonColor)
    {
        // Title
        CreateText(panel, "🎮 CHOR POLICE", 80, new Color(1f, 0.85f, 0f), new Vector2(0, 300), new Vector2(1600, 100));

        // Subtitle
        CreateText(panel, "A Game of Deception", 40, new Color(0.7f, 0.7f, 0.8f), new Vector2(0, 200), new Vector2(1600, 60));

        // Start Button
        CreateButton(panel, "StartButton", "▶ START GAME", new Color(0.2f, 0.6f, 0.3f), new Color(0.15f, 0.4f, 0.2f), new Vector2(0, 0), new Vector2(320, 80));

        // Instructions
        CreateText(panel, "Human Player vs 3 AI Players\n10 Rounds of Social Deduction", 24, new Color(0.5f, 0.5f, 0.6f), new Vector2(0, -150), new Vector2(1600, 40));
    }

    private static void CreateShuffleUI(GameObject panel)
    {
        // Title
        CreateText(panel, "🎲 SHUFFLING ROLES...", 60, Color.yellow, new Vector2(0, 200), new Vector2(1600, 80));

        // Timer
        CreateText(panel, "2.0s", 50, Color.white, new Vector2(0, -50), new Vector2(1600, 70));

        // Card placeholders
        CreateCard(panel, "Card1", new Color(0.3f, 0.3f, 0.35f), new Vector2(-300, 0), new Vector2(200, 280));
        CreateCard(panel, "Card2", new Color(0.3f, 0.3f, 0.35f), new Vector2(-100, 0), new Vector2(200, 280));
        CreateCard(panel, "Card3", new Color(0.3f, 0.3f, 0.35f), new Vector2(100, 0), new Vector2(200, 280));
        CreateCard(panel, "Card4", new Color(0.3f, 0.3f, 0.35f), new Vector2(300, 0), new Vector2(200, 280));
    }

    private static void CreateRoleRevealUI(GameObject panel, Color policeColor, Color chorColor, Color dakatColor, Color babuColor)
    {
        // Title
        CreateText(panel, "📜 ROLES REVEALED", 50, Color.cyan, new Vector2(0, 280), new Vector2(1600, 70));

        // Role Cards
        CreateRoleCard(panel, "PlayerRoleCard", "Player", policeColor, new Vector2(-450, 50), new Vector2(200, 250));
        CreateRoleCard(panel, "AI1RoleCard", "AI 1", chorColor, new Vector2(-150, 50), new Vector2(200, 250));
        CreateRoleCard(panel, "AI2RoleCard", "AI 2", babuColor, new Vector2(150, 50), new Vector2(200, 250));
        CreateRoleCard(panel, "AI3RoleCard", "AI 3", dakatColor, new Vector2(450, 50), new Vector2(200, 250));

        // Continue Button
        CreateButton(panel, "ContinueButton", "CONTINUE ▶", new Color(0.2f, 0.5f, 0.8f), new Color(0.15f, 0.35f, 0.6f), new Vector2(0, -220), new Vector2(280, 70));
    }

    private static void CreateRoleCard(GameObject parent, string cardName, string playerName, Color bgColor, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        GameObject card = CreateCard(parent, cardName + "Card", bgColor, anchoredPosition, sizeDelta);

        // Player Name
        CreateText(card, playerName, 28, Color.white, new Vector2(0, 80), new Vector2(180, 50));

        // Role Text
        CreateText(card, "???", 40, new Color(0.1f, 0.1f, 0.1f), new Vector2(0, -30), new Vector2(180, 70));
    }

    private static void CreatePolicePickUI(GameObject panel, Color buttonNormal, Color chorColor, Color dakatColor)
    {
        // Instruction
        CreateText(panel, "You are the POLICE!\nSelect who you think is the CHOR.", 32, Color.white, new Vector2(0, 280), new Vector2(1600, 60));

        // Target
        CreateText(panel, "Find the CHOR!", 40, chorColor, new Vector2(0, 150), new Vector2(1600, 70));

        // Suspect Buttons
        Vector2 buttonSize = new Vector2(220, 280);
        Vector2 cardSpacing = new Vector2(300, 0);

        GameObject button1 = CreateButton(panel, "Suspect1Button", "", buttonNormal, new Color(0.25f, 0.25f, 0.3f), new Vector2(-cardSpacing.x, 0), buttonSize);
        GameObject button2 = CreateButton(panel, "Suspect2Button", "", buttonNormal, new Color(0.25f, 0.25f, 0.3f), new Vector2(0, 0), buttonSize);
        GameObject button3 = CreateButton(panel, "Suspect3Button", "", buttonNormal, new Color(0.25f, 0.25f, 0.3f), new Vector2(cardSpacing.x, 0), buttonSize);

        // Suspect Names
        CreateText(button1, "AI 1", 28, Color.white, new Vector2(0, -100), new Vector2(220, 50));
        CreateText(button2, "AI 2", 28, Color.white, new Vector2(0, -100), new Vector2(220, 50));
        CreateText(button3, "AI 3", 28, Color.white, new Vector2(0, -100), new Vector2(220, 50));
    }

    private static void CreateRevealResultUI(GameObject panel, Color green, Color red)
    {
        // Result Title
        string resultText = "🎉 Police Caught the CHOR!";
        Color resultColor = green;
        CreateText(panel, resultText, 50, resultColor, new Vector2(0, 100), new Vector2(1600, 70));

        // Picked Player
        CreateText(panel, "Picked: AI 1 (CHOR)", 36, Color.white, new Vector2(0, 0), new Vector2(1600, 60));
    }

    private static void CreateScorecardUI(GameObject panel, Color policeColor, Color chorColor, Color dakatColor, Color babuColor)
    {
        // Round Title
        CreateText(panel, "Round 1 Results", 50, Color.cyan, new Vector2(0, 280), new Vector2(1600, 70));

        // Result
        string resultText = "🎉 Police Caught the Target!";
        CreateText(panel, resultText, 36, new Color(0.2f, 0.8f, 0.2f), new Vector2(0, 200), new Vector2(1600, 50));

        // Picked Player
        CreateText(panel, "Picked: AI 1 (CHOR)", 28, Color.white, new Vector2(0, 120), new Vector2(1600, 45));

        // Player Scores
        CreateScoreRow(panel, "Player1Row", "Player", "POLICE", "800", policeColor, new Vector2(0, 50), new Vector2(1600, 35));
        CreateScoreRow(panel, "Player2Row", "AI 1", "CHOR", "700", chorColor, new Vector2(0, -30), new Vector2(1600, 35));
        CreateScoreRow(panel, "Player3Row", "AI 2", "BABU", "1000", babuColor, new Vector2(0, -110), new Vector2(1600, 35));
        CreateScoreRow(panel, "Player4Row", "AI 3", "DAKAT", "600", dakatColor, new Vector2(0, -190), new Vector2(1600, 35));

        // Next Round Button
        CreateButton(panel, "NextRoundButton", "NEXT ROUND ▶", new Color(0.15f, 0.6f, 0.3f), new Color(0.1f, 0.5f, 0.2f), new Vector2(0, -200), new Vector2(280, 70));
    }

    private static void CreateScoreRow(GameObject parent, string rowName, string playerName, string role, string score, Color roleColor, Vector2 anchoredPosition, Vector2 nameSizeDelta)
    {
        // Background
        GameObject bg = CreatePanel(parent, rowName + "Bg", new Color(0.15f, 0.15f, 0.2f, 0.9f), anchoredPosition, new Vector2(1600, nameSizeDelta));

        // Name
        CreateText(bg, playerName, 24, Color.white, new Vector2(0, 10), new Vector2(180, 30));

        // Role
        CreateText(bg, role, 24, roleColor, new Vector2(400, 10), new Vector2(180, 30));

        // Score
        CreateText(bg, score, 24, Color.yellow, new Vector2(-100, 10), new Vector2(180, 30));
    }

    private static void CreateGameOverUI(GameObject panel, Color yellow, Color white, Color policeColor, Color chorColor, Color dakatColor, Color babuColor)
    {
        // Title
        CreateText(panel, "🏆 GAME OVER 🏆", 80, yellow, new Vector2(0, 280), new Vector2(1600, 80));

        // Winner
        CreateText(panel, "Player Wins!", 50, yellow, new Vector2(0, 180), new Vector2(1600, 70));

        // Leaderboard Title
        CreateText(panel, "─── LEADERBOARD ───", 36, Color.cyan, new Vector2(0, 100), new Vector2(1600, 50));

        // Leaderboard Rows
        CreateLeaderboardRow(panel, "FirstPlaceRow", "1. Player", "10000", yellow, new Vector2(0, 40), new Vector2(1600, 30));
        CreateLeaderboardRow(panel, "SecondPlaceRow", "2. AI 1", "8000", new Color(0.75f, 0.75f, 0.75f), new Vector2(0, 0), new Vector2(1600, 30));
        CreateLeaderboardRow(panel, "ThirdPlaceRow", "3. AI 2", "6000", new Color(0.64f, 0.4f, 0.2f), new Vector2(0, -40), new Vector2(1600, 30));
        CreateLeaderboardRow(panel, "FourthPlaceRow", "4. AI 3", "4000", Color.white, new Vector2(0, -80), new Vector2(1600, 30));

        // Buttons
        CreateButton(panel, "PlayAgainButton", "▶ PLAY AGAIN", new Color(0.15f, 0.6f, 0.3f), new Color(0.1f, 0.5f, 0.2f), new Color(0.15f, 0.35f, 0.6f), new Vector2(-160, -200), new Vector2(280, 70));
        CreateButton(panel, "MainMenuButton", "◀ MAIN MENU", new Color(0.6f, 0.2f, 0.2f), new Color(0.5f, 0.15f, 0.15f), new Color(0.4f, 0.3f, 0.6f), new Vector2(160, -200), new Vector2(280, 70));
    }

    private static void CreateLeaderboardRow(GameObject parent, string rowName, string text, string score, Color placeColor, Vector2 anchoredPosition, Vector2 textOffset)
    {
        // Text
        CreateText(parent, text, 28, placeColor, anchoredPosition + textOffset, new Vector2(1600, 30));

        // Score
        CreateText(parent, score, 28, placeColor, new Vector2(-100, 0) + textOffset, new Vector2(1600, 30));
    }

    private static void CreateHUDUI(GameObject panel)
    {
        // Round Counter
        CreateText(panel, "Round: 1/10", 24, Color.white, new Vector2(-400, 480), new Vector2(200, 30), TextAlignmentOptions.MiddleLeft);

        // Target Indicator
        CreateText(panel, "Find the CHOR!", 32, Color.yellow, new Vector2(0, 480), new Vector2(1600, 30), TextAlignmentOptions.MiddleCenter);
    }
}
