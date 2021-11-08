using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToRegisterScript : MonoBehaviour
{

    public Canvas loginCanvas;
    public Canvas registerCanvas;
    public void ReplaceToRegister()
    {
        registerCanvas.enabled = true;
        loginCanvas.enabled = false;
    }
}
