using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    public Text _Text;
    private static float fps;

    void Update()
    {
        fps = 1.0f / Time.deltaTime;
        _Text.text ="FPS : " + ((int)fps).ToString();
    }
    
}