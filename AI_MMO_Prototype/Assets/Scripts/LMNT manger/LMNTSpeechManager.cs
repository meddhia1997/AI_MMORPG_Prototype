using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LMNTSpeechManager : MonoBehaviour
{
    // LMNT API endpoint and API key.
    private string lmntApiEndpoint = "https://api.lmnt.com/v1/ai/speech/bytes";
    private string lmntApiKey = "39ada102e07c42c3b4d19074e5d27cfa";

    // Assign an AudioSource via the Inspector.
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// Call this method with the text you want to generate speech for.
    /// </summary>
    /// <param name="text">The text to synthesize.</param>
    public void GenerateSpeech(string text)
    {
        StartCoroutine(CallLMNTSpeechAPI(text));
    }

    private IEnumerator CallLMNTSpeechAPI(string text)
    {
        // Create the request payload with the provided text.
        LMNTRequest requestPayload = new LMNTRequest
        {
            voice = "zoe",
            text = text, // Use the text received from DeepSeek
            model = "aurora",
            language = "en",
            format = "mp3",
            sample_rate = "24000",
            speed = 1
        };

        string jsonPayload = JsonUtility.ToJson(requestPayload);

        UnityWebRequest request = new UnityWebRequest(lmntApiEndpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("X-API-Key", lmntApiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("LMNT API Error: " + request.error);
        }
        else
        {
            // The response contains the audio bytes in MP3 format.
            byte[] audioBytes = request.downloadHandler.data;
            Debug.Log("LMNT API call successful. Received " + audioBytes.Length + " bytes of audio data.");

            // Save the audio bytes to a temporary file.
            string tempFilePath = Path.Combine(Application.persistentDataPath, "temp.mp3");
            File.WriteAllBytes(tempFilePath, audioBytes);
            Debug.Log("Saved audio to: " + tempFilePath);

            // Now load the MP3 file as an AudioClip.
            string fileURL = "file://" + tempFilePath;
            using (UnityWebRequest audioRequest = UnityWebRequestMultimedia.GetAudioClip(fileURL, AudioType.MPEG))
            {
                yield return audioRequest.SendWebRequest();

                if (audioRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error loading AudioClip: " + audioRequest.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(audioRequest);
                    Debug.Log("AudioClip loaded successfully.");

                    // Play the AudioClip using the assigned AudioSource.
                    if (audioSource != null)
                    {
                        audioSource.clip = clip;
                        audioSource.Play();
                    }
                    else
                    {
                        Debug.LogError("AudioSource not assigned in LMNTSpeechManager.");
                    }
                }
            }
        }
    }

    [System.Serializable]
    private class LMNTRequest
    {
        public string voice;
        public string text;
        public string model;
        public string language;
        public string format;
        public string sample_rate;
        public int speed;
    }
}
