using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenScript : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTexts;

    public Image ninjaImage;

    public Image teamImage;

    public Image resultImage;

    public Sprite[] ninjaSprites;

    public Sprite[] teamSprites;

    public Sprite[] resultSprites;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetGameResult(float[] teamScores)
    {
        int intScoreBlue = Mathf.RoundToInt(teamScores[(int)Team.Blue]);
        int intScoreRed = Mathf.RoundToInt(teamScores[(int)Team.Red]);
        scoreTexts[(int)Team.Blue].text = intScoreBlue.ToString();
        scoreTexts[(int)Team.Red].text = intScoreRed.ToString();
        if (intScoreBlue == intScoreRed)
        {
            ninjaImage.sprite = null;
            teamImage.sprite = null;
            resultImage.sprite = resultSprites[(int)GameResult.Draw];
        }
        else if (intScoreBlue > intScoreRed)
        {
            ninjaImage.sprite = ninjaSprites[(int)Team.Blue];
            teamImage.sprite = teamSprites[(int)Team.Blue];
            resultImage.sprite = resultSprites[(int)GameResult.Win];
        }
        else
        {
            ninjaImage.sprite = ninjaSprites[(int)Team.Red];
            teamImage.sprite = teamSprites[(int)Team.Red];
            resultImage.sprite = resultSprites[(int)GameResult.Win];
        }
    }
}
