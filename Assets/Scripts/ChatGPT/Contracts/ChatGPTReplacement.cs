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
    ACTION,
    API_KEY
}