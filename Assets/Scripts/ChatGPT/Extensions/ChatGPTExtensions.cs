using System.Linq;

public static class ChatGPTExtensions
{
    public const string KEYWORD_USING = "using UnityEngine";
    public const string KEYWORD_PUBLIC_CLASS = "public class";
    public static readonly string[] filters = { "C#", "c#" };

    public static ChatGPTResponse CodeCleanUp(this ChatGPTResponse chatGPTResponse)
    {
        // apply filters
        filters.ToList().ForEach(f =>
        {
            chatGPTResponse.Data = chatGPTResponse.Data.Replace(f, string.Empty);
        });

        // split due to explanations
        var codeLines = chatGPTResponse.Data.Split("```");

        // extract code
        chatGPTResponse.Data = codeLines.FirstOrDefault(c => c.Contains(KEYWORD_USING) ||
            c.Contains(KEYWORD_PUBLIC_CLASS));

        return chatGPTResponse;
    }
}