using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagStartPointScript : MonoBehaviour, IDynamicObject
{
    public Team team;

    public Sprite[] withFlagSprites;

    public Sprite[] withoutFlagSprites;

    public ObjectScript objectScript;

    public float spritesDurationSec;

    public Team Team
    {
        get { return team; }
        set { team = value; }
    }

    public void Init()
    {
    }

    void Start()
    {      
    }

    public void ChangeAnimation(bool isFlagKeeped)
    {
        if (isFlagKeeped)
        {
            Utils.MakeAnimation(objectScript, spritesDurationSec, withFlagSprites);
        }
        else
        {
            Utils.MakeAnimation(objectScript, spritesDurationSec, withoutFlagSprites);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            if (player.isAlive && player.Team == this.Team && player.carriedFlag != null)
            {
                player.ConveyFlag();
            }
        }
    }
}
