using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;


namespace Pinner
{
    class WebPinner
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting....");
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("load-extension=C:/Users/Ben/AppData/Local/Google/Chrome/User Data/Default/Extensions/gkbhgdhhefdphpikedbinecandoigdel/3.5.7_0");
            IWebDriver driver = new ChromeDriver(options);
            //driver.Manage().Window.Maximize();

            WebPinner.TailwindLogin(driver);
            
            WebPinner.getS6Products(driver);

            Console.ReadLine();
            
            //driver.Quit();
        }

        static private IWebDriver TailwindLogin(IWebDriver driver) {
            string home = "https://tailwindapp.com/login";
            driver.Navigate().GoToUrl(home);

            IWebElement username = driver.FindElement(By.Name("email"));
            IWebElement password = driver.FindElement(By.Name("password"));
            IWebElement login = driver.FindElement(By.ClassName("btn-login"));

            username.SendKeys("oberholzer.sam@gmail.com");
            password.SendKeys("31ndyh177");

            login.Click();
            
            return driver;

        }

        static private Boolean getS6Products(IWebDriver driver)  {
            string home = "https://society6.com/samanndesigns/duvet-covers?sort=new";
            driver.Navigate().GoToUrl(home);

            IList<IWebElement> links = new List<IWebElement>();

            try {
                links = driver.FindElements(By.ClassName("title_product_1YKi0"));
            } catch(NoSuchElementException e) {
                IWebElement advert = driver.FindElement(By.ClassName("bx-close-link"));
                advert.Click();
            } finally {
                links = driver.FindElements(By.ClassName("title_product_1YKi0"));
            }
            
            Console.WriteLine("Number of Links: "+links.Count());

            int count = 0;
            string url = "";
            List<string> productLinks = new List<string>();
                        
            foreach(IWebElement item in links) {
                try {
                    url = item.GetAttribute("href");
                }
                catch (NoSuchElementException e) {
                    IWebElement advert = driver.FindElement(By.ClassName("bx-close-link"));
                    advert.Click();
                }
                finally {
                    url = item.GetAttribute("href");
                }
                
                productLinks.Add(url);
                Console.WriteLine(url);                
            }

            Actions clicker = new Actions(driver);
            foreach(string prodLink in productLinks) {
                Console.WriteLine("going to product");
                driver.Navigate().GoToUrl(prodLink);
                Thread.Sleep(2000);

                //

                // get all the buttons and then use the first one
                WebPinner.ClickScheduleButton(driver);
                //clicker.MoveToElement(scheduleButton).Click().Perform();
                
                Thread.Sleep(2500);

                Console.WriteLine("going home");
                driver.Navigate().GoToUrl(home);
                //WebPinner.OpenNewTab(item.GetAttribute("href"), driver);
                Thread.Sleep(2500);

                if (++count > 5) {
                    break;
                }
            }

            return true;
        }

        public static void ClickScheduleButton(IWebDriver driver) {
            IList<IWebElement> buttons = driver.FindElements(By.XPath("//*[@id='tw_schedule_btn']"));
            IWebElement scheduleButton = buttons.ElementAt(0);

            TimeSpan tspan = new TimeSpan(100);
            WebDriverWait wait = new WebDriverWait(driver, tspan);
//            wait.Until(ExpectedConditions.visibilityOfElementLocated(By.XPath("//*[@id='tw_schedule_btn']")));
            wait.Until(WebPinner.ElementIsVisibleAndEnabled(scheduleButton));
            
            Actions actions = new Actions(driver);
            actions.MoveToElement(scheduleButton).Perform();
            actions.MoveToElement(scheduleButton).Click().Perform();
        }

        public static Func<IWebDriver, bool> ElementIsVisibleAndEnabled(IWebElement element) {
            return (driver) =>  {
                try {
                    return element.Displayed && element.Enabled;
                }
                catch (Exception) {
                    // If element is null, stale or if it cannot be located
                    return false;
                }
            };
        }

        static private Boolean OpenNewTab(string url, IWebDriver driver)
        {
            /*IWebElement element = driver.FindElement(By.TagName("body"));
            
            element.SendKeys(OpenQA.Selenium.Keys.Control + "t");
            driver.SwitchTo().Window(driver.WindowHandles.Last());*/
            

            driver.Url = "https://society6.com/samanndesigns/duvet-covers?sort=new";
            Thread.Sleep(2500);
            // do stuff
            //driver.SwitchTo().Window(driver.WindowHandles.First());
            return true;
        }
    }
}
