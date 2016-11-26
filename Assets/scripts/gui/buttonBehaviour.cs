﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttonBehaviour : MonoBehaviour {

    public void quitGame()
    {
        Application.Quit();
    }

    public void enterCredits()
    {
        SceneManager.LoadScene("scn_credits");
    }

    public void startGame()
    {
        SceneManager.LoadScene("scn_level");
    }
}
