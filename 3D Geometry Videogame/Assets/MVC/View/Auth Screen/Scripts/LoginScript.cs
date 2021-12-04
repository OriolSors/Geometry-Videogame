using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginScript : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField userName;

    private SceneController sceneController;

    void Start()
    {
        sceneController = GameObject.Find("Scene Controller").GetComponent<SceneController>();
    }

    public void GetAuth()
    {
        string name = userName.text;
        sceneController.LoadCorrectScene(name, ExistUser);
    }

    private void ExistUser(string advice)
    {

        userName.placeholder.GetComponent<TextMeshProUGUI>().text = advice;
        userName.placeholder.color = Color.red;
        userName.text = "";

    }

}
