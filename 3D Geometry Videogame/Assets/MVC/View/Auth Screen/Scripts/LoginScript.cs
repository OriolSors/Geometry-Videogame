using TMPro;
using UnityEngine;

public class LoginScript : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField emailInput;

    [SerializeField]
    private TMP_InputField passwordInput;

    private SceneController sceneController;

    public Canvas errorCanvas;

    void Start()
    {
        errorCanvas.enabled = false;
        sceneController = GameObject.Find("Scene Controller").GetComponent<SceneController>();
    }

    public void LoginUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        sceneController.LoginUser(email, password, GetNegativeResultOfUserLogged);

    }

    private void GetNegativeResultOfUserLogged(string message)
    {
        errorCanvas.enabled = true;
        errorCanvas.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }

    public void HideErrorCanvas()
    {
        errorCanvas.enabled = false;
    }
}
