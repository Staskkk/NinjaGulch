using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum PlayerDirection
{
    Left = 0,
    Top = 1,
    Right = 2,
    Bottom = 3
}

public enum PlayerTurn
{
    Up = 0,
    Down = 1
}

public enum PowerUp
{
    None = 0,
    Katana = 1,
    Immortality = 2,
    SpeedUp = 3,
    Shurikens = 4,
}

public enum Team
{
    Blue = 0,
    Red = 1,
    None = 2
}

public enum ScoreOption
{
    NinjaElimination,
    FlagConvey,
}

public enum GameResult
{
    Win = 0,
    Draw = 1
}

public delegate void NextAction();

public delegate void SetupParamsFunc(GameObject obj);

public static class Utils
{
    public static void MakeAnimation(ObjectScript animatedObject, float durationSec, Sprite[] sprites, bool playOnce = false, NextAction nextAction = null)
    {
        if (animatedObject == null)
        {
            return;
        }

        if (animatedObject.animationCoroutine != null)
        {
            animatedObject.StopCoroutine(animatedObject.animationCoroutine);
        }

        animatedObject.animationCoroutine =
                animatedObject.StartCoroutine(AnimationCoroutine(animatedObject.spriteRenderer, durationSec, sprites, playOnce, nextAction));
    }

    private static IEnumerator AnimationCoroutine(SpriteRenderer spriteRenderer, float durationSec, Sprite[] sprites, bool playOnce, NextAction nextAction)
    {
        if (spriteRenderer != null && sprites != null && sprites.Length > 0)
        {
            int curSprite = 0;
            do
            {
                spriteRenderer.sprite = sprites[curSprite];

                curSprite++;
                if (curSprite == sprites.Length)
                {
                    if (playOnce)
                    {
                        break;
                    }

                    curSprite = 0;
                }

                yield return new WaitForSeconds(durationSec / sprites.Length);
            } while (true);
        }

        nextAction?.Invoke();
    }
}
