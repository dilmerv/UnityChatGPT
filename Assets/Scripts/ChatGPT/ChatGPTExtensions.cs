using System.Linq;

public static class ChatGPTExtensions
{
    public const string KEYWORD_APPLY = "Apply()";
    public const string KEYWORD_USING = "using UnityEngine";
    public const string KEYWORD_PUBLIC_CLASS = "public class";

    public static ChatGPTResponse CodeCleanUp(this ChatGPTResponse chatGPTResponse)
    {
        var codelines = chatGPTResponse.Data.Split("```");
        chatGPTResponse.Data = codelines.FirstOrDefault(c => c.Contains(KEYWORD_APPLY) || c.Contains(KEYWORD_USING) || c.Contains(KEYWORD_PUBLIC_CLASS));
        return chatGPTResponse;
    }

}
