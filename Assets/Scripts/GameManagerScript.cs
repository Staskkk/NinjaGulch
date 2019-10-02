using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript i;

    public GameObject ninja;

    public GameObject flag;

    public GameObject powerUp;

    public Transform map;

    public Transform[] ninjaStartPoints;

    public Transform[] flagStartPoints;

    public Transform powerUpStartPoint;

    public float powerUpGenerateDelay;

    public float ninjaEliminationScore;

    public float flagConveyScore;

    public bool isEndless;

    public float gameTimeoutSec;

    private List<GameObject> gameObjects = new List<GameObject>();

    private float[] teamScores = new float[2];

    private Coroutine powerUpCoroutine;

    private Coroutine gameTimeoutCoroutine;

    private GameObject powerUpObj;

    private void Awake()
    {
        i = this;
    }

    void Start()
    {
        GameStart();
    }

    void Update()
    {
    }

    public void AddScores(ScoreOption scoreOption, Team team)
    {
        switch (scoreOption)
        {
            case ScoreOption.NinjaElimination:
                teamScores[(int)team] += ninjaEliminationScore;
                break;
            case ScoreOption.FlagConvey:
                teamScores[(int)team] += flagConveyScore;
                break;
        }
    }

    public void GameStart()
    {
        GameOver();
        teamScores = new float[2];
        CreateNinja(Team.Red);
        CreateNinja(Team.Blue);
        CreateGameObject(flag, flagStartPoints[(int)Team.Red], Team.Red);
        CreateGameObject(flag, flagStartPoints[(int)Team.Blue], Team.Blue);
        powerUpCoroutine = StartCoroutine(PowerUpGenerator());
        if (!isEndless)
        {
            gameTimeoutCoroutine = StartCoroutine(GameTimeout());
        }
    }
    
    public GameObject CreateNinja(Team team)
    {
        return CreateGameObject(ninja, ninjaStartPoints[(int)team], team,
            (obj) =>
            {
                obj.GetComponent<PlayerScript>().playerControls[(int)team].enabled = true;
            });
    }

    private IEnumerator GameTimeout()
    {
        float gameTimeout = gameTimeoutSec;
        while (gameTimeout > 0)
        {
            yield return new WaitForSeconds(1);
            gameTimeout--;
        }

        GameOver();
    }

    private IEnumerator PowerUpGenerator()
    {
        while (true)
        {
            if (powerUpObj != null)
            {
                Destroy(powerUpObj);
            }

            powerUpObj = CreateGameObject(powerUp, powerUpStartPoint, Team.None,
                (obj) =>
                {
                    int powerUpCount = System.Enum.GetNames(typeof(PowerUp)).Length;
                    obj.GetComponent<PowerUpScript>().powerType = (PowerUp)Random.Range(1, powerUpCount - 1);
                });
            yield return new WaitForSeconds(powerUpGenerateDelay);
        }
    }

    private GameObject CreateGameObject(GameObject prefab, Transform startPoint, Team team, SetupParamsFunc setupParamsFunc = null)
    {
        var obj = Instantiate(prefab, new Vector3(startPoint.position.x, startPoint.position.y, prefab.transform.position.z), Quaternion.identity, map);
        var dynObj = obj.GetComponent<IDynamicObject>();
        dynObj.Team = team;
        setupParamsFunc?.Invoke(obj);
        gameObjects.Add(obj);
        obj.SetActive(true);
        dynObj.Init();
        return obj;
    }

    public void GameRestart()
    {
        GameOver();
        GameStart();
    }

    public void GameOver()
    {
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }

        if (gameTimeoutCoroutine != null)
        {
            StopCoroutine(gameTimeoutCoroutine);
        }

        foreach (var gameObj in gameObjects)
        {
            Destroy(gameObj);
        }
    }
}
