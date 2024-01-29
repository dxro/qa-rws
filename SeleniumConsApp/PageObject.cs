using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumConsApp
{
    internal class PageObject
    {
        public string? href { get; set; }
        public PageObject(ChromeDriver driver)
        {
            IWebElement contentElement = FindBody(driver);
            IReadOnlyCollection<IWebElement> paragraps = FindParagraphs(contentElement);
            IWebElement? firstLink = null;

            foreach (IWebElement paragraph in paragraps)
            {
                try
                {
                    IReadOnlyCollection<IWebElement> links = FindLinks(paragraph);
                    foreach (IWebElement link in links)
                    {
                        href = link.GetAttribute("href");
                        if (IsValidLink(href))
                        {
                            firstLink = link;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                }
                if (firstLink != null)
                {
                    break;
                }
            }
            if (firstLink == null)
            {
                driver.Quit();
                throw new ArgumentException("This page does not contain any valid link !!!");
            }
        }

        private static IReadOnlyCollection<IWebElement> FindLinks(IWebElement paragraph)
        {
            return paragraph.FindElements(By.TagName("a"));
        }

        private static IReadOnlyCollection<IWebElement> FindParagraphs(IWebElement contentElement)
        {
            try
            {
                return contentElement.FindElements(By.TagName("p"));

            }
            catch (Exception)
            {

                throw new ArgumentException("This page does not contain any paragraph !!!");
            }
        }

        private static IWebElement FindBody(ChromeDriver driver)
        {
            try
            {
                return driver.FindElement(By.XPath("//div[@id='mw-content-text']"));
            }
            catch (Exception)
            {

                throw new ArgumentException("This page does not contain any content !!!");
            }
        }

        static bool IsValidLink(string href)
        {
            if (href.Contains("cite_note")) return false;
            if (href.Contains("Help:IPA")) return false;
            if (href.Contains("Help:Pronunciation")) return false;
            if (href.Contains("wikimedia.org")) return false;
            if (href.Contains("File:")) return false;
            return true;
        }
    }

}
