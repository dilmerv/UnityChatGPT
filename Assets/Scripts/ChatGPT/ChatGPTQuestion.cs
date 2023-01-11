using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatGPTQuestion", menuName = "ChatGPT/ChatGPTQuestion", order = 2)]
public class ChatGPTQuestion : ScriptableObject
{
    public string scenarioTitle;

    [TextArea(8,20)]
    public string prompt;

    public ChatGPTReplacement[] replacements;

    public string SearchEntityValue
    {
        get
        {
            var searchEntityValue = replacements?
                .SingleOrDefault(r => r.replacementType == Replacements.SEARCH_ENTITY);

            if (searchEntityValue != null) 
                return searchEntityValue.Value.value;
            else 
                return string.Empty;
        }
    }
}
