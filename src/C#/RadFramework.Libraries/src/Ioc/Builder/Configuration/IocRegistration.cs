namespace RadDevelopers.Servers.Web.Config;

public interface IocRegistration
{
    string TKey { get; set; }
    string TImplementation { get; set; }
    string Lifecycle { get; set; }
}