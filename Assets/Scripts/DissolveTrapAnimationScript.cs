using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveTrapAnimationScript : MonoBehaviour
{
    public ObjectScript objectScript;

    public DissolveTrapScript trapScript;

    public Sprite[] dissolveSprites;

    public float dissolveAnimDurationSec;

    private int? oldDissolveStage = null;

    private void Update()
    {
        if (oldDissolveStage != trapScript.dissolveStage)
        {
            Utils.MakeAnimation(objectScript, dissolveAnimDurationSec, new Sprite[] { dissolveSprites[trapScript.dissolveStage] });
            oldDissolveStage = trapScript.dissolveStage;
        }
    }
}
