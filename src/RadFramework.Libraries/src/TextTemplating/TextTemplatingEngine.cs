namespace RadFramework.Libraries.TextTemplating;

public class TextTemplatingEngine
{
    public void TemplateFromStringTemplate(TextTemplate engineTemplate, string templateText)
    {
        // go through the template and watch out for placeholders
        for(int i = 0; i < templateText.Length; i++)
        {
            TemplateCursorStatus status = GetTemplateCursorStatus(ref templateText, ref i);
            
            if (status == TemplateCursorStatus.Placeholder)
            {
                string placeholderName = "";
                
                ExtractPlaceholderName(ref templateText, ref i, ref placeholderName);

                // store the placeholders name
                engineTemplate.AddPart(placeholderName, true);

                // skip ]
                i++;
            }
            else if(status == TemplateCursorStatus.Text)
            {
                string templatePart = "";
                
                ExtractTemplateText(engineTemplate, ref templateText, ref i, ref templatePart);
                
                engineTemplate.AddPart(templatePart, false);
            }
            else if(status == TemplateCursorStatus.EndOfFile)
            {
                return;
            }
        }
    }

    private static TemplateCursorStatus GetTemplateCursorStatus(ref string templateText, ref int i)
    {
        if (i == 0)
        {
            if (0 == templateText.Length)
            {
                return TemplateCursorStatus.EndOfFile;
            }
            
            if (IsPlaceholderStart(templateText, i))
            {
                return TemplateCursorStatus.Placeholder;
            }
            else
            {
                return TemplateCursorStatus.Text;
            }
        }

        if (i > templateText.Length)
        {
            return TemplateCursorStatus.EndOfFile;
        }

        if (IsPlaceholderStart(templateText, i))
        {
            return TemplateCursorStatus.Placeholder;
        }
        else
        {
            return TemplateCursorStatus.Text;
        }
    }

    private static bool IsPlaceholderStart(string templateText, int i)
    {
        return i < templateText.Length 
               && (i + 1) < templateText.Length 
               && templateText[i] == '$' 
               && templateText[i + 1] == '[';
    }

    private enum TemplateCursorStatus
    {
        Placeholder,
        Text,
        EndOfFile
    }

    private static void ExtractTemplateText(
        TextTemplate engineTemplate,
        ref string template,
        ref int i,
        ref string templatePart)
    {
        try
        {
            while (!IsPlaceholderStart(template, i))
            {
                templatePart += template[i];
                i++;
            }

            i--;
        }
        catch (IndexOutOfRangeException)
        {
        }
    }

    private static void ExtractPlaceholderName(
        ref string template, 
        ref int i, 
        ref string placeholderName)
    {
        try
        {
            // skip the special chars
            i += 2;

            // wait for the end of the placeholder
            while (template[i] != ']')
            {
                placeholderName += template[i];
                i++;
            }

            i--;
        }
        catch (IndexOutOfRangeException)
        {
        }
    }
}