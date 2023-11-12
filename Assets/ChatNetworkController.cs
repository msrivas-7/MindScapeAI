using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Point of this class is to be able to send Chat (string) messages to others for them to play through their AzureVoiceGenerators
/// </summary>
public class ChatNetworkController : MonoBehaviour
{
    public UnityEvent OnStartTyping;
    /// <summary>
    /// Reference to the audio source to play sound through
    /// </summary>
    public AzureVoiceGenerator azureVoice;

    /// <summary>
    /// The audio source that the audio clip should be played through
    /// </summary>
    public AudioSource azureAudioSource;

    /// <summary>
    /// Test clip to send over the network (doesn't work since files are too large)
    /// </summary>
    public AudioClip testClip;

    /// <summary>
    /// Text to display output from SendMessageToAll
    /// </summary>
    public TextMeshProUGUI outputText;

    public string mostRecentMessage = "NONE";
    public string testMessage = "";

    private bool waitingForAzureResponse = false;


    /// <summary>
    /// Play message locally with Azure
    /// </summary>
    /// <param name="message">The message to play on Azure</param>
    public void PlayMessage(string message)
    {
        //Set output text to text response from chatbot!
       // outputText.text = message;
        UpdateText(message);
        OnStartTyping?.Invoke();

        //Invoke audio request with message and wait for clip to not be null
        azureVoice.inputField.text = message;
        azureVoice.InvokeAzureVoiceRequest();
        //StopSpeaking.Invoke();

    }

    /// <summary>
    /// Send short message string over the network
    /// </summary>
    /// <param name="message"></param>
   /* public void SendMessageToAll(string message)
    {
        GetAudioClipFromAzure(message);
        if (GameController.Instance != null)
        {
            Debug.Log("Sending message to everyone");
            GameController.Instance.SendReliableData(message); //send message to everyone else
        }
        else
        {
            Debug.Log("No GameController to send message through!");
        }

    }*/

    /// <summary>
    /// Converts message to AudioClip and sends over the network to everyone else
    /// </summary>
    /// <param name="message"></param>
    /*public void SendAudioClipToAll(string message)
    {
        if(waitingForAzureResponse)
        {
            Debug.Log("Still waiting for previous response from azure");
            return;
        }

        //Convert message to audio clip
        AudioClip audioData = GetAudioClipFromAzure(message);

        //convert to JSON string to send over network
        string dataToSend = AudioClipJsonConverter.AudioClipToJson(audioData);

        if (GameController.Instance != null)
        {
            Debug.Log("Sending audio clip JSON to everyone to play");
            GameController.Instance.SendReliableData(dataToSend); //send message to everyone else
        }
        else
        {
            Debug.Log("No GameController to send audio clip through!");
        }
        
    }*/
    public float delay = 0.2f;
    IEnumerator ShowTextWordByWord(string fullText)
    {
        string[] words = fullText.Split(' ');
        string currentText = "";

        foreach (string word in words)
        {
            currentText += word + " ";
            outputText.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }

    public void UpdateText(string fullText)
    {
        StartCoroutine(ShowTextWordByWord(fullText));
    }

    /// <summary>
    /// Called when a message is received by the subscribed OnReliableMessage event handler
    /// </summary>
    /// <param name="data">The data that was sent</param>
    /// <param name="sendBySelf">Whether or not sender id == network id -> args.SenderId == NetworkInterface.Instance.NetworkGuid</param>
    private void MessageReceived(object data, bool sendBySelf)
    {
        //Debug.Log("Data: " + data);
        //Debug.Log("Send By Self: " + sendBySelf.ToString());
        
        Debug.Log("Received data string: " + (string)data);

        //Set output text to text response from chatbot!
       // outputText.text = (string)data;
        UpdateText((string)data);

        /*
         * This is the old script for sending a whole audio clip to others
         * 
        //exit out since the sender will already have the audio clip play automatically by AzureVoiceGenerator
        if (sendBySelf)
        {
            Debug.Log("Audio clip already played for you since you are the sender. Exiting..");
            return;
        }

        //Parse the data as an audio clip
        Debug.Log("Converting data JSON string to Audio Clip");
        AudioClip parsedClip = AudioClipJsonConverter.JsonToAudioClip((string)data);
        
        //Set audio source to null to make sure all clients start as null (including sender)
        outputSource.clip = null;

        //Set to audio source this received clip
        outputSource.clip = parsedClip;

        //Play audio source unless you are the sender then don't
        outputSource.Play();
        */
    }

    private AudioClip GetAudioClipFromAzure(string message)
    {
        Debug.Log("Converting following message to AudioClip: " + message);

        //Set AudioClip to null in Azure Audio Source
        azureAudioSource.clip = null;

        //Invoke audio request with message and wait for clip to not be null
        azureVoice.inputField.text = message;
        azureVoice.InvokeAzureVoiceRequest();

        waitingForAzureResponse = true; //make sure this function isn't called more than once

        Debug.Log("Invoking audio request to get back clip...");
        while (azureAudioSource.clip == null) { }
        Debug.Log("Audio Clip is no longer null!");

        waitingForAzureResponse = false; //done with waiting, set to false

        //set new AudioClip to send equal to Azure Audio Source output
        AudioClip audioData = azureAudioSource.clip;

        //return the audio clip object
        return audioData;
    }

    /// <summary>
    /// Play test message through audio source that should be hooked up to TeamSpeak for all to hear
    /// </summary>
    public void TestAudioSound()
    {
        //Set output text to be the test message
        outputText.text = testMessage;

        //Invoke audio request with message and wait for clip to not be null
        azureVoice.inputField.text = testMessage;
        azureVoice.InvokeAzureVoiceRequest();
    }

}
