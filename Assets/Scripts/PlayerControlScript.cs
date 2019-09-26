using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{
    public KeyCode keyLeft;
    public KeyCode keySwitch;
    public KeyCode keyRight;

    public float speed;

    public PlayerState playerState;

    public PlayerTurn playerTurn;

    public Transform main;

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
            main.rotation = Quaternion.Euler(0, 0, 180);
        } else if (isRightHolded)
        {
            playerState = PlayerState.Right;
            movementVector = Vector3.right;
            main.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            if (playerTurn == PlayerTurn.Up)
            {
                playerState = PlayerState.Top;
                movementVector = Vector3.up;
                main.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                playerState = PlayerState.Bottom;
                movementVector = Vector3.down;
                main.rotation = Quaternion.Euler(0, 0, -90);
            }
        }

        gameObject.transform.position += speed * movementVector * Time.deltaTime;
    }
}
