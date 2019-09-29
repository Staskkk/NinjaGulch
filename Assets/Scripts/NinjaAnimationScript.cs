using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAnimationScript : MonoBehaviour
{
    public Sprite[] movementLeft;
    public Sprite[] movementTop;
    public Sprite[] movementRight;
    public Sprite[] movementBottom;
    public Sprite[] idleLeft;
    public Sprite[] idleTop;
    public Sprite[] idleRight;
    public Sprite[] idleBottom;

    public float durationSec = 1;

    private PlayerControlScript playerControl;

    private Sprite[][] movement;

    private ObjectScript objectScript;

    private int oldAnim = -1;

    void Start()
    {
        playerControl = GetComponentInChildren<PlayerControlScript>(false);
        objectScript = GetComponent<ObjectScript>();
        movement = new Sprite[8][];
        movement[(int)PlayerState.Left] = movementLeft;
        movement[(int)PlayerState.Top] = movementTop;
        movement[(int)PlayerState.Right] = movementRight;
        movement[(int)PlayerState.Bottom] = movementBottom;
        movement[(int)PlayerState.IdleLeft] = idleLeft;
        movement[(int)PlayerState.IdleTop] = idleTop;
        movement[(int)PlayerState.IdleRight] = idleRight;
        movement[(int)PlayerState.IdleBottom] = idleBottom;
    }

    void Update()
    {
        if (movement != null)
        {
            int currentAnim = (int)playerControl.playerState;
            if (currentAnim != oldAnim)
            {
                Utils.MakeAnimation(objectScript, durationSec, movement[currentAnim]);
                oldAnim = currentAnim;
            }
        }
    }

    public void TakeFlagAnimation()
    {

    }

    public void PowerUpAnimation(PowerUp powerUp)
    {

    }

    public void DamageAnimation(float damage)
    {

    }

    public void DieAnimation()
    {
        objectScript.spriteRenderer.enabled = false;
    }

    public void ResurrectAnimation()
    {
    }
}


