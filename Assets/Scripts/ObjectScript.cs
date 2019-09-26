using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite[] sprites;

    public float durationSec = 1;

    public Coroutine animationCoroutine;

    private void Start()
    {
        Utils.MakeAnimation(this, durationSec, sprites);
    }
}
