using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAnimationScript : MonoBehaviour
{
    public TrapScript trapScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            if (player.isAlive)
            {
                trapScript.Activate();
            }
        }
    }
}
