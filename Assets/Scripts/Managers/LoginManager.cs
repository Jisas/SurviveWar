using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEditor;
using GooglePlayGames;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private string clientScene;

    [Header("Panels")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject recoveryPanel;

    [Header("Login")]
    [SerializeField] private TMP_InputField emailLoginInput;
    [SerializeField] private TMP_InputField paswordLoginInput;

    [Header("Register")]
    [SerializeField] private TMP_InputField userNameRegisterInput;
    [SerializeField] private TMP_InputField emailRegisterInput;
    [SerializeField] private TMP_InputField paswordRegisterInput;
    [SerializeField] private TMP_InputField confimPasswordRegisterInput;

    [Header("Login")]
    [SerializeField] private TMP_InputField emailRecoveryInput;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI messageText;

    public void RegisterUser()
    {
        if (!string.IsNullOrEmpty(userNameRegisterInput.text) && 
            !string.IsNullOrEmpty(emailRegisterInput.text) && 
            !string.IsNullOrEmpty(paswordRegisterInput.text) && 
            !string.IsNullOrEmpty(confimPasswordRegisterInput.text))
        {
            if (paswordRegisterInput.text == confimPasswordRegisterInput.text)
            {
                var request = new RegisterPlayFabUserRequest
                {
                    DisplayName = userNameRegisterInput.text,
                    Email = emailRegisterInput.text,
                    Password = paswordRegisterInput.text,
                    RequireBothUsernameAndEmail = false
                };

                PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSucces, OnError);
            }
            else
            {
                messageText.text = "Las contraseñas no coinciden";
            }
        }
        else
        {
            messageText.text = "Debes rellenar todos los campos";
        }
    }

    private void OnRegisterSucces(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registro Exitoso";
        ActiveLoginPanel(true);
        ActiveRegisterPanel(false);
    }

    public void EmailLogin()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailLoginInput.text,
            Password = paswordLoginInput.text
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucces, OnError);
    }

    private void OnLoginSucces(LoginResult result)
    {
        LevelLoader.LoadLevel(clientScene);
    }

    public void RecoveryPasword()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailRecoveryInput.text,
            TitleId = "5AFBA"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySucces, OnRecoveryError);
    }

    private void OnRecoverySucces(SendAccountRecoveryEmailResult obj)
    {
        messageText.text = "Correo de recuperacion enviado";
    }

    private void OnRecoveryError(PlayFabError obj)
    {
        messageText.text = "Correo incorrecto o no registrado";
    }

    private void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
    }

    public void ActiveLoginPanel(bool value)
    {
        loginPanel.SetActive(value);
    }

    public void ActiveRegisterPanel(bool value)
    {
        registerPanel.SetActive(value);
    }

    public void ActivePasswordRecoveryPanel(bool value)
    {
        recoveryPanel.SetActive(value);
    }
}
