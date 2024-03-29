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

    public float ressurectDelay;

    public float damage;

    public float bonusDamage;

    public FlagScript carriedFlag;

    public bool isImmortal;

    public bool isRangeAttack;

    public float speed;

    public float bonusSpeed;

    public bool isAlive = true;

    public bool isInDeathCondition;

    public float startImmortalDuration;

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
        if (startImmortalDuration > 0)
        {
            this.SetPowerUp(PowerUp.Immortality, startImmortalDuration);
        }
    }

    void Update()
    {
        if (this.isInDeathCondition && !playerAttack.isHitting && this.isAlive)
        {
            this.Die();
        }
    }

    public void Destroy()
    {
        Debug.Log($"{this.Team} ninja object was destroyed!");
        Object.Destroy(gameObject);
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
        if (this.isImmortal)
        {
            damage = 0;
        }

        Debug.Log($"{this.Team} ninja was hit, damage: {damage}!");
        isDied = false;
        if (this.isImmortal)
        {
            return;
        }

        health = health > damage ? health - damage : 0;
        ninjaAnimation.DamageAnimation(damage);
        if (health <= 0)
        {
            isDied = true;
            isInDeathCondition = true;
        }
    }

    public void Die()
    {
        Debug.Log($"{this.Team} ninja died!");
        this.isAlive = false;
        if (carriedFlag != null)
        {
            this.speed += this.carriedFlag.playerSpeedReduceWhenGrabbed;
            carriedFlag.Drop(transform.position);
            carriedFlag = null;
        }

        ninjaAnimation.DieAnimation();
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(ressurectDelay);
        this.Destroy();
        if (autoResurrect)
        {
            this.Resurrect();
        }
    }

    public void Resurrect()
    {
        Debug.Log($"{this.Team} ninja resurrects!");
        GameManagerScript.i.CreateNinja(this.Team);
    }

    public void SetPowerUp(PowerUp powerType, float? powerDuration = null, float? powerParam = null)
    {
        this.powerUpDuration = powerDuration;
        this.powerUp = powerType;
        Debug.Log($"{this.Team} ninja gets power-up {this.powerUp}, duration: {this.powerUpDuration}!");
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }

        bonusSpeed = 0;
        bonusDamage = 0;
        isRangeAttack = false;
        isImmortal = false;
        switch (this.powerUp)
        {
            case PowerUp.SpeedUp:
                bonusSpeed += powerParam.Value;
                powerUpCoroutine = StartCoroutine(powerUpRoutine(this.powerUpDuration.Value));
                break;
            case PowerUp.Katana:
                bonusDamage += powerParam.Value;
                powerUpCoroutine = StartCoroutine(powerUpRoutine(this.powerUpDuration.Value));
                break;
            case PowerUp.Shurikens:
                bonusDamage = 1;
                isRangeAttack = true;
                powerUpCoroutine = StartCoroutine(powerUpRoutine(this.powerUpDuration.Value));
                break;
            case PowerUp.Immortality:
                isImmortal = true;
                powerUpCoroutine = StartCoroutine(powerUpRoutine(this.powerUpDuration.Value));
                break;
        }

        ninjaAnimation.PowerUpAnimation(powerUp);
    }

    private IEnumerator powerUpRoutine(float powerDuration)
    {
        yield return new WaitForSeconds(powerDuration);
        this.SetPowerUp(PowerUp.None);
    }
}
