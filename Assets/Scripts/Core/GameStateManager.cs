// GameStateManager.cs
// Author: Sadikur Rahman
// Description: State machine controller for managing game flow phases.

using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private GameState _currentState;
    public GameState CurrentState => _currentState;

    // Events for UI to subscribe to
    public event Action<GameState> OnStateChanged;
    public event Action<bool, PlayerData, RoleType> OnRevealResult; // didWin, pickedTarget, target

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ChangeState(GameState.Menu);
    }

    public void ChangeState(GameState newState)
    {
        if (_currentState == newState) return;

        Debug.Log($"Game State: {_currentState} -> {newState}");

        // Exit current state
        OnExitState(_currentState);

        _currentState = newState;

        // Enter new state
        OnEnterState(newState);

        // Notify listeners
        OnStateChanged?.Invoke(newState);
    }

    private void OnExitState(GameState state)
    {
        // Cleanup for each state
        switch (state)
        {
            case GameState.Shuffling:
                // Stop shuffle animation
                break;
            case GameState.PolicePick:
                // Hide pick UI
                break;
            case GameState.Scorecard:
                // Hide scorecard
                break;
        }
    }

    private void OnEnterState(GameState state)
    {
        // Initialize each state
        switch (state)
        {
            case GameState.Menu:
                // Show menu
                break;
            case GameState.Shuffling:
                // Start 2-second shuffle animation
                Invoke(nameof(OnShuffleComplete), 2f);
                break;
            case GameState.RoleReveal:
                // Show roles, wait for player acknowledgment
                break;
            case GameState.PolicePick:
                // Enable police pick UI
                break;
            case GameState.RevealResult:
                // Trigger event for UI to show result
                OnRevealResult?.Invoke(
                    GameManager.Instance.DidPoliceWin,
                    GameManager.Instance.PickedTarget,
                    GameManager.Instance.CurrentTarget
                );
                Invoke(nameof(OnResultRevealComplete), 2f);
                break;
            case GameState.Scorecard:
                // Show round scores
                break;
            case GameState.GameOver:
                // Show final results
                break;
        }
    }

    private void OnShuffleComplete()
    {
        if (_currentState == GameState.Shuffling)
        {
            // Assign roles after shuffle animation
            if (GameManager.Instance.Players.Count > 0)
            {
                RoleManager.Instance.AssignRoles(GameManager.Instance.Players);

                // Initialize AI players with new roles using reflection
                InitializeAIPlayersReflection();
                
                ChangeState(GameState.RoleReveal);
            }
            else
            {
                Debug.LogError("GameStateManager: No players found in GameManager during ShuffleComplete!");
                // Optionally return to menu or try to re-init
                ReturnToMenu();
            }
        }
    }

    private void InitializeAIPlayersReflection()
    {
        // Find AIManager by component name to avoid assembly reference
        MonoBehaviour[] allComponents = FindObjectsOfType<MonoBehaviour>();
        foreach (var component in allComponents)
        {
            if (component != null && component.GetType().Name == "AIManager")
            {
                // Use reflection to call InitializeAIPlayers
                System.Reflection.MethodInfo method = component.GetType().GetMethod("InitializeAIPlayers");
                if (method != null)
                {
                    method.Invoke(component, null);
                    Debug.Log("GameStateManager: AI players initialized via reflection");
                }
                return;
            }
        }
        Debug.LogWarning("GameStateManager: AIManager not found in scene");
    }

    private void OnResultRevealComplete()
    {
        if (_currentState == GameState.RevealResult)
        {
            ChangeState(GameState.Scorecard);
        }
    }

    // Helper methods for state transitions
    public void StartGame()
    {
        GameManager.Instance.StartNewGame();
        ChangeState(GameState.Shuffling);
    }

    public void OnRoleRevealComplete()
    {
        if (_currentState == GameState.RoleReveal)
        {
            ChangeState(GameState.PolicePick);
        }
    }

    public void OnPolicePickComplete(PlayerData pickedTarget)
    {
        if (_currentState == GameState.PolicePick)
        {
            GameManager.Instance.ProcessPolicePick(pickedTarget);
            ChangeState(GameState.RevealResult);
        }
    }

    public void OnScorecardComplete()
    {
        if (_currentState == GameState.Scorecard)
        {
            if (GameManager.Instance.CurrentRound < GameManager.Instance.TotalRounds)
            {
                GameManager.Instance.StartNextRound();
                ChangeState(GameState.Shuffling);
            }
            else
            {
                ChangeState(GameState.GameOver);
            }
        }
    }

    public void ReturnToMenu()
    {
        GameManager.Instance.ResetGame();
        ChangeState(GameState.Menu);
    }
}
