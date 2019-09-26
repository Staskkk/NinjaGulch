using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public float damage = 5f;

    public float activateDurationSec = 1;

    public Sprite[] activateSprites;

    private ObjectScript objectScript;

    void Start()
    {
        objectScript = GetComponent<ObjectScript>();
    }

    void Update()
    {      
    }

    public void Activate()
    {
        Utils.MakeAnimation(objectScript, activateDurationSec, activateSprites, true, () =>
        {
            Utils.MakeAnimation(objectScript, objectScript.durationSec, objectScript.sprites);
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            if (player.isAlive)
            {
                this.Activate();
                player.SetDamage(damage);
            }
        }
    }
}
