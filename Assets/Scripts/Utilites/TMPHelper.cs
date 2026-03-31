// TMPHelper.cs
// Author: Sadikur Rahman
// Description: Helper class to safely handle TextMeshPro references with fallback.

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class TMPHelper
{
    private static System.Type tmpType;
    private static bool tmpChecked = false;

    static TMPHelper()
    {
        CheckTMPAvailable();
    }

    private static void CheckTMPAvailable()
    {
        if (tmpChecked) return;
        tmpChecked = true;

        foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            tmpType = assembly.GetType("UnityEngine.UI.TextMeshProUGUI");
            if (tmpType != null)
            {
                Debug.Log("TextMeshPro found!");
                break;
            }
        }

        if (tmpType == null)
        {
            Debug.LogWarning("TextMeshPro not found. Install via Window > TextMeshPro > Import TMP Essentials");
        }
    }

    public static bool IsTMPAvailable()
    {
        CheckTMPAvailable();
        return tmpType != null;
    }

    public static Component AddTMPText(GameObject obj)
    {
        if (!IsTMPAvailable())
        {
            // Fallback to Unity UI Text
            return obj.AddComponent<UnityEngine.UI.Text>();
        }

        return obj.AddComponent(tmpType);
    }
}
#endif
