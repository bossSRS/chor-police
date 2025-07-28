// ScoreConfig.cs
// Author: Sadikur Rahman
// Description: ScriptableObject that holds score values for each role.

using UnityEngine;

[CreateAssetMenu(fileName = "ScoreConfig", menuName = "ChorPolice/ScoreConfig")]
public class ScoreConfig : ScriptableObject
{
    public int babuScore = 1000;
    public int policeWinScore = 800;
    public int chorSurviveScore = 700;
    public int dakatSurviveScore = 600;
}