using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    public Texture2D mira;
    void OnGUI()
    {
        
        float xMin = (Screen.width / 2) - (mira.width / 2);
        float yMin = (Screen.height / 2) - (mira.height / 2);

        GUI.DrawTexture(new Rect(xMin, yMin, mira.width, mira.height), mira);
    }

    void Update()
    {
        Cursor.visible = false;
    }
}
