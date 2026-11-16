# ConsoleSelenium41

[![Testes Selenium](https://github.com/CobkCobinho/console-selenium41/actions/workflows/tests.yml/badge.svg)](https://github.com/CobkCobinho/console-selenium41/actions/workflows/tests.yml)

Atividade individual da Lista 41 de Gestão e Qualidade de Software, professor Daniel Henrique Matos de Paiva.

**Autor:** Luiz Henrique Souza dos Santos.

Automação do [formulário oficial do Selenium](https://www.selenium.dev/selenium/web/web-form.html), utilizando **C#, .NET 10, MSTest e Selenium WebDriver**.

## Estrutura

- `ConsoleSelenium41.sln`: solução com os dois projetos.
- `ConsoleSelenium/`: aplicação Console e configuração compartilhada da automação (URL, espera e modo de execução).
- `ConsoleSelenium.Tests/`: projeto MSTest com referência ao projeto principal e pacotes Selenium.WebDriver e Selenium.Support.
- `ConsoleSelenium.Tests/WebFormularioDesafioTest.cs`: os três métodos de teste pedidos.
- `.github/workflows/tests.yml`: compilação e execução automática em cada push e pull request.

## Cenários implementados

### 1. Cadastro completo

Método: `CadastroCompleto_DevePreencherCamposEReceberConfirmacao`

Preenche nome completo, senha fictícia e descrição da experiência com testes automatizados. Valida o valor exato de `Readonly input` e a mensagem `Received!` após o envio.

### 2. Seletores e listas

Método: `SeletoresEListas_DeveSelecionarTwoEPreencherSeattle`

Utiliza `SelectElement` para selecionar `Two` e verifica o valor `2`. Confirma que `Seattle` pertence ao datalist, preenche a cidade e valida o envio com sucesso.

### 3. Estados dos controles

Método: `EstadosDosControles_DeveMarcarCheckboxAlternarRadioEAlterarCor`

Valida o primeiro checkbox marcado por padrão, marca o segundo por clique, alterna a seleção do rádio, altera a cor e confirma o sucesso do envio.

## Como executar

Requisitos: SDK .NET 10, Google Chrome instalado e acesso à internet para NuGet, Selenium Manager e formulário.

Na pasta raiz:

```sh
dotnet restore ConsoleSelenium41.sln
dotnet build ConsoleSelenium41.sln --configuration Release --no-restore
dotnet test ConsoleSelenium41.sln --configuration Release --no-build
```

O Selenium Manager resolve o ChromeDriver compatível com o Chrome instalado. Não é necessário manter um caminho fixo de driver no código.

Por padrão, os testes executam em modo headless (sem janela). Para acompanhar o navegador no PowerShell:

```powershell
$env:SELENIUM_HEADLESS = "false"
dotnet test ConsoleSelenium41.sln
```

Em Bash:

```sh
SELENIUM_HEADLESS=false dotnet test ConsoleSelenium41.sln
```

A aplicação Console apresenta as informações da atividade e o comando de testes:

```sh
dotnet run --project ConsoleSelenium
```

## Organização e decisões

Cada teste abre sua própria sessão do Chrome e o encerramento ocorre no `TestCleanup`. As esperas explícitas utilizam `WebDriverWait`, sem pausas fixas com `Thread.Sleep`.

Os seletores utilizam os atributos `name` e `id` reais da página. O color picker é alterado por JavaScript no input, com eventos `input` e `change`: a janela nativa de escolha de cor fica fora do DOM e varia conforme o sistema operacional. O valor final é validado antes do envio.

A senha é um dado fictício de teste. O site demonstrativo envia o formulário via GET; não utilize credenciais reais.

## Validação e entrega

O GitHub Actions compila a solução e executa os três testes contra o formulário real. O relatório TRX fica disponível no artefato `resultados-selenium` de cada execução. Falhas de rede, indisponibilidade do formulário ou mudanças no site podem afetar estes testes de integração.

Trabalho realizado individualmente. [Repositório público para entrega](https://github.com/CobkCobinho/console-selenium41).
