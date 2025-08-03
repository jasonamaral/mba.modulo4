namespace Api.Core.Identidade;

public class AppSettings
{
    public AppSettings(string autenticacaoJwksUrl)
    {
        AutenticacaoJwksUrl = autenticacaoJwksUrl;
    }

    public string AutenticacaoJwksUrl { get;}
}