using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameSelectionScript : MonoBehaviour
{

    //TODO: passar les dades de la pantalla de Mission List Player o llegir directament de la firebase. Amb aixo actualitzem els percentatges o la posicio de les figures
    //restants, aixi com ajustar l'algorisme dins els minijocs

    public void ToCollect()
    {
        SceneManager.LoadScene("Game Collect");
    }

    public void ToTatami()
    {
        SceneManager.LoadScene("Game Tatami");
    }

    public void ToFootball()
    {
        SceneManager.LoadScene("Game Football");
    }

    public void ToLogin()
    {
        SceneManager.LoadScene("Auth Screen");
    }

    public void ToMissionSelection()
    {
        SceneManager.LoadScene("Player Mission List Screen");
    }

}
