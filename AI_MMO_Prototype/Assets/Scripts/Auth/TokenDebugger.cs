using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

public class TokenExtractor : MonoBehaviour
{
    private void Update()
    {
        DebugToken();
    }

    public void DebugToken()
    {
        // Retrieve the token from PlayerPrefs
        string token = PlayerPrefs.GetString("authToken", "");

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("No token found in PlayerPrefs!");
            return;
        }

        Debug.Log("Raw Token: " + token);

        // Split JWT into Header, Payload, and Signature
        string[] tokenParts = token.Split('.');
        if (tokenParts.Length != 3)
        {
            Debug.LogError("Invalid token format! Expected 3 parts but got " + tokenParts.Length);
            return;
        }

        try
        {
            // Decode all parts of the JWT
            string headerJson = DecodeBase64(tokenParts[0]);
            string payloadJson = DecodeBase64(tokenParts[1]);
            string signature = tokenParts[2]; // Signature is not decoded

            // Log each section separately
            Debug.Log("===== JWT Header =====");
            DebugJson(headerJson);

            Debug.Log("===== JWT Payload =====");
            DebugJson(payloadJson);

            Debug.Log("===== JWT Signature =====");
            Debug.Log("Signature (Base64): " + signature);

            // Extract playerdata (role) from the payload and save to PlayerPrefs
            SaveRoleFromToken(payloadJson);
        }
        catch (Exception e)
        {
            Debug.LogError("Error decoding token: " + e.Message);
        }
    }

    private string DecodeBase64(string base64)
    {
        // Ensure correct padding for Base64 decoding
        base64 = base64.Replace('-', '+').Replace('_', '/');
        while (base64.Length % 4 != 0)
        {
            base64 += "=";
        }

        byte[] data = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(data);
    }

    private void DebugJson(string json)
    {
        try
        {
            JObject jsonData = JObject.Parse(json);
            foreach (var item in jsonData)
            {
                Debug.Log($"{item.Key}: {item.Value}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing JSON: " + e.Message);
            Debug.Log(json); // Print raw JSON in case of errors
        }
    }

    // Extract role from the payload and save to PlayerPrefs
    private void SaveRoleFromToken(string payloadJson)
    {
        try
        {
            JObject payloadData = JObject.Parse(payloadJson);

            // Check if the playerdata and role fields exist
            if (payloadData["playerdata"] != null && payloadData["playerdata"]["role"] != null)
            {
                string role = payloadData["playerdata"]["role"].ToString();
                PlayerPrefs.SetString("playerRole", role); // Save the role in PlayerPrefs
                Debug.Log("Player role saved: " + role);
            }
            else
            {
                Debug.LogError("Role not found in playerdata.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error extracting role from token: " + e.Message);
        }
    
    
    }   
}