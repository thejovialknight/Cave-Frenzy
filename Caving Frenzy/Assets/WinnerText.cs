using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerText : MonoBehaviour
{
    void Start()
    {
        if(GameManager.Instance.winner != null) {
            GetComponent<TextMeshPro>().text = "Winner: " + GameManager.Instance.winner.nickname + "!";
        }
    }
}
