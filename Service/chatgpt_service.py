from chatgpt_wrapper import ChatGPT
from flask import Flask, request, jsonify

app = Flask(__name__)
chatGPT = ChatGPT()
filteredInfo = " - do not write any explanations"

@app.route("/chatgpt/question", methods=['POST'])
def question():
    prompt = request.json
    question = prompt['question'] + filteredInfo
    response = chatGPT.ask(question)
    return response

@app.route("/chatgpt/status", methods=['GET'])
def status():
    return jsonify(status='ok')
    
if __name__ == "__main__":
    app.run(threaded=False)