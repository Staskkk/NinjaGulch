using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagScript : MonoBehaviour
{
    public Team team;

    public Sprite[] grabSprites;

    public float grabDurationSec = 1;

    public Sprite[] positionSprites;

    public float positionDurationSec = 1;

    public bool isGrabbed;

    private ObjectScript objectScript;

    void Start()
    {
        objectScript = GetComponent<ObjectScript>();
        isGrabbed = false;
    }

    void Update()
    {
    }

    public void Grab()
    {
        if (!isGrabbed)
        {
            isGrabbed = true;
            Utils.MakeAnimation(objectScript, grabDurationSec, grabSprites, true, () =>
            {
                objectScript.spriteRenderer.enabled = false;
            });
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
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
            if (player.isAlive && player.team != this.team && !this.isGrabbed)
            {
                Debug.Log($"{player.team} ninja takes the flag!");
                player.TakeFlag();
                this.Grab();
            }
        }
    }
}
