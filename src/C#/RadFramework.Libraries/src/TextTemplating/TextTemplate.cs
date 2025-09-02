namespace RadFramework.Libraries.TextTemplating;

public class TextTemplate
{
    internal string Tag { get; set; }
    
    private List<(string part, bool isPlaceholder)> stringParts = new();

    internal void AddPart(string part, bool isPlaceholder)
    {
        stringParts.Add((part, isPlaceholder));
    }
    
    public string FillTemplate(Dictionary<string, string> arguments)
    {
        string processedTemplate = "";

        for (var i = 0; i < stringParts.Count; i++)
        {
            var part = stringParts[i];
            bool hasValue = false;

            if (part.isPlaceholder && (hasValue = arguments.TryGetValue(part.part, out var argument)))
            {
                processedTemplate += argument;
                continue;
            }

            if (part.isPlaceholder)
            {
                continue;
            }
            
            processedTemplate += part.part;
        }

        return processedTemplate;
    }
}