using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript i;

    public GameObject LogoScreen;

    public GameObject StartScreen;

    public GameObject Instruction1Screen;

    public GameObject Instruction2Screen;

    public GameObject WinScreen;

    public TextMeshProUGUI timerText;

    public Image[] healthHearts;

    public TextMeshProUGUI[] scoreTexts;

    public GameObject ninjaBlue;

    public GameObject ninjaRed;

    public GameObject flagBlue;

    public GameObject flagRed;

    public GameObject powerUp;

    public Transform map;

    public Transform[] ninjaStartPoints;

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

    public float logoScreenDurationSec;

    public KeyCode keyToContinue;

    public KeyCode keyToPlayAgain;

    public KeyCode keyToExit;

    public bool isGameOver;

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
        LogoScreen.SetActive(true);
        StartCoroutine(LogoScreenCoroutine());
    }

    private IEnumerator LogoScreenCoroutine()
    {
        yield return new WaitForSeconds(logoScreenDurationSec);
        LogoScreen.SetActive(false);
        StartScreen.SetActive(true);
    }

    void Update()
    {
        if (LogoScreen.activeSelf)
        {
            return;
        }

        if (StartScreen.activeSelf && Input.GetKeyDown(keyToContinue))
        {
            StartScreen.SetActive(false);
            Instruction1Screen.SetActive(true);
        }
        else if (Instruction1Screen.activeSelf && Input.GetKeyDown(keyToContinue))
        {
            Instruction1Screen.SetActive(false);
            Instruction2Screen.SetActive(true);
        }
        else if (Instruction2Screen.activeSelf && Input.GetKeyDown(keyToContinue))
        {
            Instruction2Screen.SetActive(false);
            GameStart();
        }
        else if (!this.isGameOver)
        {
            for (int i = 0; i < ninjas.Length; i++)
            {
                if (ninjas[i] != null)
                {
                    healthHearts[i].fillAmount = ninjas[i].health / ninjas[i].maxHealth;
                    scoreTexts[i].text = Mathf.RoundToInt(teamScores[i]).ToString();
                }
            }
        }
        else if (Input.GetKeyDown(keyToPlayAgain))
        {
            GameRestart();
        }
        else if (Input.GetKeyDown(keyToExit))
        {
            GameExit();
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
        map.gameObject.SetActive(true);
        WinScreen.SetActive(false);
        isGameOver = false;
        teamScores = new float[2];
        ReturnFlags();
        RepairTraps();
        CreateNinja(Team.Blue);
        CreateNinja(Team.Red);
        powerUpCoroutine = StartCoroutine(PowerUpGenerator());
        if (!isEndless)
        {
            gameTimeoutCoroutine = StartCoroutine(GameTimeout());
        }
    }
    
    public void RepairTraps()
    {
        var traps = GameObject.FindGameObjectsWithTag("Trap");
        foreach (var trap in traps)
        {
            var trapScript = trap.GetComponent<DissolveTrapScript>();
            if (trapScript != null)
            {
                trapScript.Repair();
            }
        }
    }

    public void ReturnFlags()
    {
        var flags = GameObject.FindGameObjectsWithTag("Flag");
        foreach (var flag in flags)
        {
            var flagScript = flag.GetComponent<FlagScript>();
            if (flagScript != null)
            {
                flagScript.Return();
            }
        }
    }

    public GameObject CreateNinja(Team team)
    {
        var ninja = team == Team.Blue ? ninjaBlue : ninjaRed;
        return CreateGameObject(ninja, ninjaStartPoints[(int)team],
            (obj) =>
            {
                var playerObj = obj.GetComponent<PlayerScript>();
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

            powerUpObj = CreateGameObject(powerUp, powerUpStartPoint,
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

    private GameObject CreateGameObject(GameObject prefab, Transform startPoint, SetupParamsFunc setupParamsFunc = null)
    {
        var obj = Instantiate(prefab, new Vector3(startPoint.position.x, startPoint.position.y, prefab.transform.position.z), Quaternion.identity, map);
        var dynObj = obj.GetComponent<IDynamicObject>();
        if (obj.CompareTag("Player"))
        {
            ninjas[(int)dynObj.Team] = obj.GetComponent<PlayerScript>();
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
        this.isGameOver = true;
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

        WinScreen.GetComponent<WinScreenScript>().SetGameResult(teamScores);
        WinScreen.SetActive(true);
        map.gameObject.SetActive(false);
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
