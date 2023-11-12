/*using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChatBotController : MonoBehaviour
{
    public ChatNetworkController networkController;
    public AzureVoiceGenerator azureVoice;
    public TextMeshProUGUI textDebugger;

    private OpenAIAPI openAI;
    private IChatEndpoint chatEndpoint;
    private Conversation currentConversation;
    public string question;
    private bool playAudio = false;

    [Serializable]
    public class ConfigData
    {
        public string OpenAI_APIKey;
        public string AzureVoiceSubscriptionKey;
        public string AzureVoiceRegion;
    }

    [Serializable]
    public class OpenAI_RequestConfiguration
    {
        public double Temperature = 0.7;
        public int MaxTokens = 150;
    }

    public OpenAI_RequestConfiguration requestConfiguration;

    void Awake()
    {
        // Load configuration from a file
        ConfigData configData;
        string configFilePath = Path.Combine(Application.streamingAssetsPath, "services_config.json");
        if (File.Exists(configFilePath))
        {
            string jsonString = File.ReadAllText(configFilePath);
            configData = JsonUtility.FromJson<ConfigData>(jsonString);
        }
        else
        {
            Debug.LogError("Configuration file not found!");
            return;
        }

        // Initialize OpenAI and Azure Voice
        openAI = new OpenAIAPI(new APIAuthentication(configData.OpenAI_APIKey));
        azureVoice.SetServiceConfiguration(configData.AzureVoiceSubscriptionKey, configData.AzureVoiceRegion);

        // Initialize Chat Endpoint and Conversation
        chatEndpoint = openAI.Chat;
        chatEndpoint.DefaultChatRequestArgs = new ChatRequest()
        {
            Model = Model.GPT4,
            Temperature = requestConfiguration.Temperature,
            MaxTokens = requestConfiguration.MaxTokens
        };

        currentConversation = chatEndpoint.CreateConversation();


        openAI.ImageGenerations
    }

    public async void SendRequest()
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            Debug.LogError("No question provided to send.");
            return;
        }

        try
        {
            // Append user input to the conversation
            currentConversation.AppendUserInput(question);

            // Get response from the chatbot
            string response = await currentConversation.GetResponseFromChatbotAsync();

            if (!string.IsNullOrEmpty(response))
            {
                ProcessResponse(response);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error in sending request: " + e.Message);
        }
    }

    private void ProcessResponse(string response)
    {
        if (!string.IsNullOrEmpty(response))
        {
            // Trim the response to the last complete sentence
            string trimmedResponse = TrimToLastCompleteSentence(response);

            Debug.Log("Response: " + trimmedResponse);
            textDebugger.text = trimmedResponse;

            if (networkController != null)
            {
                networkController.PlayMessage(trimmedResponse);
            }
            else
            {
                Debug.LogWarning("NetworkController is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("Received empty response.");
        }
    }

    private string TrimToLastCompleteSentence(string response)
    {
        int lastPeriodIndex = response.LastIndexOf('.');
        if (lastPeriodIndex >= 0)
        {
            return response.Substring(0, lastPeriodIndex + 1); // Include the period
        }

        return response; // Return the original response if no period is found
    }


    public void OnFinalResult(string youSaid)
    {
        textDebugger.text = "You said: " + youSaid;
        question = youSaid;
        SendRequest();
    }
}*/


