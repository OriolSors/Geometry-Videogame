using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameSelectionScript : MonoBehaviour
{

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
        AuthController.Instance.SignOut();
        SceneManager.LoadScene("Auth Screen");
    }

    public void ToMissionSelection()
    {
        SceneManager.LoadScene("Player Mission List Screen");
    }

}
