// ScoreConfig.cs
// Author: Sadikur Rahman
// Description: ScriptableObject that holds score values for each role.

using UnityEngine;

[CreateAssetMenu(fileName = "ScoreConfig", menuName = "ChorPolice/ScoreConfig")]
public class ScoreConfig : ScriptableObject
{
    [Header("Babu Scoring")]
    public int babuScore = 1000;

    [Header("Police Scoring")]
    public int policeWinScore = 800;

    [Header("Chor Scoring")]
    public int chorSurviveScore = 700;  // When not caught
    public int chorCaughtScorePartial = 400;  // 50% chance when caught

    [Header("Dakat Scoring")]
    public int dakatSurviveScore = 600;  // When not caught
    public int dakatCaughtScorePartial = 600;  // 50% chance when caught
}
