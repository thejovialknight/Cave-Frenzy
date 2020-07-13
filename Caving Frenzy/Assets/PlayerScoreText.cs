using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScoreText : MonoBehaviour
{
    public PlayerController player;
    public string playerLabel;

    TextMeshProUGUI textMeshPro;

    void Awake() {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        textMeshPro.text = playerLabel + ": " + player.playerMatchData.score.ToString();
    }
}
