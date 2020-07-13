using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Match Ruleset")]

public class MatchRuleset : ScriptableObject
{
    public GameMode mode;
    public int goalAmount;
}

public enum GameMode {
    GemHunt,
    Exploration,
    TimeAttack,
    DiamondHunt
}