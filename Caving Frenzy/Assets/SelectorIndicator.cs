using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorIndicator : MonoBehaviour
{
    public float lerpSpeed= 0.5f;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, LevelManager.Instance.ActivePlayer().transform.position, Time.deltaTime * lerpSpeed);
    }
}
