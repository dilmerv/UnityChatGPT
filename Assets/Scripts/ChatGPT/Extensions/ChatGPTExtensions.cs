using System.Linq;

public static class ChatGPTExtensions
{
    public const string KEYWORD_USING = "using UnityEngine";
    public const string KEYWORD_PUBLIC_CLASS = "public class";
    public static readonly string[] filters = { "C#", "c#","csharp","CSHARP" };

    public static ChatGPTResponse CodeCleanUp(this ChatGPTResponse chatGPTResponse)
    {
        var message = chatGPTResponse.Choices.FirstOrDefault().Message.Content;

        // apply filters
        filters.ToList().ForEach(f =>
        {
            message = message.Replace(f, string.Empty);
        });

        // split due to explanations
        var codeLines = message.Split("```");

        // extract code
        message = codeLines.FirstOrDefault(c => c.Contains(KEYWORD_USING) ||
            c.Contains(KEYWORD_PUBLIC_CLASS)).Trim();

        // handle this better
        chatGPTResponse.Choices[0].Message.Content = message;

        return chatGPTResponse;
    }
}