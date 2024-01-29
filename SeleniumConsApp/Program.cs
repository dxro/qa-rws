using OpenQA.Selenium.Chrome;
using SeleniumConsApp;

ChromeDriverService service = ChromeDriverService.CreateDefaultService();
service.EnableVerboseLogging = false;
service.SuppressInitialDiagnosticInformation = true;
service.HideCommandPromptWindow = true;
ChromeOptions options = new();
var driver = new ChromeDriver(service, options)
{
    Url = "https://en.wikipedia.org/wiki/Special:Random"
};
string[] commandLineArgs = Environment.GetCommandLineArgs();
if (commandLineArgs.Length > 1) driver.Url = commandLineArgs[1];//for specified starting page
//driver.Url = "https://en.wikipedia.org/wiki/British_passport";
Console.WriteLine("Start Page = " + driver.Title);
string? href;
int transitions = 0;
do
{
    PageObject page = new(driver);
    href = page.href;
    Console.WriteLine(" href = " + href);
    driver.Url = href; //jump to link page
    Thread.Sleep(100);
    transitions++;

} while (href != "https://en.wikipedia.org/wiki/Philosophy");
Console.WriteLine("Number of transitions = " + transitions);
driver.Quit();
