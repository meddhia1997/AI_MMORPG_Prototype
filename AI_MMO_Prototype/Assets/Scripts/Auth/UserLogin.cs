using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class UserLogin : MonoBehaviour
{
    // UI references using TextMeshPro
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TextMeshProUGUI responseText; 

    // UI elements to toggle after login
    public GameObject loginPanel; // The GameObject to deactivate
    public List<GameObject> activateOnLogin; // List of GameObjects to activate

    // URL for your backend login endpoint
    private string loginUrl = "http://localhost:5000/auth/login"; // Replace with your actual backend URL

    void Start()
    {
        // Add listener to the button
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    // Called when the Login button is clicked
    private void OnLoginButtonClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        // Ensure fields are not empty
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            responseText.text = "Email and password are required!";
            return;
        }

        // Create a login request object
        var loginData = new
        {
            email = email,
            password = password
        };

        // Start the login request
        StartCoroutine(LoginUser(loginData));
    }

    // Coroutine for logging in the user
    private IEnumerator LoginUser(object loginData)
    {
        // Convert the login data to JSON using Newtonsoft.Json
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);

        // Create the UnityWebRequest for the POST request
        UnityWebRequest www = new UnityWebRequest(loginUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // Parse the response
            string response = www.downloadHandler.text;
            JObject jsonResponse = JObject.Parse(response);

            // Display a success message
            responseText.text = "Login successful!\n" + jsonResponse["message"];

            // Check if token exists before storing it
            if (jsonResponse.TryGetValue("token", out JToken token))
            {
                PlayerPrefs.SetString("authToken", token.ToString());
                responseText.text += "\nToken saved!";
                PlayerDataFetcher.Instance.StartFetchingPlayerData();

            }

            // Hide login UI and activate new UI elements
            HandleSuccessfulLogin();
        }
        else
        {
            // Handle errors
            responseText.text = "Login failed: " + www.error;
        }
    }

    // Method to handle UI changes after a successful login
    private void HandleSuccessfulLogin()
    {
        if (loginPanel != null)
        {
            loginPanel.SetActive(false); // Hide login panel
        }

        // Activate all GameObjects in the list
        foreach (GameObject obj in activateOnLogin)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
