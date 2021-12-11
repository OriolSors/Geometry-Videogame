using TMPro;
using UnityEngine;

public class LoginScript : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField emailInput;

    [SerializeField]
    private TMP_InputField passwordInput;

    private AuthManager authManager;

    public Canvas errorCanvas;

    void Start()
    {
        errorCanvas.enabled = false;
        authManager = GameObject.Find("Scene Controller").GetComponent<AuthManager>();
    }

    public async void LoginUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        await authManager.LoginUser(email, password, GetNegativeResultOfUserLogged);

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
