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

    private void ExistUser(bool exist, string advice)
    {
        if (!exist)
        {
            userName.placeholder.GetComponent<TextMeshProUGUI>().text = "User not exists";
            userName.placeholder.color = Color.red;
            userName.text = "";
        }
    }

}
