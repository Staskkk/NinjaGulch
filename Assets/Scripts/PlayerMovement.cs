using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum PlayerState
{
    Left = 0,
    Right = 1,
    Top = 2,
    Bottom = 3,
    IdleLeft = 4,
    IdleRight = 5,
    IdleTop = 6,
    IdleBottom = 7
}

public enum PlayerTurn
{
    Up = 0,
    Down = 1
}

public class PlayerMovement : MonoBehaviour
{
    public KeyCode keyLeft;
    public KeyCode keySwitch;
    public KeyCode keyRight;

    public float speed;

    public PlayerState playerState;

    public PlayerTurn playerTurn;

    void Start()
    {
        
    }

    void Update()
    {
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
            if ((int)playerState < (int)PlayerState.IdleLeft)
            {
                playerState = (PlayerState)((int)(playerState) + 4);
            }

            movementVector = Vector3.zero;
        } else if (isLeftHolded)
        {
            playerState = PlayerState.Left;
            movementVector = Vector3.left;
        } else if (isRightHolded)
        {
            playerState = PlayerState.Right;
            movementVector = Vector3.right;
        }
        else
        {
            if (playerTurn == PlayerTurn.Up)
            {
                playerState = PlayerState.Top;
                movementVector = Vector3.up;
            }
            else
            {
                playerState = PlayerState.Bottom;
                movementVector = Vector3.down;
            }
        }

        gameObject.transform.position += speed * movementVector * Time.deltaTime;
    }
}
