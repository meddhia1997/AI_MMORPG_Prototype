using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class UserAuthentication : MonoBehaviour
{ 
    public RoleSelector roleSelector; // Reference to the RoleSelector script
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField repeatPasswordInput;
    public TMP_InputField emailInput;
    public Button signInButton;
    public TextMeshProUGUI responseText;

    // UI Panels
    public GameObject signInPanel; // Panel containing sign-in UI
    public GameObject mainMenu; // Panel for main menu after login

    private string signUpUrl = "http://localhost:5000/auth/signup"; 

    void Start()
    {
        signInButton.onClick.AddListener(OnSignInButtonClicked);
    }

    private void OnSignInButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        string repeatPassword = repeatPasswordInput.text;
        string email = emailInput.text;
        string role = roleSelector.GetSelectedSpriteName();
        if (password != repeatPassword)
        {
            responseText.text = "Passwords do not match!";
            return;
        }

    var user = new
    {
        username = username,
        email = email,
        password = password,
        playerdata = new JObject
        {
            ["role"] = role // Add the role field to playerdata
        }
    };

        StartCoroutine(SignUpUser(user));
    }

    private IEnumerator SignUpUser(object user)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
        UnityWebRequest www = new UnityWebRequest(signUpUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            JObject jsonResponse = JObject.Parse(response);

            responseText.text = "Sign up successful!\n" + jsonResponse["message"];

            // Disable sign-in UI and enable main menu
            signInPanel.SetActive(false);
            mainMenu.SetActive(true);
        }
        else
        {
            responseText.text = "Error: " + www.error;
        }
    }
}
