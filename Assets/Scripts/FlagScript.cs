using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagScript : MonoBehaviour, IDynamicObject
{
    public Sprite[] onBaseSprites;

    public Sprite[] droppedSprites;

    public float spritesDurationSec;

    public Sprite[] grabSprites;

    public float grabDurationSec = 1;

    public Sprite[] positionSprites;

    public float positionDurationSec = 1;

    public PlayerScript carrierPlayer;

    public FlagStartPointScript startPointScript;

    public ObjectScript objectScript;

    public bool isDropped;

    public float playerSpeedReduceWhenGrabbed;

    public Team team;

    public Team Team
    {
        get { return team; }
        set { team = value; }
    }

    void Start()
    {
        //Utils.MakeAnimation(objectScript, positionDurationSec, positionSprites);
        Utils.MakeAnimation(objectScript, spritesDurationSec, onBaseSprites);
        startPointScript.ChangeAnimation(false);
    }

    void Update()
    {
    }

    public void Init()
    {
        Debug.Log($"{this.Team} flag was created!");
        objectScript = GetComponent<ObjectScript>();
        carrierPlayer = null;
        this.isDropped = false;
    }

    public void Grab(PlayerScript carrierPlayer)
    {
        Debug.Log($"{this.Team} flag was grabbed by {carrierPlayer.Team} ninja!");
        this.isDropped = false;
        this.carrierPlayer = carrierPlayer;
        //Utils.MakeAnimation(objectScript, grabDurationSec, grabSprites);
        gameObject.SetActive(false);
        transform.parent = carrierPlayer.transform;
        transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
        startPointScript.ChangeAnimation(false);
    }

    public void Drop(Vector3 position)
    {
        Debug.Log($"{this.Team} flag was dropped!");
        this.isDropped = true;
        this.SetPosition(position, false);
        Utils.MakeAnimation(objectScript, spritesDurationSec, droppedSprites);
    }

    public void Convey()
    {
        Debug.Log($"{this.Team} flag was conveyed!");
        this.isDropped = false;
        GameManagerScript.i.AddScores(ScoreOption.FlagConvey, carrierPlayer.Team);
        this.SetPosition(startPointScript.transform.position);
        Utils.MakeAnimation(objectScript, spritesDurationSec, onBaseSprites);
    }

    public void Return()
    {
        Debug.Log($"{this.Team} flag was returned!");
        this.isDropped = false;
        this.SetPosition(startPointScript.transform.position);
        Utils.MakeAnimation(objectScript, spritesDurationSec, onBaseSprites);
    }

    private void SetPosition(Vector3 position, bool isStartPoint = true)
    {
        gameObject.SetActive(false);
        transform.parent = GameManagerScript.i.map;
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        carrierPlayer = null;
        gameObject.SetActive(true);
        if (isStartPoint)
        {
            startPointScript.ChangeAnimation(true);
        }

        //Utils.MakeAnimation(objectScript, positionDurationSec, positionSprites);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerScript>();
            if (player.isAlive)
            {
                if (player.Team != this.Team && carrierPlayer == null)
                {
                    player.TakeFlag(this);
                    this.Grab(player);
                } else if (player.Team == this.Team)
                {
                    if (this.isDropped)
                    {
                        this.Return();
                    }
                }
            }
        }
    }
}
