
# MindScapeAI VR: AI NPC Unity Template Scene

This repository hosts the MindScapeAI project, an innovative Unity-based framework designed for creating immersive AI-powered Non-Player Characters (NPCs) in virtual reality. Developed by team NeuralNexus at the Stanford Immerse The Bay Hackathon, the project integrates OpenAI API, Azure Voice API, Google Cloud Speech to Text, and Oculus Lip Sync to create a unique VR conversational experience.

## 🌟 Inspiration

MindScapeAI aims to revolutionize the realm of AI interaction by merging it with the visually immersive world of VR. Our goal is to elevate standard dialogues into extraordinary visual journeys, providing an engaging and creative platform for AI engagement in VR.

## 🚀 Features

- **AI-Powered NPCs**: Leveraging the power of OpenAI's GPT-4 to create responsive, intelligent NPCs.
- **Voice Interaction**: Integrating Azure Voice and Google Cloud Speech to Text for seamless voice communication.
- **Visual Storytelling**: Utilizing OpenAI's image generation API to create dynamic, contextually relevant visuals.
- **Immersive VR Experience**: Building on Unity 2021.3.x, the project offers a fully immersive VR experience.
- **Customizable Personalities**: Featuring diverse AI personalities like Travel Guide, Medical Professional, Friendly Companion, and Storyteller.
- **Oculus Lip Sync**: Enhancing realism with accurate lip-syncing for NPCs.

## 🛠️ Setup

Ensure the following services are set up with the provided instructions and create configuration files in the `StreamingAssets` folder:

1. [Google Cloud Speech to Text](https://cloud.google.com/speech-to-text)
2. [Azure Voice API](https://azure.microsoft.com/en-us/services/cognitive-services/speech-services/)
3. [OpenAI API](https://beta.openai.com/docs/)

Review setup instructions for these repositories:
- [OpenAI C# API](https://github.com/betalgo/openai)
- [Google Speech to Text](https://github.com/oshoham/UnityGoogleStreamingSpeechToText)
- [Azure Voice](https://github.com/Azure-Samples/cognitive-services-speech-sdk/blob/master/quickstart/csharp/unity/text-to-speech/README.md)
- [Oculus Lip Sync](https://developer.oculus.com/documentation/unity/audio-ovrlipsync-using-unity/)

### Configuration Files

`services_config.json` template:
```json
{
  "OpenAI_APIKey": "your_openai_api_key",
  "AzureVoiceSubscriptionKey": "your_azure_voice_subscription_key",
  "AzureVoiceRegion": "your_azure_voice_region"
}
```

`gcp_credentials.json` template:
```json
{
  // Your Google Cloud credentials here
}
```

## 🎮 Usage

After setting up, run the scene to interact with the AI NPC. Ask questions and experience the AI's responses in an immersive VR environment.

## 🌱 What We Learned

This project honed our skills in debugging and problem-solving under complex scenarios. It highlighted the importance of leveraging team strengths and selecting ambitious yet attainable goals.

## 🔮 What's Next

Looking ahead, we plan to enhance MindScapeAI with better models for even more fluid conversations, integrate Vision capabilities to give the Avatars the ability to see and be aware of their surroundings, and expand platform compatibility. By incorporating user feedback, we aim to continually evolve our platform to exceed expectations in the rapidly advancing AI-VR landscape.

## 📚 References & Resources

- [Azure Cognitive Services Speech SDK Quickstart](https://docs.microsoft.com/azure/cognitive-services/speech-service/quickstart-text-to-speech-csharp-unity)
- [Speech SDK API reference for C#](https://aka.ms/csspeech/csharpref)

## 👥 Authors

- [Mehul Srivastava](https://www.linkedin.com/in/msrivas7/)
- [Hriday Shah](https://www.linkedin.com/in/hridayshah)
- [Bhanu Reddy](https://www.linkedin.com/in/bhanu-reddy-chada)
