﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour, IDynamicObject
{
    public NinjaAnimationScript ninjaAnimation;

    public PlayerAttackScript playerAttack;

    public bool autoResurrect = true;

    public float health;

    public float maxHealth;

    public float damage;

    public float bonusDamage;

    public FlagScript carriedFlag;

    public bool isImmortal;

    public bool isRangeAttack;

    public float speed;

    public float bonusSpeed;

    public bool isAlive = true;

    public PowerUp powerUp;

    public float? powerUpDuration;

    public Team team;

    public Team Team
    {
        get { return team; }
        set { team = value; }
    }

    public float TotalSpeed
    {
        get { return this.speed + this.bonusSpeed; }
    }

    private Coroutine powerUpCoroutine;

    void Start()
    {
    }

    void Update()
    {
        if (!this.isAlive && !playerAttack.isHitting)
        {
            this.Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{this.Team} ninja died!");
        if (carriedFlag != null)
        {
            this.speed += this.carriedFlag.playerSpeedReduceWhenGrabbed;
            carriedFlag.Drop(transform.position);
            carriedFlag = null;
        }

        Object.Destroy(gameObject);
        if (autoResurrect)
        {
            this.Resurrect();
        }
    }

    public void Init()
    {
        Debug.Log($"{this.Team} ninja was created!");
        isAlive = true;
        health = maxHealth;
        carriedFlag = null;
        powerUp = PowerUp.None;

        ninjaAnimation.ResurrectAnimation();
    }

    public void TakeFlag(FlagScript flag)
    {
        Debug.Log($"{this.Team} ninja takes the flag!");
        this.carriedFlag = flag;
        this.speed -= flag.playerSpeedReduceWhenGrabbed;
        this.SetPowerUp(null);
        ninjaAnimation.TakeFlagAnimation();
    }

    public void ConveyFlag()
    {
        this.speed += this.carriedFlag.playerSpeedReduceWhenGrabbed;
        this.carriedFlag.Convey();
        this.carriedFlag = null;
        ninjaAnimation.ConveyFlagAnimation();
    }

    public void SetDamage(float damage, out bool isDied)
    {
        isDied = false;
        if (this.isImmortal)
        {
            damage = 0;
        }

        Debug.Log($"{this.Team} ninja was hit, damage: {damage}!");
        health = health > damage ? health - damage : 0;
        //playerHealthTexts[(int)Team].text = Mathf.RoundToInt(health).ToString();
        ninjaAnimation.DamageAnimation(damage);
        if (health <= 0)
        {
            isDied = true;
            SetDeathCondition();
        }
    }

    public void SetDeathCondition()
    {
        Debug.Log($"{this.Team} ninja in death condition!");
        this.isAlive = false;
        ninjaAnimation.DieAnimation();
    }

    public void Resurrect()
    {
        Debug.Log($"{this.Team} ninja resurrects!");
        GameManagerScript.i.CreateNinja(this.Team);
    }

    public void SetPowerUp(PowerUpScript powerUpScript, float? powerDuration = null)
    {
        this.powerUpDuration = powerDuration;
        this.powerUp = powerUpScript?.powerType ?? PowerUp.None;
        Debug.Log($"{this.Team} ninja gets power-up {this.powerUp}, duration: {powerDuration}!");
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }

        switch (this.powerUp)
        {
            case PowerUp.SpeedUp:
                bonusSpeed += powerUpScript.bonusSpeed;
                powerUpCoroutine = StartCoroutine(powerUpRoutine(powerDuration.Value));
                break;
            case PowerUp.Katana:
                bonusDamage += powerUpScript.bonusDamage;
                powerUpCoroutine = StartCoroutine(powerUpRoutine(powerDuration.Value));
                break;
            case PowerUp.Shurikens:
                bonusDamage = 1;
                isRangeAttack = true;
                powerUpCoroutine = StartCoroutine(powerUpRoutine(powerDuration.Value));
                break;
            case PowerUp.Immortality:
                isImmortal = true;
                powerUpCoroutine = StartCoroutine(powerUpRoutine(powerDuration.Value));
                break;
            case PowerUp.None:
                bonusSpeed = 0;
                bonusDamage = 0;
                isRangeAttack = false;
                isImmortal = false;
                break;
        }

        ninjaAnimation.PowerUpAnimation(powerUp);
    }

    private IEnumerator powerUpRoutine(float powerDuration)
    {
        yield return new WaitForSeconds(powerDuration);
        this.SetPowerUp(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }
}
