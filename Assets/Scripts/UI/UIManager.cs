// UIManager.cs
// Author: Sadikur Rahman
// Description: Controls the UI Toolkit updates for roles, timers, results, and scores.

using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDocument uiDocument;

    [Header("UXML Assets")]
    public VisualTreeAsset mainMenuAsset;
    public VisualTreeAsset gameHUDAsset;
    public VisualTreeAsset roleRevealAsset;
    public VisualTreeAsset policePickAsset;
    public VisualTreeAsset revealResultAsset;
    public VisualTreeAsset scoreboardAsset;
    public VisualTreeAsset gameOverAsset;

    private VisualElement _root;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        if (uiDocument != null)
            _root = uiDocument.rootVisualElement;
    }

    private void Start()
    {
        // Subscribe to state changes
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged += HandleStateChange;
            GameStateManager.Instance.OnRevealResult += HandleRevealResultEvent;
        }

        HandleStateChange(GameState.Menu);
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= HandleStateChange;
            GameStateManager.Instance.OnRevealResult -= HandleRevealResultEvent;
        }
    }

    private void HandleStateChange(GameState newState)
    {
        if (_root == null) return;

        _root.Clear();

        switch (newState)
        {
            case GameState.Menu:
                LoadView(mainMenuAsset);
                SetupMainMenu();
                break;
            case GameState.Shuffling:
                LoadView(roleRevealAsset);
                SetupShufflingView();
                break;
            case GameState.RoleReveal:
                LoadView(roleRevealAsset);
                SetupRoleReveal();
                break;
            case GameState.PolicePick:
                LoadView(gameHUDAsset);
                LoadOverlay(policePickAsset);
                SetupGameHUD();
                SetupPolicePick();
                break;
            case GameState.RevealResult:
                LoadView(gameHUDAsset);
                LoadOverlay(revealResultAsset);
                SetupGameHUD();
                // Actual result populated via HandleRevealResultEvent
                break;
            case GameState.Scorecard:
                LoadView(scoreboardAsset);
                SetupScoreboard();
                break;
            case GameState.GameOver:
                LoadView(gameOverAsset);
                SetupGameOver();
                break;
        }
    }

    private void LoadView(VisualTreeAsset asset)
    {
        if (asset != null)
        {
            _root.Clear();
            asset.CloneTree(_root);
            
            // Ensure the root children fill the space if they aren't absolute
            foreach (var child in _root.Children())
            {
                child.style.flexGrow = 1;
            }
        }
    }
    
    private void LoadOverlay(VisualTreeAsset asset)
    {
        if (asset != null)
        {
            var overlay = asset.CloneTree();
            overlay.style.position = Position.Absolute;
            overlay.style.left = 0; overlay.style.right = 0;
            overlay.style.top = 0; overlay.style.bottom = 0;
            
            // Crucial: Allow clicks to pass through the overlay container
            overlay.pickingMode = PickingMode.Ignore;
            // Also ensure all children of the overlay don't block by default unless they are buttons/labels
            foreach (var child in overlay.Children())
            {
                // We want the actual UI elements (like buttons) to still be clickable
                // but the containers themselves should be transparent to clicks
                if (child.name == "" || child is VisualElement && !(child is Button || child is Label))
                {
                    child.pickingMode = PickingMode.Ignore;
                }
            }
            
            _root.Add(overlay);
        }
    }

    #region View Setups

    private void SetupMainMenu()
    {
        var startBtn = _root.Q<Button>("btn-singleplayer");
        if (startBtn != null)
        {
            startBtn.clicked += () => {
                Debug.Log("UIManager: Start Button Clicked");
                GameStateManager.Instance.StartGame();
            };
        }
            
        var quitBtn = _root.Q<Button>("btn-quit");
        if (quitBtn != null)
            quitBtn.clicked += () => Application.Quit();
    }

    private void SetupShufflingView()
    {
        var header = _root.Q<Label>();
        if (header != null) header.text = "Shuffling Roles...";
        
        var roleLbl = _root.Q<Label>("lbl-assigned-role");
        if (roleLbl != null) roleLbl.text = "???";
        
        var iconLbl = _root.Q<Label>("icon-assigned-role");
        if (iconLbl != null) iconLbl.text = "🎲";
        
        var continueBtn = _root.Q<Button>("btn-continue");
        if (continueBtn != null) continueBtn.style.display = DisplayStyle.None;
    }

    private void SetupRoleReveal()
    {
        var player = GameManager.Instance.HumanPlayer;
        if (player == null)
        {
            Debug.LogError("UIManager: Human Player is NULL in SetupRoleReveal!");
            return;
        }

        Debug.Log($"UIManager: Showing Role Reveal for {player.Name} - Role: {player.Role}");

        var roleLbl = _root.Q<Label>("lbl-assigned-role");
        if (roleLbl != null) roleLbl.text = "YOU ARE: " + player.Role.ToString();
        
        var iconLbl = _root.Q<Label>("icon-assigned-role");
        if (iconLbl != null) iconLbl.text = GetRoleEmoji(player.Role);
        
        var continueBtn = _root.Q<Button>("btn-continue");
        if (continueBtn != null)
        {
            continueBtn.style.display = DisplayStyle.Flex;
            continueBtn.clicked += () => {
                Debug.Log("UIManager: Role Reveal Continue Button Clicked");
                GameStateManager.Instance.OnRoleRevealComplete();
            };
        }
        else
        {
            Debug.LogError("UIManager: Continue Button NOT FOUND in RoleRevealUI!");
        }
    }

    private void SetupGameHUD()
    {
        if (GameManager.Instance == null) return;

        var targetLbl = _root.Q<Label>("lbl-current-target");
        if (targetLbl != null)
        {
            RoleType target = GameManager.Instance.CurrentTarget;
            targetLbl.text = $"Target: {target}";
            
            // Apply role color class
            targetLbl.RemoveFromClassList("role-chor");
            targetLbl.RemoveFromClassList("role-dakat");
            targetLbl.AddToClassList(target == RoleType.CHOR ? "role-chor" : "role-dakat");
        }
        
        var roundLbl = _root.Q<Label>("lbl-round-counter");
        if (roundLbl != null)
        {
            roundLbl.text = $"Round {GameManager.Instance.CurrentRound} / {GameManager.Instance.TotalRounds}";
        }

        // Sync player cards
        var players = GameManager.Instance.Players;
        for (int i = 0; i < players.Count; i++)
        {
            var card = _root.Q<VisualElement>($"card-{i}");
            if (card == null) continue;

            var nameLbl = card.Q<Label>($"lbl-name-{i}");
            if (nameLbl != null) nameLbl.text = players[i].Name;

            var iconLbl = card.Q<Label>($"icon-role-{i}");
            if (iconLbl != null)
            {
                // Only show role if it's the human player or we are in reveal result state
                if (!players[i].IsAI || GameStateManager.Instance.CurrentState == GameState.RevealResult)
                {
                    iconLbl.text = GetRoleEmoji(players[i].Role);
                    iconLbl.RemoveFromClassList("role-police");
                    iconLbl.RemoveFromClassList("role-chor");
                    iconLbl.RemoveFromClassList("role-babu");
                    iconLbl.RemoveFromClassList("role-dakat");
                    iconLbl.AddToClassList("role-" + players[i].Role.ToString().ToLower());
                }
                else
                {
                    iconLbl.text = "?";
                }
            }
        }
    }

    private void SetupPolicePick()
    {
        var police = GameManager.Instance.PolicePlayer;
        if (police == null) return;

        bool isHumanPolice = !police.IsAI;
        var promptLbl = _root.Q<Label>("lbl-prompt");
        var confirmBtn = _root.Q<Button>("btn-confirm-arrest");

        if (confirmBtn != null) confirmBtn.SetEnabled(false);

        if (isHumanPolice)
        {
            if (promptLbl != null) promptLbl.text = $"You are the Police! Catch the {GameManager.Instance.CurrentTarget}!";
            
            // Make other cards selectable
            PlayerData selectedPlayer = null;
            var players = GameManager.Instance.Players;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Role == RoleType.POLICE) continue;

                var card = _root.Q<VisualElement>($"card-{i}");
                if (card == null) continue;

                card.AddToClassList("player-card--selectable");
                
                int index = i;
                card.RegisterCallback<ClickEvent>(evt => {
                    // Deselect others
                    _root.Query<VisualElement>(className: "player-card--selected").ForEach(el => el.RemoveFromClassList("player-card--selected"));
                    
                    // Select this
                    card.AddToClassList("player-card--selected");
                    selectedPlayer = players[index];
                    if (confirmBtn != null) confirmBtn.SetEnabled(true);
                });
            }

            if (confirmBtn != null)
            {
                confirmBtn.clicked += () => {
                    if (selectedPlayer != null)
                        GameStateManager.Instance.OnPolicePickComplete(selectedPlayer);
                };
            }
        }
        else
        {
            if (promptLbl != null) promptLbl.text = "Police is thinking...";
            if (confirmBtn != null) confirmBtn.style.display = DisplayStyle.None;
        }
    }

    private void SetupScoreboard()
    {
        var container = _root.Q<VisualElement>("score-rows-container");
        if (container == null) return;

        container.Clear();
        var players = GameManager.Instance.Players;

        foreach (var p in players)
        {
            var row = new VisualElement();
            row.AddToClassList("scoreboard-row");

            var nameCell = new Label(p.Name); nameCell.AddToClassList("scoreboard-cell");
            var roleCell = new Label(p.Role.ToString()); roleCell.AddToClassList("scoreboard-cell");
            
            // For points this round, we'd ideally have that stored. For now let's just show total.
            var pointsCell = new Label("-"); pointsCell.AddToClassList("scoreboard-cell");
            var totalCell = new Label(p.Score.ToString()); totalCell.AddToClassList("scoreboard-cell");

            row.Add(nameCell);
            row.Add(roleCell);
            row.Add(pointsCell);
            row.Add(totalCell);
            container.Add(row);
        }

        var nextBtn = _root.Q<Button>("btn-next-round");
        if (nextBtn != null)
            nextBtn.clicked += () => GameStateManager.Instance.OnScorecardComplete();
    }

    private void SetupGameOver()
    {
        var leaderboard = GameManager.Instance.GetLeaderboard();
        var winner = leaderboard.FirstOrDefault();

        var winnerLbl = _root.Q<Label>("lbl-winner");
        if (winnerLbl != null && winner != null) winnerLbl.text = $"{winner.Name} Wins!";

        var container = _root.Q<VisualElement>("final-score-rows-container");
        if (container != null)
        {
            container.Clear();
            for (int i = 0; i < leaderboard.Count; i++)
            {
                var p = leaderboard[i];
                var row = new VisualElement();
                row.AddToClassList("scoreboard-row");

                var rankCell = new Label($"#{i + 1}"); rankCell.AddToClassList("scoreboard-cell"); rankCell.style.width = Length.Percent(15);
                var nameCell = new Label(p.Name); nameCell.AddToClassList("scoreboard-cell"); nameCell.style.width = Length.Percent(40);
                var scoreCell = new Label(p.Score.ToString()); scoreCell.AddToClassList("scoreboard-cell"); scoreCell.style.width = Length.Percent(45); scoreCell.style.unityTextAlign = TextAnchor.MiddleRight;

                row.Add(rankCell);
                row.Add(nameCell);
                row.Add(scoreCell);
                container.Add(row);
            }
        }

        var playAgainBtn = _root.Q<Button>("btn-play-again");
        if (playAgainBtn != null)
            playAgainBtn.clicked += () => GameStateManager.Instance.StartGame();

        var mainMenuBtn = _root.Q<Button>("btn-main-menu");
        if (mainMenuBtn != null)
            mainMenuBtn.clicked += () => GameStateManager.Instance.ReturnToMenu();
    }

    #endregion

    private void HandleRevealResultEvent(bool didWin, PlayerData pickedTarget, RoleType target)
    {
        // Update Result Overlay UI
        var statusLbl = _root.Q<Label>("lbl-result-status");
        if (statusLbl != null)
        {
            statusLbl.text = didWin ? $"Police Caught the {target}!" : $"Wrong! Innocent {pickedTarget.Role} was arrested!";
        }

        var nameLbl = _root.Q<Label>("lbl-revealed-name");
        if (nameLbl != null) nameLbl.text = pickedTarget.Name;

        var iconLbl = _root.Q<Label>("icon-revealed-role");
        if (iconLbl != null) iconLbl.text = GetRoleEmoji(pickedTarget.Role);

        var roleNameLbl = _root.Q<Label>("lbl-revealed-role");
        if (roleNameLbl != null) roleNameLbl.text = pickedTarget.Role.ToString();

        // Reveal all roles on HUD
        SetupGameHUD();
    }

    private string GetRoleEmoji(RoleType role)
    {
        switch (role)
        {
            case RoleType.POLICE: return "👮";
            case RoleType.CHOR: return "🕵️";
            case RoleType.BABU: return "💼";
            case RoleType.DAKAT: return "🧨";
            default: return "?";
        }
    }
}