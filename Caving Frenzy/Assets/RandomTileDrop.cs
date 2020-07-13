using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTileDrop : MonoBehaviour
{
    public GameObject tileToDrop;
    public float chanceToDrop;

    void OnEnable() {
        TileController.onDestroy += OnDestroy;
    }

    void OnDisable() {
        TileController.onDestroy -= OnDestroy;
    }

    void OnDestroy(GameObject obj, PlayerController player) {
        if(ReferenceEquals(obj, gameObject)) {
            if(Random.Range(0f, 1f) <= chanceToDrop) {
                LevelManager.Instance.PlaceTile((int)transform.position.x, (int)transform.position.y, tileToDrop);
            }
        }
    }
}
