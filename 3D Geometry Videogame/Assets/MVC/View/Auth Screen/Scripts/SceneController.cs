using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    private AuthController authController;


    private void Start()
    {
        authController = AuthController.Instance;
    }

    public void RegisterAndCreateNewUser(string username, string email, string password, string accountType, Action<string> GetNegativeResultOfUserCreation)
    {
        authController.RegisterNewUser(username, email, password, accountType, ConfirmUserLogged, GetNegativeResultOfUserCreation);
    }

    public void LoginUser(string email, string password, Action<string> GetNegativeResultOfUserLogged)
    {
        authController.LoginUser(email, password, ConfirmUserLogged, GetNegativeResultOfUserLogged);
    }

    private void ConfirmUserLogged(string accountType)
    {
        switch (accountType)
        {
            case "Designer":
                SceneManager.LoadScene("Designer Mission List Screen");
                break;
            case "Player":
                SceneManager.LoadScene("Player Mission List Screen");
                break;
        }
    }

    public void ExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
