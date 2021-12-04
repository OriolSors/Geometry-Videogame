using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLoginScript : MonoBehaviour
{
    public Canvas loginCanvas;
    public Canvas registerCanvas;
    public void ReplaceToLogin()
    {
        registerCanvas.enabled = false;
        loginCanvas.enabled = true;
    }
}
