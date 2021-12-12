using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{

    private AuthController authController;


    private void Start()
    {
        authController = AuthController.Instance;
    }

    public async Task RegisterAndCreateNewUser(string username, string email, string password, string accountType, Action<string> GetNegativeResultOfUserCreation)
    {
        await authController.RegisterNewUser(username, email, password, GetNegativeResultOfUserCreation);
        authController.CreateNewUser(username, email, accountType);
        await authController.SetCurrentUser(email, ConfirmUserLogged);
        return;
    }

    public async Task LoginUser(string email, string password, Action<string> GetNegativeResultOfUserLogged)
    {
        await authController.LoginUser(email, password, GetNegativeResultOfUserLogged);
        await authController.SetCurrentUser(email, ConfirmUserLogged);
        return;
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
