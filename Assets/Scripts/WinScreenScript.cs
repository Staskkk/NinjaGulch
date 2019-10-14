using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenScript : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTexts;

    public Image congratulationImage;

    public Image ninjaImage;

    public Image teamImage;

    public Image resultImage;

    public Image tieBlueNinjaImage;

    public Image tieRedNinjaImage;

    public Image tieResultImage;

    public Sprite[] congratulationSprites;

    public Sprite[] ninjaSprites;

    public Sprite[] teamSprites;


    public void SetGameResult(float[] teamScores)
    {
        ninjaImage.gameObject.SetActive(true);
        teamImage.gameObject.SetActive(true);
        resultImage.gameObject.SetActive(true);
        tieBlueNinjaImage.gameObject.SetActive(false);
        tieRedNinjaImage.gameObject.SetActive(false);
        tieResultImage.gameObject.SetActive(false);
        int intScoreBlue = Mathf.RoundToInt(teamScores[(int)Team.Blue]);
        int intScoreRed = Mathf.RoundToInt(teamScores[(int)Team.Red]);
        scoreTexts[(int)Team.Blue].text = intScoreBlue.ToString();
        scoreTexts[(int)Team.Red].text = intScoreRed.ToString();
        if (intScoreBlue == intScoreRed)
        {
            congratulationImage.sprite = congratulationSprites[(int)GameResult.Draw];
            ninjaImage.gameObject.SetActive(false);
            teamImage.gameObject.SetActive(false);
            resultImage.gameObject.SetActive(false);
            tieBlueNinjaImage.gameObject.SetActive(true);
            tieRedNinjaImage.gameObject.SetActive(true);
            tieResultImage.gameObject.SetActive(true);
        }
        else if (intScoreBlue > intScoreRed)
        {
            congratulationImage.sprite = congratulationSprites[(int)GameResult.Win];
            ninjaImage.sprite = ninjaSprites[(int)Team.Blue];
            teamImage.sprite = teamSprites[(int)Team.Blue];
        }
        else
        {
            congratulationImage.sprite = congratulationSprites[(int)GameResult.Win];
            ninjaImage.sprite = ninjaSprites[(int)Team.Red];
            teamImage.sprite = teamSprites[(int)Team.Red];
        }
    }
}
