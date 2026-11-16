using ConsoleSelenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ConsoleSelenium.Tests;

[TestClass]
[DoNotParallelize]
public sealed class WebFormularioDesafioTest
{
    private IWebDriver? driver;
    private WebDriverWait espera = null!;

    private IWebDriver Navegador => driver
        ?? throw new InvalidOperationException("O navegador ainda nao foi inicializado.");

    [TestInitialize]
    public void AbrirFormulario()
    {
        var opcoes = new ChromeOptions();
        if (ConfiguracaoAutomacao.ExecutarSemJanela)
        {
            opcoes.AddArgument("--headless=new");
        }

        opcoes.AddArgument("--window-size=1440,1000");
        driver = new ChromeDriver(opcoes);
        driver.Manage().Timeouts().PageLoad = ConfiguracaoAutomacao.TempoEspera;
        espera = new WebDriverWait(driver, ConfiguracaoAutomacao.TempoEspera);
        driver.Navigate().GoToUrl(ConfiguracaoAutomacao.UrlFormulario);
        espera.Until(navegador => navegador.FindElement(By.Name("my-text")).Displayed);
    }

    [TestCleanup]
    public void FecharNavegador()
    {
        try
        {
            driver?.Quit();
        }
        finally
        {
            driver?.Dispose();
            driver = null;
        }
    }

    [TestMethod]
    public void CadastroCompleto_DevePreencherCamposEReceberConfirmacao()
    {
        const string nomeCompleto = "Luiz Henrique Souza dos Santos";
        // Senha ficticia, exclusiva deste formulario demonstrativo.
        const string senha = "T3ste!Selenium#41_2026";
        const string experiencia =
            "Estou praticando testes automatizados com C#, MSTest e Selenium, " +
            "criando cenarios de interacao e assercoes para validar formularios web.";

        PreencherCampo("my-text", nomeCompleto);
        PreencherCampo("my-password", senha);
        PreencherCampo("my-textarea", experiencia);

        Assert.AreEqual(nomeCompleto, Campo("my-text").GetDomProperty("value"));
        Assert.AreEqual(senha, Campo("my-password").GetDomProperty("value"));
        Assert.AreEqual(experiencia, Campo("my-textarea").GetDomProperty("value"));
        Assert.AreEqual("Readonly input", Campo("my-readonly").GetDomProperty("value"),
            "O campo somente leitura deve preservar seu valor padrao.");

        EnviarFormularioEValidarSucesso();
    }

    [TestMethod]
    public void SeletoresEListas_DeveSelecionarTwoEPreencherSeattle()
    {
        var seletor = new SelectElement(Campo("my-select"));
        seletor.SelectByText("Two");

        Assert.AreEqual("2", seletor.SelectedOption.GetDomProperty("value"),
            "A opcao Two deve corresponder ao valor 2.");

        const string cidade = "Seattle";
        Assert.IsTrue(Navegador.FindElements(By.CssSelector("#my-options option"))
            .Any(opcao => opcao.GetDomProperty("value") == cidade),
            "A cidade deve estar entre as sugestoes do datalist.");
        PreencherCampo("my-datalist", cidade);
        Assert.AreEqual(cidade, Campo("my-datalist").GetDomProperty("value"));

        EnviarFormularioEValidarSucesso();
    }

    [TestMethod]
    public void EstadosDosControles_DeveMarcarCheckboxAlternarRadioEAlterarCor()
    {
        var primeiroCheckbox = Navegador.FindElement(By.Id("my-check-1"));
        var segundoCheckbox = Navegador.FindElement(By.Id("my-check-2"));
        Assert.IsTrue(primeiroCheckbox.Selected,
            "O primeiro checkbox deve iniciar marcado.");
        Assert.IsFalse(segundoCheckbox.Selected,
            "O segundo checkbox deve iniciar desmarcado.");

        segundoCheckbox.Click();
        Assert.IsTrue(segundoCheckbox.Selected);
        Assert.IsTrue(primeiroCheckbox.Selected);

        var primeiroRadio = Navegador.FindElement(By.Id("my-radio-1"));
        var segundoRadio = Navegador.FindElement(By.Id("my-radio-2"));
        Assert.IsTrue(primeiroRadio.Selected);
        segundoRadio.Click();
        Assert.IsTrue(segundoRadio.Selected);
        Assert.IsFalse(primeiroRadio.Selected,
            "Selecionar o segundo radio deve desmarcar o primeiro.");

        const string novaCor = "#336699";
        var seletorCor = Campo("my-colors");
        Assert.AreNotEqual(novaCor, seletorCor.GetDomProperty("value"));
        // O dialogo nativo de cor nao pertence ao DOM. Alteramos o input e
        // notificamos os eventos que uma escolha de cor normalmente dispara.
        ((IJavaScriptExecutor)Navegador).ExecuteScript(
            "arguments[0].value = arguments[1];" +
            "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));" +
            "arguments[0].dispatchEvent(new Event('change', { bubbles: true }));",
            seletorCor, novaCor);
        Assert.AreEqual(novaCor, seletorCor.GetDomProperty("value"));

        EnviarFormularioEValidarSucesso();
    }

    private IWebElement Campo(string nome) => Navegador.FindElement(By.Name(nome));

    private void PreencherCampo(string nome, string valor)
    {
        var campo = Campo(nome);
        campo.Clear();
        campo.SendKeys(valor);
    }

    private void EnviarFormularioEValidarSucesso()
    {
        Navegador.FindElement(By.CssSelector("button[type='submit']")).Click();
        var mensagem = espera.Until(navegador =>
        {
            var elemento = navegador.FindElement(By.Id("message"));
            return elemento.Displayed ? elemento : null;
        }) ?? throw new WebDriverTimeoutException("A mensagem de confirmacao nao foi encontrada.");

        Assert.AreEqual("Received!", mensagem.Text,
            "O formulario deve confirmar o recebimento apos a submissao.");
        Assert.AreEqual("/selenium/web/submitted-form.html", new Uri(Navegador.Url).AbsolutePath);
    }
}
