# PaLM2Unity
Unity script to talk to Google's  PaLM2 Api.
A simple Discord bot example that uses PaLM2 Api.


> Getting your PaLM2 Key - https://makersuite.google.com/waitlist

# How to use

* Import the folder into your project.
* Open PaLM2Scene inside the PaLM2 Folder
* Go to the PALM2API and insert your API key. 
* Press play and type in the text field and click send. 

# Function Usage:

TextPrompt and TextPromptFromInput are there to access the coroutine. 

# AddToStart

This uses the text generation method which has a faster response time. You can add personality or extra information using addToPromptStart. It's defaulted to "Be Breif" causing it to give short responses and not overwhelm the text box. 

# Debug
Use DebugResponse to get the full response from the PaLM2 Api
