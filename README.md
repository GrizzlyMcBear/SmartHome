# SmartHome
A POC project for a smart home system, features would be added incrementally and would love to get feedbacks and suggestions


## Action Items


### Speech Recognition

- [ ] Add a POC for a feature to recognize speech and convert it into string
- [ ] Research the namespaces [System.Speech.Recognition](https://docs.microsoft.com/en-us/dotnet/api/system.speech.recognition?view=netframework-4.8) and
[Microsoft.Speech.Recognition](https://docs.microsoft.com/en-us/previous-versions/office/developer/speech-technologies/dd167476(v=office.14)) and find the differences between them.
- [ ] Design and implement a basic mechanism to turn that string into actions  
i.e. add a new device to the home.  


### Image Recognition

- [ ] Add image recognition features to open the pet door when recognizing our pets.
- [ ] Check the option of using the image recognition feature to unlock doors for family members and friends.


### Language

- [ ] Add recognition and synthesis for the following languages:
	 - [ ] Hebrew  


## Links

- [x] A [TTS synthesis MSDN article](https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/june/speech-text-to-speech-synthesis-in-net) from June 2019
- [ ] A [Speech recognition MSDN article](https://docs.microsoft.com/en-us/archive/msdn-magazine/2014/december/voice-recognition-speech-recognition-with-net-desktop-applications#adding-speech-to-a-console-application) from December 2014
- [Voice Browser Working Group](w3.org/TR/speech-synthesis)
- [Bing Translator](bing.com/translator)


### Further reading

- Synthesizing TTS: read about `concatenation unit selection TTS`
- If prosody is required, read and implement SSML
- Research about using the [Microsoft neural network TTS](https://azure.microsoft.com/en-us/blog/microsoft-previews-neural-network-text-to-speech/) considering the following:
	 - Its price-plan
	 - The ability to anonymise the outgoing data (privacy)
- [Create a Custom Voice](http://bit.ly/2VE8th4) by using Microsoft's [Cognitive Services](http://bit.ly/2XWorku)
- [Speech Software Development Kit](bit.ly/2DDTh9I)