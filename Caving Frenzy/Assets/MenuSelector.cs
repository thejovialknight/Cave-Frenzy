using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelector : MonoBehaviour
{
    public float speed = 4;

    public float horizontalDirection = 1;

    public Animator animator;

    void Update()
    {
        Vector3 targetPosition = MenuManager.Instance.buttons[MenuManager.Instance.selection].transform.position;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPosition.y, transform.position.z), speed * Time.deltaTime);

        if(transform.position.y == targetPosition.y) {
            animator.SetFloat("xDirection", horizontalDirection);
            animator.SetFloat("yDirection", 0f);
            animator.SetTrigger("stopMoving");
        }
        else {
            animator.SetTrigger("moving");
            animator.SetFloat("xDirection", 0f);
            animator.SetFloat("yDirection", targetPosition.y - transform.position.y);
        }
    }
}
