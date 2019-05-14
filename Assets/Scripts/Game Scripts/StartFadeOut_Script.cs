using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartFadeOut_Script : MonoBehaviour 
{
    public Image startFadeInImage;
    public Text startFadeInText;
    public GameObject startCanvas;

   void Start()
   {
        startFadeInImage.GetComponent<Image>().CrossFadeAlpha(0.0f, 15.0f, false);
        startFadeInText.GetComponent<Text>().CrossFadeAlpha(0.0f, 3.0f, false);

        Destroy(startCanvas, 10);

    }
}
