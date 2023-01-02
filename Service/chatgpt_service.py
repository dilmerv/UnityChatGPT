from chatgpt_wrapper import ChatGPT
from flask import Flask, request

app = Flask(__name__)
chatGPT = ChatGPT()
filteredInfo = " - do not write any explanations"

@app.route("/chatgpt/question", methods=['POST'])
def process_message():
    prompt = request.json
    question = prompt['question'] + filteredInfo
    response = chatGPT.ask(question)
    return response
    
if __name__ == "__main__":
    app.run(threaded=False)