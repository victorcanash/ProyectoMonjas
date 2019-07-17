using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private void Awake()
    {
        if (!GameManager.Instance.singletons)
        {
            GameManager.Instance.init();
            //Constants.SetStartValues();
            //NeonInputManager.Instance.init();
            //LanguageManager.Instance.init();
            //LevelsManager.Instance.init();
            //UIManagerNeon.Instance.init();
            //SoundManager.Instance.init();
        }
    }
}
