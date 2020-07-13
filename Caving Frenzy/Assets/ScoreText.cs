using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    void Start()
    {
        if(GameManager.Instance.winner != null && GameManager.Instance.loser != null) {
            TextMeshPro tmpText = GetComponent<TextMeshPro>();
            tmpText.text = "Winner: " + GameManager.Instance.winner.score + ", Loser: " + GameManager.Instance.loser.score;
        }
    }
}
