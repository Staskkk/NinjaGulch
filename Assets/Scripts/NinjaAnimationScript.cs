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

    public Sprite[] immortalitySprites;

    public Sprite[] speedUpSprites;

    public Sprite[] playerTurnUpSprites;

    public Sprite[] playerTurnDownSprites;

    public Sprite[] dieSprites;

    public float playerTurnDurationSec;

    public float attackDurationSec;

    public float getDamagedDurationSec;

    public float dieDuration;

    public float movementAnimSpeedRatio;

    public ObjectScript powerUpSpriteObject;

    public ObjectScript playerTurnObject;

    public PlayerControlScript playerControl;

    public ObjectScript objectScript;

    public PlayerScript playerScript;

    public PlayerAttackScript attackScript;

    private Sprite[][][][] sprites;

    private Sprite[][] powerSprites;

    private Sprite[][] playerTurnSprites;

    private int oldAnim = -1;

    private int oldPowerAnim = -1;

    private PlayerTurn? oldPlayerTurn = null;

    private float oldPlayerSpeed = -1;

    private Coroutine damageCoroutine;

    private enum AnimActionState
    {
        Idle = 0,
        Move = 1,
        Attack = 2
    }

    private enum AnimItemState
    {
        Hands = 0,
        Flag = 1,
        Katana = 2
    }

    private enum AnimPowerState
    {
        None,
        Immortality,
        SpeedUp
    }

    private AnimActionState ActionState
    {
        get
        {
            if (attackScript.isHitting)
            {
                return AnimActionState.Attack;
            }

            if (playerControl.isIdle)
            {
                return AnimActionState.Idle;
            }

            return AnimActionState.Move;
        }
    }

    private AnimItemState ItemState
    {
        get
        {
            if (playerScript.carriedFlag != null)
            {
                return AnimItemState.Flag;
            }

            if (playerScript.powerUp == PowerUp.Katana)
            {
                return AnimItemState.Katana;
            }

            return AnimItemState.Hands;
        }
    }

    private AnimPowerState PowerState
    {
        get
        {
            if (playerScript.powerUp == PowerUp.Immortality)
            {
                return AnimPowerState.Immortality;
            }
            else if (playerScript.powerUp == PowerUp.SpeedUp)
            {
                return AnimPowerState.SpeedUp;
            }

            return AnimPowerState.None;
        }
    }

    void Start()
    {
        sprites = new Sprite[3][][][];

        sprites[(int)AnimActionState.Idle] = new Sprite[3][][];  
        
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Hands] = new Sprite[4][];
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Hands][(int)PlayerDirection.Left] = idleLeft;
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Hands][(int)PlayerDirection.Top] = idleTop;
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Hands][(int)PlayerDirection.Right] = idleRight;
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Hands][(int)PlayerDirection.Bottom] = idleBottom;

        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Flag] = new Sprite[4][];
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Flag][(int)PlayerDirection.Left] = idleFlagLeft;
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Flag][(int)PlayerDirection.Top] = idleFlagTop;
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Flag][(int)PlayerDirection.Right] = idleFlagRight;
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Flag][(int)PlayerDirection.Bottom] = idleFlagBottom;

        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Katana] = new Sprite[4][];
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Katana][(int)PlayerDirection.Left] = idleKatanaLeft;
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Katana][(int)PlayerDirection.Top] = idleKatanaTop;
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Katana][(int)PlayerDirection.Right] = idleKatanaRight;
        sprites[(int)AnimActionState.Idle][(int)AnimItemState.Katana][(int)PlayerDirection.Bottom] = idleKatanaBottom;

        sprites[(int)AnimActionState.Move] = new Sprite[3][][];

        sprites[(int)AnimActionState.Move][(int)AnimItemState.Hands] = new Sprite[4][];
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Hands][(int)PlayerDirection.Left] = movementLeft;
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Hands][(int)PlayerDirection.Top] = movementTop;
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Hands][(int)PlayerDirection.Right] = movementRight;
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Hands][(int)PlayerDirection.Bottom] = movementBottom;

        sprites[(int)AnimActionState.Move][(int)AnimItemState.Flag] = new Sprite[4][];
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Flag][(int)PlayerDirection.Left] = movementFlagLeft;
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Flag][(int)PlayerDirection.Top] = movementFlagTop;
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Flag][(int)PlayerDirection.Right] = movementFlagRight;
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Flag][(int)PlayerDirection.Bottom] = movementFlagBottom;

        sprites[(int)AnimActionState.Move][(int)AnimItemState.Katana] = new Sprite[4][];
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Katana][(int)PlayerDirection.Left] = movementKatanaLeft;
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Katana][(int)PlayerDirection.Top] = movementKatanaTop;
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Katana][(int)PlayerDirection.Right] = movementKatanaRight;
        sprites[(int)AnimActionState.Move][(int)AnimItemState.Katana][(int)PlayerDirection.Bottom] = movementKatanaBottom;

        sprites[(int)AnimActionState.Attack] = new Sprite[3][][];

        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Hands] = new Sprite[4][];
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Hands][(int)PlayerDirection.Left] = attackLeft;
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Hands][(int)PlayerDirection.Top] = attackTop;
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Hands][(int)PlayerDirection.Right] = attackRight;
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Hands][(int)PlayerDirection.Bottom] = attackBottom;

        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Flag] = new Sprite[4][];
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Flag][(int)PlayerDirection.Left] = attackFlagLeft;
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Flag][(int)PlayerDirection.Top] = attackFlagTop;
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Flag][(int)PlayerDirection.Right] = attackFlagRight;
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Flag][(int)PlayerDirection.Bottom] = attackFlagBottom;

        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Katana] = new Sprite[4][];
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Katana][(int)PlayerDirection.Left] = attackKatanaLeft;
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Katana][(int)PlayerDirection.Top] = attackKatanaTop;
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Katana][(int)PlayerDirection.Right] = attackKatanaRight;
        sprites[(int)AnimActionState.Attack][(int)AnimItemState.Katana][(int)PlayerDirection.Bottom] = attackKatanaBottom;

        powerSprites = new Sprite[3][];
        powerSprites[(int)AnimPowerState.None] = new Sprite[] { null };
        powerSprites[(int)AnimPowerState.Immortality] = immortalitySprites;
        powerSprites[(int)AnimPowerState.SpeedUp] = speedUpSprites;

        playerTurnSprites = new Sprite[2][];
        playerTurnSprites[(int)PlayerTurn.Up] = playerTurnUpSprites;
        playerTurnSprites[(int)PlayerTurn.Down] = playerTurnDownSprites;
    }

    void Update()
    {
        if (!this.playerScript.isAlive)
        {
            return;
        }

        int actionState = (int)this.ActionState;
        int itemState = (int)this.ItemState;
        int playerDirection = (int)playerControl.playerDirection;
        int currentAnim = actionState * 100 + itemState * 10 + playerDirection;
        if (currentAnim != oldAnim || oldPlayerSpeed != playerScript.TotalSpeed)
        {
            Utils.MakeAnimation(
                objectScript,
                actionState != (int)AnimActionState.Attack ? 1 / playerScript.TotalSpeed * movementAnimSpeedRatio : attackDurationSec,
                sprites[actionState][itemState][playerDirection]);
            oldAnim = currentAnim;
            oldPlayerSpeed = playerScript.TotalSpeed;
        }

        int powerState = (int)this.PowerState;
        int currentPowerAnim = powerState;
        if (currentPowerAnim != oldPowerAnim)
        {
            Utils.MakeAnimation(powerUpSpriteObject, playerScript.powerUpDuration ?? 1, powerSprites[powerState]);
            oldPowerAnim = currentPowerAnim;
        }

        if (oldPlayerTurn != playerControl.playerTurn)
        {
            Utils.MakeAnimation(playerTurnObject, playerTurnDurationSec, playerTurnSprites[(int)playerControl.playerTurn]);
            oldPlayerTurn = playerControl.playerTurn;
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
        if (this.playerScript.isAlive)
        {
            this.damageCoroutine = StartCoroutine(DamageAnimCoroutine(damage));
        }
    }

    private IEnumerator DamageAnimCoroutine(float damage)
    {
        float damageDelay = getDamagedDurationSec / 6;
        for (int i = 0; i < 6; i++)
        {
            objectScript.spriteRenderer.enabled = !objectScript.spriteRenderer.enabled;
            yield return new WaitForSeconds(damageDelay);
        }
    }

    public void DieAnimation()
    {
        if (this.damageCoroutine != null)
        {
            StopCoroutine(this.damageCoroutine);
        }

        objectScript.spriteRenderer.enabled = true;
        Utils.MakeAnimation(objectScript, dieDuration, dieSprites);
    }

    public void ResurrectAnimation()
    {
    }
}


