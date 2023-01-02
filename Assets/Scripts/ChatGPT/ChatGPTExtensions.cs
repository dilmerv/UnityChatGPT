public static class ChatGPTExtensions
{
    public static ChatGPTResponse CodeCleanUp(this ChatGPTResponse chatGPTResponse)
    {
        chatGPTResponse.Data = chatGPTResponse.Data.Replace("```", string.Empty);
        chatGPTResponse.Data = chatGPTResponse.Data.TrimStart('\n');
        chatGPTResponse.Data = chatGPTResponse.Data.TrimEnd('\n');
        return chatGPTResponse;
    }

}
