using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveTrapScript : MonoBehaviour, ITrap
{
    public float damage;

    public int dissolveStage;

    public int finalDissolveStage;

    public float dissolveDelay;

    public float dissolveTime;

    private bool isDissolving = false;

    private readonly List<PlayerScript> enteredPlayers = new List<PlayerScript>();

    public float Damage => damage;

    void Start()
    {
        
    }

    void Update()
    {
        if (isDissolving && dissolveStage < finalDissolveStage)
        {
            dissolveTime += Time.deltaTime;
            if (dissolveTime >= dissolveDelay)
            {
                dissolveTime = 0;
                dissolveStage++;
                if (dissolveStage >= finalDissolveStage)
                {
                    dissolveStage = finalDissolveStage;
                    foreach (var ninja in enteredPlayers)
                    {
                        this.HitNinja(ninja);
                    }
                }
            }
        }
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
            enteredPlayers.Add(player);
            if (player.isAlive)
            {
                if (dissolveStage == finalDissolveStage)
                {
                    this.HitNinja(player);
                }
                else if (!isDissolving)
                {
                    isDissolving = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            enteredPlayers.Remove(player);
            if (enteredPlayers.Count == 0)
            {
                isDissolving = false;
            }
        }
    }
}
