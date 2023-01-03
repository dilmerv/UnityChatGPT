from chatgpt_wrapper import ChatGPT
from flask import Flask, request, jsonify

app = Flask(__name__)
chatGPT = ChatGPT()
filteredInfo = " - do not write any explanations"

@app.route("/chatgpt/question", methods=["POST"])
def question():
    args = request.args
    prompt = request.json
    question = prompt["question"] + filteredInfo

    if args.get("debug", default=False, type=bool):
        print("ChatGPT Question Received...")
        print("ChatGPT Question is: {}".format(question))

    response = chatGPT.ask(question)
    return response

@app.route("/chatgpt/status", methods=["GET"])
def status():
    return jsonify(status="ok")
    
if __name__ == "__main__":
    app.run(threaded=False)