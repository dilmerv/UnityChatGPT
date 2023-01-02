# Unity ChatGPT
Few examples with ChatGPT In Unity and this is highly experimental, the ChatGPT API used here is not (official).

## Unity Requirements:
1. Unity 2021.3.8f or greater
2. [Roslyn C# - Runtime Compiler](https://assetstore.unity.com/packages/tools/integration/roslyn-c-runtime-compiler-142753?aid=1101l7LXo) (This is an Affiliate Link)

## ChatGPT Flask Service:
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
4. Go to Assets > Settings > ChatGPTSettings.asset and point the API Url to your ChatGPT flask service URL
   <img src="https://github.com/dilmerv/UnityChatGPT/blob/master/docs/images/ChatGPTSettings.png" width="300">