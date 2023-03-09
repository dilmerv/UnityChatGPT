# Unity ChatGPT Experiments
Few examples based on [my 🤖 ChatGPT with Unity video series on YouTube](https://www.youtube.com/watch?v=6pWoVRYNWws&list=PLQMQNmwN3Fvxec05vELA3D05-Y93LzFt_&index=5) which is now supporting ChatGPT API and ChatGPT Python Wrapper BUT highly experimental. 

🔔 *Support all my work by [Subscribing to YouTube](https://www.youtube.com/@dilmerv?sub_confirmation=1)* thank you !

## ChatGPT Demo Scenes

|Scenes||
|---|---|
|**ChatGPTLogger.unity**: a simple scene showing you how to ask ChatGPT to generate code which creates primitive cubes|**ChatGPTPlayerClones**: loads a player armature from resources, clones players every 1/2 a second, gets the starter assets input component, and makes players move and jump|
|<img src="https://github.com/dilmerv/UnityChatGPT/blob/master/docs/images/ChatGPTDemo_1.gif" width="300">|<img src="https://github.com/dilmerv/UnityChatGPT/blob/master/docs/images/ChatGPTDemo_2.gif" width="300">|

## ChatGPT Example Prompts
1. A unity c# class script that creates 100 cubes by Using PrimitiveType cubes and then forms a three dimensional pyramid
2. A unity c# class script that finds a PlayerArmature game object and get StarterAssetsInputs component and sets the "move" field to a vector2 with 0.3f for x and 0 for y, set "move" field to 0 after 3 seconds, then set "jump" field to true for 3 seconds
3. A unity c# class script that loads a PlayerArmature from Resources consistently every 1/2 second, get StarterAssetsInputs component, set the "move" field to a vector2 with 1.0f for x and 0 for y, then set the "jump" field to true

## Unity Requirements:
1. Unity 2021.3.8f or greater
2. [Roslyn C# - Runtime DLLs](https://github.com/dilmerv/UnityRoslynDemos) which you can get from the Resources folder. I'm also planning to add the Class Library project to GitHub which should allow you to generate these DLLs yourself.
3. Open any of the available scenes
4. Create an account in [OpenAI](https://platform.openai.com/signup)
5. Get a new [API Key](https://platform.openai.com/account/api-keys) and your [API Organization](https://platform.openai.com/account/org-settings)
6. Update ChatGPTSettings file located under Assets/Settings/ChatGPT/
   
   <img src="https://github.com/dilmerv/UnityChatGPT/blob/master/docs/images/NewChatGPTSettings.png" width="300">

7. Each scene has a ChatGPTTester game object & script in the hierarchy, feel free to associate a new ChatGPTQuestion scriptable object as a reference as shown below:

   <img src="https://github.com/dilmerv/UnityChatGPT/blob/master/docs/images/NewChatGPTQuestion.png" width="300">

## ChatGPT Flask Service 

⚠️Deprecated - the latest changes use ChatGPT API instead of a python wrapper, however you could still use this but the python service may need some refactoring as the incoming object changed**

ChatGPT is not officially available which means there're unofficial solutions which allow you to use code which gets ChatGPT answers from [OpenAI](https://beta.openai.com/playground) by using browser automation tools. To do this do the following:
1. Open a terminal and cd into this repo UnityChatGPT > Service folder
2. Be sure you've Python 3.7 or greater installed if not (download it before proceeding)
3. Create a Python virtual environment and activate it: (note that I am using Windows and some of these commands may differ a bit with macOS or Linux)
    ```
    python -m venv chatgpt_env
    cd chatgpt_env
    .\Scripts\activate
    pip install pyreadline3
    pip install git+https://github.com/mmabrouk/chatgpt-wrapper
    playwright install firefox
    pip install flask
    ```
3. Login to OpenAI with the following command:
    ```
    chatgpt install
    ```
4. Now you should have an environment created and ChatGPT tested

## How to run the samples ?
1. Run our ChatGPT flask service which exposes an endpoint at /chatgpt/question/
   ```
    python .\chatgpt_service.py
   ```
2. Test the ChatGPT flask service by doing a GET request at http://127.0.0.1:5000/chatgpt/status if you get the response below proceed to step 4:
    ```json
    {
        "status": "ok"
    }
    ```
3. Open the Unity project and scene located at Assets > Scenes > ChatGPTLogger.unity
4. Go to Assets > Settings > ChatGPTSettings.asset and point the API Url to your ChatGPT flask service URL:

   <img src="https://github.com/dilmerv/UnityChatGPT/blob/master/docs/images/ChatGPTSettings.png" width="300">
5. Ask ChatGPT a question as shown by clicking on "ChatGPTTester" game object:

   <img src="https://github.com/dilmerv/UnityChatGPT/blob/master/docs/images/ChatGPTPrompt.png" width="300">
6. Hit Play In Unity

## Sample Questions to Ask ChatGPT
1. Write unity c# script monoBehavior named CubePlacer provides a method called Apply() which creates 5 primitive cubes 8 meter away from the camera along the z axis and each separated by 2 meters along the x axis and tilt it by 15 degrees on x
