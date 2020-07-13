using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemRandomizer : MonoBehaviour
{
    public Animator animator;
    public List<RuntimeAnimatorController> controllers = new List<RuntimeAnimatorController>();

    void Start()
    {
        animator.runtimeAnimatorController = controllers[Random.Range(0, controllers.Count)];
    }
}
