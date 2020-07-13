using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMatchData playerMatchData;

    public string horizontalAxis;
    public string verticalAxis;

    public bool isInTurn;
    public int moveLimit = 3;
    public int movesTaken = 0;
    public Vector3 targetPosition;
    public bool isMoving = false;
    public bool isUpgraded;
    public int upgradeMoveBonus = 1;
    public int upgradeMineBonus = 1;

    public RuntimeAnimatorController defaultAnimator;
    public RuntimeAnimatorController upgradedAnimator;

    public float speed = 6f;

    public AudioClip runSound;

    public Animator animator;

    void Start() {
        UpdateFOWFromCurrentPosition();
        GameManager.Instance.players.Add(this);
    }

    void Update()
    {
        if(!isMoving) {
            if(isInTurn) {

                if(Input.GetButtonDown("Up")) {
                    if(Action(0, 1)) {
                        AdvanceMove();
                    }
                }

                if(Input.GetButtonDown("Down")) {
                    if(Action(0, -1)) {
                        AdvanceMove();
                    }
                }

                if(Input.GetButtonDown("Left")) {
                    if(Action(-1, 0)) {
                        AdvanceMove();
                    }
                }

                if(Input.GetButtonDown("Right")) {
                    if(Action(1, 0)) {
                        AdvanceMove();
                    }
                }
            }
        }
        else {
            Vector2 movementVector = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.position = movementVector;
            if(transform.position == targetPosition) {
                isMoving = false;
            }
        }

        if(playerMatchData.score >= GameManager.Instance.ruleset.goalAmount) {
            GameManager.Instance.EndMatch(playerMatchData);
        }
    }

    void AdvanceMove() {
        movesTaken++;
        if(movesTaken >= GetMoveLimit()) {
            LevelManager.Instance.AdvanceTurn();
            movesTaken = 0;
        }

        Input.ResetInputAxes();
    }

    bool Action(int x, int y) {
        int desiredX = (int)transform.position.x + x;
        int desiredY = (int)transform.position.y + y;

        if(LevelManager.Instance.CheckWalkable(desiredX, desiredY)) {
            Move(desiredX, desiredY);
            return true;
        }

        if(LevelManager.Instance.CheckDiggable(desiredX, desiredY)) {
            Dig(desiredX, desiredY, GetPower());
            return true;
        }

        if(LevelManager.Instance.CheckAnvil(desiredX, desiredY)) {
            Upgrade();
            return true;
        }

        return false;
    }

    void Move(int desiredX, int desiredY) {
        targetPosition = new Vector3(desiredX, desiredY, transform.position.z);
        TileController.RaiseUpdateFOW(new Vector3(desiredX, desiredY, transform.position.z));
        isMoving = true;
        animator.SetFloat("xDirection", targetPosition.x - transform.position.x);
        animator.SetFloat("yDirection", targetPosition.y - transform.position.y);
        animator.SetTrigger("moving");
        AudioSource.PlayClipAtPoint(runSound, Camera.main.transform.position);
    }

    void Dig(int desiredX, int desiredY, int power) {
        if(LevelManager.Instance.Dig(desiredX, desiredY, GetPower(), this) == true) {
            Move(desiredX, desiredY);
        }
        animator.SetFloat("xDirection", desiredX - transform.position.x);
        animator.SetFloat("yDirection", desiredY - transform.position.y);
        animator.SetTrigger("moving");
        UpdateFOWFromCurrentPosition();
    }

    void Upgrade() {
        animator.runtimeAnimatorController = upgradedAnimator;
        isUpgraded = true;
    }

    void UpdateFOWFromCurrentPosition() {
        TileController.RaiseUpdateFOW(new Vector3(transform.position.x, transform.position.y, transform.position.z));
    }

    public void StartTurn() {
        isInTurn = true;
    }

    public void EndTurn() {
        isInTurn = false;
    }

    public int GetMoveLimit() {
        int limit = moveLimit;
        if(isUpgraded) {
            limit += upgradeMoveBonus;
        }

        return limit;
    }

    public int GetPower() {
        int power = 1;
        if(isUpgraded) {
            power += upgradeMineBonus;
        }

        return power;
    }
}

[System.Serializable]
public class PlayerMatchData {
    public string nickname;
    public int score;
}
