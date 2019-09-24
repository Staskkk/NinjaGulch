using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaSpriteAnimation : MonoBehaviour
{
    public Sprite[] movementLeft;
    public Sprite[] movementTop;
    public Sprite[] movementRight;
    public Sprite[] movementBottom;
    public Sprite[] idleLeft;
    public Sprite[] idleTop;
    public Sprite[] idleRight;
    public Sprite[] idleBottom;

    public float durationSec;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    private Sprite[][] movement;
    private int[] curSprite;
    private float[] changeTime;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        movement = new Sprite[8][];
        movement[(int)PlayerState.Left] = movementLeft;
        movement[(int)PlayerState.Top] = movementTop;
        movement[(int)PlayerState.Right] = movementRight;
        movement[(int)PlayerState.Bottom] = movementBottom;
        movement[(int)PlayerState.IdleLeft] = idleLeft;
        movement[(int)PlayerState.IdleTop] = idleTop;
        movement[(int)PlayerState.IdleRight] = idleRight;
        movement[(int)PlayerState.IdleBottom] = idleBottom;
        curSprite = new int[movement.Length];
        changeTime = new float[movement.Length];
    }

    void Update()
    {
        if (movement != null)
        {
            int currentAnim = (int)playerMovement.playerState;
            MakeAnimation(movement[currentAnim], ref curSprite[currentAnim], ref changeTime[currentAnim]);
        }
    }

    private void MakeAnimation(Sprite[] sprites, ref int curSprite, ref float changeTime)
    {
        if (changeTime <= Time.time && sprites != null && sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[curSprite];

            curSprite++;
            if (curSprite == sprites.Length)
            {
                curSprite = 0;
            }

            changeTime = Time.time + (durationSec / sprites.Length);
        }
    }
}


