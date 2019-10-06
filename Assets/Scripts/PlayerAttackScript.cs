using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public PlayerScript currentPlayer;

    public PlayerControlScript playerControl;

    public float hitDelay;

    public float preHitDelay;

    public bool isHitDelay;

    public bool isHitting;

    private PlayerScript otherPlayer;

    private Coroutine hittingCoroutine;

    private PlayerDirection? hitDirection = null;

    void Start()
    {
    }

    void Update()
    {
        if (otherPlayer != null && currentPlayer.Team != otherPlayer.Team && !currentPlayer.isRangeAttack && !isHitDelay && !isHitting)
        {
            MakeHit();
        }
        else if (isHitting && hitDirection != playerControl.playerDirection)
        {
            StopHit();
        }
    }

    public void MakeHit()
    {
        isHitting = true;
        hitDirection = playerControl.playerDirection;
        hittingCoroutine = StartCoroutine(HittingRoutine());
    }
    
    public void StopHit()
    {
        if (hittingCoroutine != null)
        {
            isHitting = false;
            hitDirection = null;
            StopCoroutine(hittingCoroutine);
        }
    }

    private IEnumerator HittingDelayRoutine()
    {
        yield return new WaitForSeconds(hitDelay);
        isHitDelay = false;
    }

    private IEnumerator HittingRoutine()
    {
        yield return new WaitForSeconds(preHitDelay);
        if (otherPlayer != null && isHitting && otherPlayer.isAlive)
        {
            otherPlayer.SetDamage(currentPlayer.damage + currentPlayer.bonusDamage, out bool isDied);
            if (isDied)
            {
                GameManagerScript.i.AddScores(ScoreOption.NinjaElimination, currentPlayer.Team);
            }

            isHitDelay = true;
            StartCoroutine(HittingDelayRoutine());
        }

        isHitting = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            if (currentPlayer.Team != player.Team && !currentPlayer.isRangeAttack)
            {
                this.otherPlayer = player;
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
                this.otherPlayer = null;
                this.StopHit();
            }
        }
    }
}
