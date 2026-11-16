# ConsoleSelenium41

[![Testes Selenium](https://github.com/CobkCobinho/console-selenium41/actions/workflows/tests.yml/badge.svg)](https://github.com/CobkCobinho/console-selenium41/actions/workflows/tests.yml)

Atividade individual da Lista 41 de Gest�o e Qualidade de Software, professor Daniel Henrique Matos de Paiva.

**Autor:** Luiz Henrique Souza dos Santos.

Automa��o do [formul�rio oficial do Selenium](https://www.selenium.dev/selenium/web/web-form.html), utilizando **C#, .NET 10, MSTest e Selenium WebDriver**.

## Estrutura

- `ConsoleSelenium41.sln`: solu��o com os dois projetos.
- `ConsoleSelenium/`: aplica��o Console e configura��o compartilhada da automa��o (URL, espera e modo de execu��o).
- `ConsoleSelenium.Tests/`: projeto MSTest com refer�ncia ao projeto principal e pacotes Selenium.WebDriver e Selenium.Support.
- `ConsoleSelenium.Tests/WebFormularioDesafioTest.cs`: os tr�s m�todos de teste pedidos.
- `.github/workflows/tests.yml`: compila��o e execu��o autom�tica em cada push e pull request.

## Cen�rios implementados

| Exerc�cio | M�todo | Verifica��es |
| --- | --- | --- |
| 1 - Cadastro completo | `CadastroCompleto_DevePreencherCamposEReceberConfirmacao` | Nome completo, senha fict�cia, descri��o da experi�ncia, valor exato de Readonly input e mensagem Received! ap�s envio. |
| 2 - Seletores e listas | `SeletoresEListas_DeveSelecionarTwoEPreencherSeattle` | SelectElement seleciona Two com valor 2, Seattle pertence ao datalist, preenchimento e envio com sucesso. |
| 3 - Estados dos controles | `EstadosDosControles_DeveMarcarCheckboxAlternarRadioEAlterarCor` | Primeiro checkbox marcado inicialmente, segundo checkbox marcado por clique, altern�ncia de r�dio, altera��o da cor e envio com sucesso. |

## Como executar

Requisitos: SDK .NET 10, Google Chrome instalado e acesso � internet para NuGet, Selenium Manager e formul�rio.

Na pasta raiz:

```sh
dotnet restore ConsoleSelenium41.sln
dotnet build ConsoleSelenium41.sln --configuration Release --no-restore
dotnet test ConsoleSelenium41.sln --configuration Release --no-build
```

O Selenium Manager resolve o ChromeDriver compat�vel com o Chrome instalado. N�o � necess�rio manter um caminho fixo de driver no c�digo.

Por padr�o, os testes executam em modo headless (sem janela). Para acompanhar o navegador no PowerShell:

```powershell
$env:SELENIUM_HEADLESS = "false"
dotnet test ConsoleSelenium41.sln
```

Em Bash:

```sh
SELENIUM_HEADLESS=false dotnet test ConsoleSelenium41.sln
```

A aplica��o Console apresenta as informa��es da atividade e o comando de testes:

```sh
dotnet run --project ConsoleSelenium
```

## Organiza��o e decis�es

Cada teste abre sua pr�pria sess�o do Chrome e o encerramento ocorre no TestCleanup. As esperas expl�citas utilizam WebDriverWait, sem pausas fixas com Thread.Sleep.

Os seletores utilizam os atributos name e id reais da p�gina. O color picker � alterado por JavaScript no input, com eventos input e change: a janela nativa de escolha de cor fica fora do DOM e varia conforme o sistema operacional. O valor final � validado antes do envio.

A senha � um dado fict�cio de teste. O site demonstrativo envia o formul�rio via GET; n�o utilize credenciais reais.

## Valida��o e entrega

O GitHub Actions compila a solu��o e executa os tr�s testes contra o formul�rio real. O relat�rio TRX fica dispon�vel no artefato `resultados-selenium` de cada execu��o. Falhas de rede, indisponibilidade do formul�rio ou mudan�as no site podem afetar estes testes de integra��o.

Trabalho realizado individualmente. Reposit�rio p�blico para entrega: https://github.com/CobkCobinho/console-selenium41
