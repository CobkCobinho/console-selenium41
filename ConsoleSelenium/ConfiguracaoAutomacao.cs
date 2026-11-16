namespace ConsoleSelenium;

public static class ConfiguracaoAutomacao
{
    public const string UrlFormulario = "https://www.selenium.dev/selenium/web/web-form.html";
    public static readonly TimeSpan TempoEspera = TimeSpan.FromSeconds(30);

    public static bool ExecutarSemJanela =>
        !string.Equals(Environment.GetEnvironmentVariable("SELENIUM_HEADLESS"),
            "false", StringComparison.OrdinalIgnoreCase);
}
