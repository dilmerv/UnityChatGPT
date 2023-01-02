using System.Linq;

public static class ChatGPTExtensions
{
    public static string[] keywords = new string[]{ "Apply()", "using UnityEngine" };

    public static ChatGPTResponse CodeCleanUp(this ChatGPTResponse chatGPTResponse)
    {
        var codelines = chatGPTResponse.Data.Split("```");
        foreach(var codeLine in codelines)
        {
            
        }
        return chatGPTResponse;
    }

}
