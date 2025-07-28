// BaseManager.cs
// Author: Sadikur Rahman
// Description: Base class for all game managers with shared structure.

using UnityEngine;

public abstract class BaseManager : MonoBehaviour
{
    public virtual void Init() {}
    public virtual void ResetManager() {}
}