using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript i;

    public TextMeshProUGUI timerText;

    public TextMeshProUGUI[] healthTexts;

    public TextMeshProUGUI[] scoreTexts;

    public GameObject ninja;

    public GameObject flag;

    public GameObject powerUp;

    public Transform map;

    public Transform[] ninjaStartPoints;

    public Transform[] flagStartPoints;

    public Transform powerUpStartPoint;

    public float powerUpPreAppearDelay;

    public float powerUpGenerateDelay;

    public float ninjaEliminationScore;

    public float flagConveyScore;

    public bool isEndless;

    public float gameTimeoutSec;

    public float rushModeTimeSec;

    public bool isRushMode;

    public float rushModeExtraSpeed;

    private float extraSpeed;

    private List<GameObject> gameObjects = new List<GameObject>();

    private float[] teamScores = new float[2];

    private Coroutine powerUpCoroutine;

    private Coroutine gameTimeoutCoroutine;

    private GameObject powerUpObj;

    private PlayerScript[] ninjas = new PlayerScript[2];

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
        for (int i = 0; i < ninjas.Length; i++)
        {
            if (ninjas[i] != null)
            {
                healthTexts[i].text = Mathf.RoundToInt(ninjas[i].health).ToString();
                scoreTexts[i].text = Mathf.RoundToInt(teamScores[i]).ToString();
            }
        }
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
                var playerObj = obj.GetComponent<PlayerScript>();
                playerObj.playerControls[(int)team].enabled = true;
                playerObj.speed += extraSpeed;
            });
    }

    private IEnumerator GameTimeout()
    {
        float gameTimeout = gameTimeoutSec;
        timerText.text = System.TimeSpan.FromSeconds(gameTimeout).ToString(@"mm\:ss");
        while (gameTimeout > 0)
        {
            yield return new WaitForSeconds(1);
            gameTimeout--;
            timerText.text = System.TimeSpan.FromSeconds(gameTimeout).ToString(@"mm\:ss");
            if (gameTimeout <= rushModeTimeSec && !isRushMode)
            {
                isRushMode = true;
                var players = GameObject.FindGameObjectsWithTag("Player");
                timerText.color = Color.red;
                extraSpeed = rushModeExtraSpeed;
                foreach (var player in players)
                {
                    player.GetComponent<PlayerScript>().speed += extraSpeed;
                }
            }
        }

        GameOver();
    }

    private IEnumerator PowerUpGenerator()
    {
        var powerUps = new PowerUp[] { PowerUp.Katana, PowerUp.SpeedUp, PowerUp.Immortality };
        int curPowerIndex = 0;
        yield return new WaitForSeconds(powerUpPreAppearDelay);
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
                    obj.GetComponent<PowerUpScript>().powerType = powerUps[curPowerIndex]; // (PowerUp)Random.Range(1, powerUpCount - 1);
                    curPowerIndex++;
                    if (curPowerIndex >= powerUps.Length)
                    {
                        curPowerIndex = 0;
                    }
                });
            yield return new WaitForSeconds(powerUpGenerateDelay);
        }
    }

    private GameObject CreateGameObject(GameObject prefab, Transform startPoint, Team team, SetupParamsFunc setupParamsFunc = null)
    {
        var obj = Instantiate(prefab, new Vector3(startPoint.position.x, startPoint.position.y, prefab.transform.position.z), Quaternion.identity, map);
        var dynObj = obj.GetComponent<IDynamicObject>();
        dynObj.Team = team;
        if (obj.CompareTag("Player"))
        {
            ninjas[(int)team] = obj.GetComponent<PlayerScript>();
        }

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

        timerText.text = "GAME OVER!";
        foreach (var gameObj in gameObjects)
        {
            Destroy(gameObj);
        }
    }
}
