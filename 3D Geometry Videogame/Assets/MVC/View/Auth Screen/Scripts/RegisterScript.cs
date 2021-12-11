using UnityEngine;
using TMPro;

public class RegisterScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameInput;

    [SerializeField]
    private TMP_InputField emailInput;

    [SerializeField]
    private TMP_InputField passwordInput;

    [SerializeField]
    private TMP_Dropdown accountTypeOption;

    private AuthManager authManager;

    public Canvas errorCanvas;

    private void Start()
    {
        errorCanvas.enabled = false;
        authManager = GameObject.Find("Scene Controller").GetComponent<AuthManager>();
    }

    public async void RegisterAndCreateNewUser()
    {
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string accountType = accountTypeOption.options[accountTypeOption.value].text;

        await authManager.RegisterAndCreateNewUser(username, email, password, accountType, GetNegativeResultOfUserCreation);

    }

    private void GetNegativeResultOfUserCreation(string message)
    {
        errorCanvas.enabled = true;
        errorCanvas.GetComponentInChildren<TextMeshProUGUI>().text = message;
        
    }

    public void HideErrorCanvas()
    {
        errorCanvas.enabled = false;
    }


}
