using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Team team;

    public float health;

    public float maxHealth = 2;

    public bool isWithFlag;

    public bool isAlive = true;

    public PowerUp powerUp;

    public PlayerControlScript playerControl;

    private NinjaAnimationScript ninjaAnimation;

    void Start()
    {
        ninjaAnimation = GetComponent<NinjaAnimationScript>();
        isAlive = true;
        health = maxHealth;
        isWithFlag = false;
        powerUp = PowerUp.None;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }

    public void TakeFlag()
    {
        this.isWithFlag = true;
        this.SetPowerUp(PowerUp.None);
        ninjaAnimation.TakeFlagAnimation();
    }

    public void SetDamage(float damage)
    {
        health = health > damage ? health - damage : 0;
        ninjaAnimation.DamageAnimation(damage);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        this.isAlive = false;
        ninjaAnimation.DieAnimation();
    }

    public void SetPowerUp(PowerUp powerUp)
    {
        this.powerUp = powerUp;
        ninjaAnimation.PowerUpAnimation(powerUp);
    }
}
