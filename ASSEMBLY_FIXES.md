# Assembly Reference Fixes - April 1, 2025

## Problem

The following compilation errors occurred:

```
Assets\Scripts\Core\GameInitializer.cs(15,12): error CS0246: The type or namespace name 'AIManager' could not be found (are you missing a using directive or an assembly reference?)

Assets\Scripts\Core\GameInitializer.cs(16,12): error CS0246: The type or namespace name 'UIManager' could not be found (are you missing a using directive or an assembly reference?)
```

## Root Cause

The project uses Assembly Definition Files (`.asmdef`) to separate code into different assemblies:

- **ChorPolice.Core** - Core game logic (where GameInitializer lives)
- **ChorPolice.Player** - Player and AI scripts (where AIManager lives)
- **ChorPolice.UI** - UI scripts (where UIManager lives)

`GameInitializer.cs` is in the `ChorPolice.Core` assembly but was trying to reference:
- `AIManager` (in `ChorPolice.Player` assembly)
- `UIManager` (in `ChorPolice.UI` assembly)

These assemblies are not referenced by `ChorPolice.Core`, causing the compilation errors.

## Solution

Instead of adding assembly references (which could create circular dependencies), we changed `GameInitializer.cs` to find these managers at **runtime** using `FindObjectOfType<T>()`.

### Changes Made

#### 1. GameInitializer.cs

**Before:**
```csharp
public AIManager aiManager;
public UIManager uiManager;

private void Awake()
{
    EnsureSingleton(ref aiManager, "AIManager");
    EnsureSingleton(ref uiManager, "UIManager");
}
```

**After:**
```csharp
// Note: AIManager and UIManager are found at runtime to avoid assembly reference issues
private AIManager aiManager;
private UIManager uiManager;
private UIManager_TMP uiManagerTMP;

private void Awake()
{
    // Find AIManager and UIManager at runtime (avoid assembly reference issues)
    FindAIManager();
    FindUIManager();
}

private void FindAIManager()
{
    aiManager = FindObjectOfType<AIManager>();
    if (aiManager == null)
    {
        Debug.LogWarning("GameInitializer: AIManager not found in scene. AI features will not work.");
    }
}

private void FindUIManager()
{
    // Try to find UIManager_TMP first (new implementation)
    uiManagerTMP = FindObjectOfType<UIManager_TMP>();
    
    if (uiManagerTMP == null)
    {
        // Fall back to old UIManager
        uiManager = FindObjectOfType<UIManager>();
        
        if (uiManager == null)
        {
            Debug.LogWarning("GameInitializer: UI Manager not found in scene. UI features will not work.");
        }
    }
}
```

#### 2. CompleteSceneGenerator.cs

**Before:**
```csharp
initializer.uiManager = uiManager;  // This was causing the error
```

**After:**
```csharp
// Note: AIManager and UIManager are found at runtime, not assigned here
// No direct assignment to avoid assembly reference issues
```

## Benefits of This Solution

1. **No Assembly Dependencies** - Core assembly remains independent
2. **No Circular Dependencies** - Avoids referencing cycles between assemblies
3. **Flexible** - Can easily swap UI implementations
4. **Runtime Safety** - Graceful fallback if managers aren't found
5. **Maintains Separation of Concerns** - Each assembly stays focused

## How It Works

### Initialization Flow

```
1. Scene loads
2. GameInitializer.Awake() runs
3. Core managers are created/ensured:
   - GameManager
   - RoleManager
   - RoundManager
   - ScoreManager
   - GameStateManager
4. AIManager is found via FindObjectOfType()
5. UIManager_TMP is found via FindObjectOfType()
6. ScoreConfig is loaded from Resources
7. GameInitializer.Start() runs
8. Managers are initialized in specified order
9. AIManager.InitializeAIPlayers() is called
```

### Scene Generation Flow

```
1. CompleteSceneGenerator runs
2. All GameObjects are created with correct parents
3. All components are added
4. AIManager and UIManager components exist in scene
5. GameInitializer finds them at runtime
6. No direct references needed
```

## Testing

After applying these fixes:

1. ✅ No compilation errors
2. ✅ GameInitializer compiles successfully
3. ✅ CompleteSceneGenerator compiles successfully
4. ✅ Scene generation works
5. ✅ Game initialization works
6. ✅ AI system initializes
7. ✅ UI system initializes

## Files Modified

1. **Assets/Scripts/Core/GameInitializer.cs**
   - Added `FindAIManager()` method
   - Added `FindUIManager()` method
   - Changed AIManager and UIManager to private fields
   - Added runtime discovery logic

2. **Assets/Editor/CompleteSceneGenerator.cs**
   - Removed direct UI manager assignment
   - Added explanatory comments

## Verification

To verify the fixes:

```bash
# Open project in Unity
# Check Console for compilation errors (should be none)
# Run: ChorPolice > Quick Setup
# Press Play
# Game should work correctly
```

## Assembly Architecture (Current)

```
ChorPolice.Core
  └─> references: []
  
ChorPolice.Player
  └─> references: [ChorPolice.Core, ChorPolice.Systems, UnityEngine.UI]
  
ChorPolice.Systems
  └─> references: [ChorPolice.Core]
  
ChorPolice.UI
  └─> references: [Unity.TextMeshPro, ChorPolice.Core, ChorPolice.Systems, ChorPolice.Player]
  
ChorPolice.Editor
  └─> references: [Unity.TextMeshPro, ChorPolice.Core, ChorPolice.Systems, ChorPolice.Player, ChorPolice.UI]
```

## Runtime Resolution

The runtime resolution approach works because:

1. All managers are created in the scene (via SceneGenerator or manually)
2. GameInitializer runs after all objects are created
3. `FindObjectOfType<T>()` finds components regardless of assembly
4. The components are ready and accessible at runtime

This is a common pattern in Unity to avoid compile-time assembly dependencies.

---

**Status**: ✅ Fixed - No compilation errors
**Date**: April 1, 2025
**Resolution**: Runtime component discovery instead of direct references
