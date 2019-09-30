using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagScript : MonoBehaviour, IDynamicObject
{
    public Sprite[] grabSprites;

    public float grabDurationSec = 1;

    public Sprite[] positionSprites;

    public float positionDurationSec = 1;

    public PlayerScript carrierPlayer;

    public bool isDropped;

    public Team Team { get; set; }

    private ObjectScript objectScript;

    void Start()
    {
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

        Utils.MakeAnimation(objectScript, positionDurationSec, positionSprites, true, () =>
        {
            Utils.MakeAnimation(objectScript, objectScript.durationSec, objectScript.sprites);
        });
    }

    public void Grab(PlayerScript carrierPlayer)
    {
        Debug.Log($"{this.Team} flag was grabbed by {carrierPlayer.Team} ninja!");
        this.isDropped = false;
        this.carrierPlayer = carrierPlayer;
        Utils.MakeAnimation(objectScript, grabDurationSec, grabSprites);
        gameObject.SetActive(false);
        transform.parent = carrierPlayer.transform;
        transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
    }

    public void Drop(Vector3 position)
    {
        Debug.Log($"{this.Team} flag was dropped!");
        this.isDropped = true;
        this.SetPosition(position);
    }

    public void Convey()
    {
        Debug.Log($"{this.Team} flag was conveyed!");
        GameManagerScript.i.AddScores(ScoreOption.FlagConvey, carrierPlayer.Team);
        this.SetPosition(GameManagerScript.i.flagStartPoints[(int)this.Team].position);
    }

    public void Return()
    {
        Debug.Log($"{this.Team} flag was returned!");
        this.SetPosition(GameManagerScript.i.flagStartPoints[(int)this.Team].position);
    }

    private void SetPosition(Vector3 position)
    {
        gameObject.SetActive(false);
        this.isDropped = false;
        transform.parent = GameManagerScript.i.map;
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        carrierPlayer = null;
        gameObject.SetActive(true);
        Utils.MakeAnimation(objectScript, positionDurationSec, positionSprites, true, () =>
        {
            Utils.MakeAnimation(objectScript, objectScript.durationSec, objectScript.sprites);
        });
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
                    } else if (player.carriedFlag != null)
                    {
                        player.carriedFlag.Convey();
                    }
                }
            }
        }
    }
}
