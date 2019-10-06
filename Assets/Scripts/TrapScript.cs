﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public float damage = 5f;

    void Start()
    {
    }

    void Update()
    {      
    }

    public void HitNinja(PlayerScript ninja)
    {
        Debug.Log($"Trap hit ninja!");
        ninja.SetDamage(damage, out bool isDied);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            if (player.isAlive)
            {
                this.HitNinja(player);
            }
        }
    }
}
