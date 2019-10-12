using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleScript : MonoBehaviour
{
    public float width;

    public float height;
    
    public bool executeOnUpdate;

    public RectTransform canvas;

    public RectTransform leftPanel;

    public RectTransform rightPanel;

    public RectTransform timer;

    private float correctScreenRatioDiff;

    private float correctScreenRatio;

    void Start()
    {
        float correctScreenWidth = width + leftPanel.rect.width + rightPanel.rect.width;
        correctScreenRatioDiff = correctScreenWidth - height;
        correctScreenRatio = correctScreenWidth / height;
        Resize(); 
    }

    void FixedUpdate()
    {
        if (executeOnUpdate)
        {
            Resize();
        }
    }

    void Resize()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        canvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldScreenWidth);
        canvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, worldScreenHeight);

        Vector3 imgScale = new Vector3(1f, 1f, 1f);

        float mapWidth = worldScreenWidth * 0.801f;
        float mapHeight = worldScreenHeight;

        float fullScaleX = mapWidth / width;
        float fullScaleY = mapHeight / height;

        float scale = transform.localScale.x;
        float bias = (worldScreenWidth - worldScreenHeight - correctScreenRatioDiff) / 2;
        float xBias = bias < 0 ? 0 : bias;
        float yBias = bias > 0 ? 0 : bias;

        if (fullScaleX < fullScaleY)
        {
            imgScale.x = fullScaleX;
            imgScale.y = fullScaleX;
        }
        else
        {
            imgScale.x = fullScaleY;
            imgScale.y = fullScaleY;
        }

        leftPanel.localScale = imgScale;
        leftPanel.anchoredPosition = new Vector2(xBias, leftPanel.anchoredPosition.y);
        rightPanel.localScale = imgScale;
        rightPanel.anchoredPosition = new Vector2(-xBias, rightPanel.anchoredPosition.y);
        timer.localScale = imgScale;
        timer.anchoredPosition = new Vector2(timer.anchoredPosition.x, yBias / correctScreenRatio);
        transform.localScale = imgScale;
    }
}
