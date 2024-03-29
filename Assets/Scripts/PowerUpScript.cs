﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour, IDynamicObject
{
    public PowerUp powerType;

    public float powerDuration;

    public float bonusSpeed;

    public float bonusDamage;

    public Sprite[] grabSprites;

    public float grabDurationSec = 1;

    public Sprite[] positionSprites;

    public float positionDurationSec = 1;

    public Sprite[] katanaSprites;
    public Sprite[] immortalitySprites;
    public Sprite[] speedUpSprites;
    public Sprite[] shurikensSprites;

    public float powerUpsDurationSec;

    public Team Team { get; set; } = Team.None;

    public float? PowerParam
    {
        get
        {
            switch (powerType)
            {
                case PowerUp.Katana:
                    return bonusDamage;
                case PowerUp.SpeedUp:
                    return bonusSpeed;
                default:
                    return null;
            }
        }
    }

    private ObjectScript objectScript;

    void Start()
    {
        objectScript = GetComponent<ObjectScript>();

        //Utils.MakeAnimation(objectScript, positionDurationSec, positionSprites, true);
        switch (powerType)
        {
            case PowerUp.Katana:
                Utils.MakeAnimation(objectScript, powerUpsDurationSec, katanaSprites);
                break;
            case PowerUp.Immortality:
                Utils.MakeAnimation(objectScript, powerUpsDurationSec, immortalitySprites);
                break;
            case PowerUp.SpeedUp:
                Utils.MakeAnimation(objectScript, powerUpsDurationSec, speedUpSprites);
                break;
            case PowerUp.Shurikens:
                Utils.MakeAnimation(objectScript, powerUpsDurationSec, shurikensSprites);
                break;
        }
    }

    void Update()
    {
    }

    public void Init()
    {
        Debug.Log($"{this.powerType} power-up was created!");
    }

    public void Grab()
    {
        Debug.Log($"{this.powerType} power-up was grabbed!");
        //Utils.MakeAnimation(objectScript, grabDurationSec, grabSprites);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            if (player.isAlive)
            {
                player.SetPowerUp(this.powerType, this.powerDuration, this.PowerParam);
                this.Grab();
            }
        }
    }
}
