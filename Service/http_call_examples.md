## HTTP examples to use with our Python Flask Service

[Download VS Code Rest Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) to easily execute these commands 
```
POST http://127.0.0.1:5000/chatgpt/question HTTP/1.1
content-type: application/json

{
    "question" : "create a unity C# script which creates a cube and do not include explanations"
}

POST http://127.0.0.1:5000/chatgpt/question HTTP/1.1
content-type: application/json

{
    "question" : "create a unity snake game and provide all the scripts required to do so"
}

GET http://127.0.0.1:5000/chatgpt/status HTTP/1.1

```
