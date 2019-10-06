using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAnimationScript : MonoBehaviour
{
    public ObjectScript objectScript;

    public float activateDurationSec;

    public Sprite[] activateSprites;

    public float activeDurationSec;

    public Sprite[] activeSprites;

    private int inTriggerZoneCount = 0;

    public void Activate()
    {
        Debug.Log($"Trap was activated!");
        Utils.MakeAnimation(objectScript, activateDurationSec, activateSprites, true, () =>
        {
            Utils.MakeAnimation(objectScript, activeDurationSec, activeSprites);
        });
    }

    public void Deactivate()
    {
        Debug.Log($"Trap was deactivated!");
        Utils.MakeAnimation(objectScript, objectScript.durationSec, objectScript.sprites);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inTriggerZoneCount++;
            var player = other.GetComponent<PlayerScript>();
            if (player.isAlive)
            {
                this.Activate();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inTriggerZoneCount--;
            if (inTriggerZoneCount == 0)
            {
                Deactivate();
            }
        }
    }
}
