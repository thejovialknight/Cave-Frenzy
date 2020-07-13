using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemTile : MonoBehaviour
{
    public int amount = 5;

    void OnEnable() {
        TileController.onDestroy += OnDestroy;
    }

    void OnDisable() {
        TileController.onDestroy -= OnDestroy;
    }

    void OnDestroy(GameObject obj, PlayerController player) {
        if(ReferenceEquals(obj, gameObject)) {
            player.playerMatchData.score += amount;
        }
    }
}
