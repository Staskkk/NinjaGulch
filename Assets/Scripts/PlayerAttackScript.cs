using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public PlayerScript currentPlayer;

    public float hitDelay;

    public float preHitDelay;

    private Coroutine hittingCoroutine;

    void Start()
    {
    }

    void Update()
    {        
    }

    public void StartHitting(PlayerScript otherPlayer)
    {
        hittingCoroutine = StartCoroutine(HittingRoutine(otherPlayer));
    }

    private IEnumerator HittingRoutine(PlayerScript otherPlayer)
    {
        while (true)
        {
            yield return new WaitForSeconds(preHitDelay);
            if (otherPlayer.isAlive)
            {
                otherPlayer.SetDamage(currentPlayer.damage + currentPlayer.bonusDamage, out bool isDied);
                if (isDied)
                {
                    GameManagerScript.i.AddScores(ScoreOption.NinjaElimination, currentPlayer.Team);
                }
            }

            yield return new WaitForSeconds(hitDelay);
        }
    }

    public void StopHitting()
    {
        if (hittingCoroutine != null)
        {
            StopCoroutine(hittingCoroutine);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            if (currentPlayer.Team != player.Team && !currentPlayer.isRangeAttack)
            {
                this.StartHitting(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            if (currentPlayer.Team != player.Team && !currentPlayer.isRangeAttack)
            {
                this.StopHitting();
            }
        }
    }
}
