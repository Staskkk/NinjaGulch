using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour, IDynamicObject
{
    public PlayerControlScript[] playerControls;

    //public Text[] playerHealthTexts;

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

    public Team Team { get; set; }

    private NinjaAnimationScript ninjaAnimation;

    private Coroutine powerUpCoroutine;

    void Start()
    {
        //playerHealthTexts[(int)Team].text = Mathf.RoundToInt(health).ToString();
    }

    void Update()
    { 
    }

    public void Init()
    {
        Debug.Log($"{this.Team} ninja was created!");
        ninjaAnimation = GetComponent<NinjaAnimationScript>();
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
        this.SetPowerUp(PowerUp.None);
        ninjaAnimation.TakeFlagAnimation();
    }

    public void ConveyFlag()
    {
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
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{this.Team} ninja died!");
        this.isAlive = false;
        ninjaAnimation.DieAnimation();
        if (carriedFlag != null)
        {
            carriedFlag.Drop(transform.position);
            carriedFlag = null;
        }

        Object.Destroy(gameObject);
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

    public void SetPowerUp(PowerUp powerUp, float? powerDuration = null)
    {
        Debug.Log($"{this.Team} ninja gets power-up {powerUp}, duration: {powerDuration}!");
        this.powerUp = powerUp;
        switch (powerUp)
        {
            case PowerUp.SpeedUp:
                bonusSpeed = 1;
                powerUpCoroutine = StartCoroutine(powerUpRoutine(powerDuration.Value));
                break;
            case PowerUp.Katana:
                bonusDamage = 1;
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
                if (powerUpCoroutine != null)
                {
                    StopCoroutine(powerUpCoroutine);
                }

                break;
        }

        ninjaAnimation.PowerUpAnimation(powerUp);
    }

    private IEnumerator powerUpRoutine(float powerDuration)
    {
        yield return new WaitForSeconds(powerDuration);
        this.SetPowerUp(PowerUp.None);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }
}
