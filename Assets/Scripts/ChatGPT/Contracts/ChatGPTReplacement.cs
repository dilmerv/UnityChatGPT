using System;

[Serializable]
public struct ChatGPTReplacement
{
    public Replacements replacementType;
    public string value;
}

[Serializable]
public enum Replacements
{
    CLASS_NAME,
    BROADCAST_NAME,
    ACTION_APPLY,
    API_KEY,
    SEARCH_ENTITY
}