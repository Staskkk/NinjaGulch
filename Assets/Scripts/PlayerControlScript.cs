using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{
    public KeyCode keyLeft;
    public KeyCode keySwitch;
    public KeyCode keyRight;

    public PlayerScript player;

    public PlayerDirection playerDirection;

    public PlayerTurn playerTurn;

    public bool isIdle;

    public Transform main;

    void Start()
    {
    }

    void Update()
    {
        if (!player.isAlive)
        {
            return;
        }

        bool isLeftHolded = Input.GetKey(keyLeft);
        bool isRightHolded = Input.GetKey(keyRight);
        bool isSwitchHolded = Input.GetKey(keySwitch);
        Vector3 movementVector;

        if (Input.GetKeyDown(keySwitch))
        {
            playerTurn = playerTurn == PlayerTurn.Down ? PlayerTurn.Up : PlayerTurn.Down;
        }

        if (isSwitchHolded || (isLeftHolded && isRightHolded))
        {
            isIdle = true;
            movementVector = Vector3.zero;
        } else if (isLeftHolded)
        {
            isIdle = false;
            playerDirection = PlayerDirection.Left;
            movementVector = Vector3.left;
            main.rotation = Quaternion.Euler(0, 0, 180);
        } else if (isRightHolded)
        {
            isIdle = false;
            playerDirection = PlayerDirection.Right;
            movementVector = Vector3.right;
            main.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            if (playerTurn == PlayerTurn.Up)
            {
                isIdle = false;
                playerDirection = PlayerDirection.Top;
                movementVector = Vector3.up;
                main.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                isIdle = false;
                playerDirection = PlayerDirection.Bottom;
                movementVector = Vector3.down;
                main.rotation = Quaternion.Euler(0, 0, -90);
            }
        }

        player.transform.position += (player.speed + player.bonusSpeed) * movementVector * Time.deltaTime;
    }
}
