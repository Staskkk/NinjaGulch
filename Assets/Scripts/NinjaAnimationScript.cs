using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NinjaAnimationScript : MonoBehaviour
{
    public Sprite[] idleLeft;
    public Sprite[] idleTop;
    public Sprite[] idleRight;
    public Sprite[] idleBottom;

    public Sprite[] idleFlagLeft;
    public Sprite[] idleFlagTop;
    public Sprite[] idleFlagRight;
    public Sprite[] idleFlagBottom;

    public Sprite[] idleKatanaLeft;
    public Sprite[] idleKatanaTop;
    public Sprite[] idleKatanaRight;
    public Sprite[] idleKatanaBottom;

    public Sprite[] movementLeft;
    public Sprite[] movementTop;
    public Sprite[] movementRight;
    public Sprite[] movementBottom;

    public Sprite[] movementFlagLeft;
    public Sprite[] movementFlagTop;
    public Sprite[] movementFlagRight;
    public Sprite[] movementFlagBottom;

    public Sprite[] movementKatanaLeft;
    public Sprite[] movementKatanaTop;
    public Sprite[] movementKatanaRight;
    public Sprite[] movementKatanaBottom;

    public Sprite[] attackLeft;
    public Sprite[] attackTop;
    public Sprite[] attackRight;
    public Sprite[] attackBottom;

    public Sprite[] attackFlagLeft;
    public Sprite[] attackFlagTop;
    public Sprite[] attackFlagRight;
    public Sprite[] attackFlagBottom;

    public Sprite[] attackKatanaLeft;
    public Sprite[] attackKatanaTop;
    public Sprite[] attackKatanaRight;
    public Sprite[] attackKatanaBottom;

    public SpriteRenderer powerUpSpriteRenderer;

    public Sprite[] immortalitySprites;

    public Sprite[] speedUpSprites;

    public float powerUpDuration;

    public float idleDurationSec;

    public float movementDurationSec;

    public float attackDurationSec;

    public PlayerControlScript playerControl;

    public ObjectScript objectScript;

    public PlayerScript playerScript;

    public PlayerAttackScript attackScript;

    private Sprite[][] movement;

    private int oldAnim = -1;

    void Start()
    {
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
                Utils.MakeAnimation(objectScript, movementDurationSec, movement[currentAnim]);
                oldAnim = currentAnim;
            }
        }
    }

    public void TakeFlagAnimation()
    {

    }

    public void ConveyFlagAnimation()
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