using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Images;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBotController : MonoBehaviour
{
    public ChatNetworkController networkController;
    public AzureVoiceGenerator azureVoice;
    public TextMeshProUGUI textDebugger;

    private OpenAIAPI openAI;
    private IChatEndpoint chatEndpoint;
    private IImageGenerationEndpoint imageGenerationEndpoint;
    private Conversation currentConversation;
    public string question;
    private bool playAudio = false;

    public ChatbotPersonalityProfile[] personalityProfiles; // for editor assignment
    protected Dictionary<string, ChatbotPersonalityProfile> chatbotPersonalities;     // for accessing relevant properties within code
    public string setPersonalityProfileName;

    public TextMeshProUGUI ActiveProfileText;

    public GameObject ImageScreen;

    [Serializable]
    public class ConfigData
    {
        public string OpenAI_APIKey;
        public string AzureVoiceSubscriptionKey;
        public string AzureVoiceRegion;
    }

    [System.Serializable]
    public class ChatbotPersonalityProfile
    {
        public string PersonalityName;
        public string PersonalityDescription;
        public ChatbotPersonalityProfile(string name, string personality)
        {
            PersonalityName = name;
            PersonalityDescription = personality;
        }
    }

    [Serializable]
    public class OpenAI_RequestConfiguration
    {
        public double Temperature = 0.7;
        public int MaxTokens = 150;
    }

    public OpenAI_RequestConfiguration requestConfiguration;

    void Awake()
    {
        ImageScreen.gameObject.SetActive(false);
        // Load configuration from a file
        ConfigData configData;
        string configFilePath = Path.Combine(Application.streamingAssetsPath, "services_config.json");
        if (File.Exists(configFilePath))
        {
            string jsonString = File.ReadAllText(configFilePath);
            configData = JsonUtility.FromJson<ConfigData>(jsonString);
        }
        else
        {
            Debug.LogError("Configuration file not found!");
            return;
        }

        // Initialize OpenAI and Azure Voice
        openAI = new OpenAIAPI(new APIAuthentication(configData.OpenAI_APIKey));
        azureVoice.SetServiceConfiguration(configData.AzureVoiceSubscriptionKey, configData.AzureVoiceRegion);

        // Initialize Chat Endpoint and Conversation
        chatEndpoint = openAI.Chat;
        imageGenerationEndpoint = openAI.ImageGenerations;
        chatEndpoint.DefaultChatRequestArgs = new ChatRequest()
        {
            Model = Model.GPT4,
            Temperature = requestConfiguration.Temperature,
            MaxTokens = requestConfiguration.MaxTokens
        };

        currentConversation = chatEndpoint.CreateConversation();

        // Set up dictionary of personality profiles
        chatbotPersonalities = new Dictionary<string, ChatbotPersonalityProfile>();
        foreach (ChatbotPersonalityProfile chatbotPersonality in personalityProfiles)
        {
            chatbotPersonalities.Add(chatbotPersonality.PersonalityName, chatbotPersonality);
        }
        if (setPersonalityProfileName == "") setPersonalityProfileName = personalityProfiles[0].PersonalityName; // Default personality profile

        ActiveProfileText.text = setPersonalityProfileName;

    }

    public async void SendRequest()
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            Debug.LogError("No question provided to send.");
            return;
        }

        try
        {
            // Modify the question with detailed instructions and an example for the desired format
            /*string modifiedQuestion = $"{question}\n\n" +
                                      "Respond in the following format: A detailed text response, " +
                                      "followed by an image prompt related to the topic, and then " +
                                      "a description or discussion about the image. For example, " +
                                      "if asked about New York, the response should be a description " +
                                      "of New York, followed by '[Image Prompt: Statue of Liberty]' " +
                                      "and then a description about how the Statue of Liberty relates " +
                                      "to New York like you could say(Here I've pulled up an image of Statue of Liberty City which is an important landmark of newyork etc etc).";
*/
            /*string modifiedQuestion = $"{question}\n\n" +
                                     "Respond with a brief text description, an image prompt related to the topic, and a short discussion about the image. " +
                                     "For example, if the topic is New York, provide a concise description, '[Image Prompt: New York Skyline],' " +
                                     "and discuss the image's relevance. E.g., 'This image showcases the iconic New York skyline, symbolizing the city's resilience and cultural richness.'";*/

            string modifiedQuestion = $"{chatbotPersonalities[setPersonalityProfileName].PersonalityDescription}+ {question}\n\n" +
                                       "You're talking to a College Student, Please provide a brief text response related to the topic. Then, suggest an image that represents the topic, placing the image prompt within square brackets like '[Image Prompt: 'your image prompt here']'. " +
                                       "The image should give meaning to the context and avoid practical representations like calendars or graphs. After the image prompt, include a short discussion explaining the image's relevance to the topic. ";
                                       /*"For example, if the topic is about 'the concept of time,' instead of suggesting a literal clock, you might suggest '[Image Prompt: A serene landscape with a sunrise],' " +
                                       "and then elaborate, 'This image of a sunrise over a tranquil field symbolizes the perpetual cycle of time and nature’s serene progression, resonating with the concept of time's steady and serene flow.'";
*/
            currentConversation.AppendUserInput(modifiedQuestion);

            // Get response from the chatbot
            string fullResponse = await currentConversation.GetResponseFromChatbotAsync();
            Debug.Log("Full Unfiltered Response: " + fullResponse);
            string textResponse, imagePrompt;
            ParseResponse(fullResponse, out textResponse, out imagePrompt);

            // Process text response
            if (!string.IsNullOrEmpty(textResponse))
            {
                ProcessTextResponse(textResponse);
            }

            // Process image prompt
            if (!string.IsNullOrEmpty(imagePrompt))
            {
                await ProcessImagePrompt(imagePrompt);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error in sending request: " + e.Message);
        }
    }


    private void ParseResponse(string fullResponse, out string textResponse, out string imagePrompt)
    {
        // Initialize the output variables
        textResponse = "";
        imagePrompt = "";

        // Check if the response contains the image prompt format
        int imagePromptStart = fullResponse.IndexOf("[Image Prompt: ");
        int imagePromptEnd = imagePromptStart != -1 ? fullResponse.IndexOf(']', imagePromptStart) : -1;

        if (imagePromptStart != -1 && imagePromptEnd != -1)
        {
            // Extract the image prompt
            imagePrompt = fullResponse.Substring(imagePromptStart + 15, imagePromptEnd - imagePromptStart - 15).Trim();

            // Extract the text before and after the image prompt
            string textBeforeImagePrompt = fullResponse.Substring(0, imagePromptStart).Trim();
            string textAfterImagePrompt = fullResponse.Substring(imagePromptEnd + 1).Trim();

            // Combine the text before and after the image prompt
            textResponse = textBeforeImagePrompt + " " + textAfterImagePrompt;
        }
        else
        {
            // If no image prompt is found, treat the entire response as the text response
            // This should not happen as per your assumption, but is here as a fallback
            textResponse = fullResponse;
        }
    }


    private void ProcessTextResponse(string response)
    {
        Debug.Log("Text Response: " + response);
        textDebugger.text = response;

        if (networkController != null)
        {
            networkController.PlayMessage(response);
        }
        else
        {
            Debug.LogWarning("NetworkController is not assigned.");
        }
    }

    public Image imageDisplay; // Assign this in the Unity Inspector

    private async Task ProcessImagePrompt(string imagePrompt)
    {
        // Generate an image
        ImageResult imageResult = await imageGenerationEndpoint.CreateImageAsync(imagePrompt);

        if (imageResult != null && imageResult.Data != null && imageResult.Data.Count > 0)
        {
            // Get the URL of the first image result
            string imageUrl = imageResult.Data[0].Url;

            // Check if the URL is not null or empty
            if (!string.IsNullOrEmpty(imageUrl))
            {
                Debug.Log("Image URL: " + imageUrl);
                StartCoroutine(LoadImage(imageUrl));
            }
            else
            {
                Debug.LogError("No image URL found in the response.");
            }
        }
        else
        {
            Debug.LogError("No image data received.");
        }
    }


    private IEnumerator LoadImage(string imageUrl)
    {
        using (WWW www = new WWW(imageUrl))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                Texture2D texture = www.texture;
                if (texture != null)
                {
                    // Convert Texture2D to Sprite
                    Sprite imageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    ImageScreen.SetActive(true);
                    imageDisplay.sprite = imageSprite;
                }
            }
            else
            {
                Debug.LogError("Error loading image: " + www.error);
            }
        }
    }


    public void OnFinalResult(string youSaid)
    {
        textDebugger.text = "You said: " + youSaid;
        question = youSaid;
        SendRequest();
    }
}
