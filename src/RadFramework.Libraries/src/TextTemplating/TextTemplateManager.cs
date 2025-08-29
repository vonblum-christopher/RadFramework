using System.Collections.Concurrent;

namespace RadFramework.Libraries.TextTemplating;

public class TextTemplateManager
{
    private TextTemplatingEngine engine = new TextTemplatingEngine();
    
    private ConcurrentDictionary<string, TextTemplate> templates = new ConcurrentDictionary<string, TextTemplate>();
    
    public TextTemplate MakeTemplateFromFile(string templateFile)
    {
        if (templates.ContainsKey(templateFile))
        {
            return templates[templateFile];
        }
        
        TextTemplate engineTemplate = NewTextTemplate(templateFile);
        engine.TemplateFromStringTemplate(engineTemplate, File.ReadAllText(templateFile));
        engineTemplate.Tag = templateFile;
        
        return engineTemplate;
    }

    private TextTemplate MakeTemplateFromString(string template)
    {
        if (templates.ContainsKey(template))
        {
            return templates[template];
        }
        
        TextTemplate engineTemplate = NewTextTemplate(template);
        engine.TemplateFromStringTemplate(engineTemplate, template);
        engineTemplate.Tag = template;
        
        return engineTemplate;
    }

    private TextTemplate NewTextTemplate(string templateIdentifier)
    {
        TextTemplate engineTextTemplate;
        engineTextTemplate = templates
            .GetOrAdd(templateIdentifier, (hashOrFileName) => new TextTemplate());
        
        engineTextTemplate.Tag = templateIdentifier;
        
        return engineTextTemplate;
    }
}