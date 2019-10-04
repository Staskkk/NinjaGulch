﻿using System.Collections;
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

    void Start()
    {
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
        float panelWidth = leftPanel.rect.width;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        canvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldScreenWidth);
        canvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, worldScreenHeight);

        Vector3 imgScale = new Vector3(1f, 1f, 1f);

        float fullScaleX = worldScreenWidth * 0.801f / width;
        float fullScaleY = worldScreenHeight / height;

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

        Vector3 panelScale = new Vector3(worldScreenWidth * 0.09375f / panelWidth, 1, 1);
        leftPanel.localScale = panelScale;
        rightPanel.localScale = panelScale;
        transform.localScale = imgScale;
    }
}