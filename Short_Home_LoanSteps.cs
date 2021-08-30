using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using TechTalk.SpecFlow;

namespace Auden_Test
{
    [Binding]
    public class Short_Home_LoanSteps
    {
        public static IWebDriver driver;
        
        [Given(@"User is at the Home Page")]
        public void GivenUserIsAtTheHomePage()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
          
            driver = new ChromeDriver(options);
            driver.Url = "https://www.auden.com/short-term-loan";
            //accept cookies popup. 
            IWebElement AcceptCookies = driver.FindElement(By.XPath("//button[@id='consent_prompt_submit']"));
            AcceptCookies.Click();

            //wait for the page to load
            WebDriverWait w = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            w.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-testid='loan-calculator-slider']")));
        }

        //set slider to 210 because default value is 200 and we are chosing any value other than default value

        [Given(@"Set Slider to 210")]
        public void SetSliderTo210()
        {
           IWebElement Slider = driver.FindElement(By.XPath("//*[@data-testid='loan-calculator-slider']"));
            Actions SliderAction = new Actions(driver);
            SliderAction.MoveToElement(Slider);
            SliderAction.Perform();
            SliderAction.ClickAndHold(Slider)
                .MoveByOffset((-(int)Slider.Size.Width / 2), 0)
                .MoveByOffset(40, 0).Release().Perform();
        }

        [When(@"User clicks a weekend date")]
        public void UserClicksAWeekendDate()
        {
            var date = DateTime.Now;
            var nextSunday = date.AddDays(7 - (int)date.DayOfWeek).Day;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
            Actions PickDate = new Actions(driver);
            IWebElement DatePicker = driver.FindElement(By.XPath("//button[@data-testid='loan-calculator-day-selector' and @value='" + nextSunday + "'][not(@disabled)]"));
            PickDate.MoveToElement(DatePicker).Click().Perform();
            IWebElement text = driver.FindElement(By.XPath("//h3[contains(text(),'how your loan would look')]"));
            PickDate.MoveToElement(text).Click().Perform();
        }


        [Then(@"Assert that the Schedule date is the friday before the selected sunday")]
        public void AssertScheduledDate()
        {
            
            
            var date = DateTime.Now;
            String fridayDate = date.AddDays(5 - (int)date.DayOfWeek).ToString("dddd d MMM yyyy");
            IWebElement scheduleDateElement = driver.FindElement(By.XPath("//label[contains(@class, 'first-repayment-date-radio-button')]//span"));
            String scheduleDate = scheduleDateElement.Text;
            //Assertion that the schedule date is the friday before the selected sunday  
            Assert.AreEqual(fridayDate, scheduleDate);
            
        }

        [When(@"User selects (min|max) value")]
        public void UserSelectsMinMaxValue(string value)
        {
            IWebElement Slider = driver.FindElement(By.XPath("//*[@data-testid='loan-calculator-slider']"));
            Actions SliderAction = new Actions(driver);
            SliderAction.MoveToElement(Slider);
            SliderAction.Perform();

            switch (value)
            {
                case "min":
                         SliderAction.ClickAndHold(Slider)
                        .MoveByOffset((-(int)Slider.Size.Width / 2), 0)
                        .MoveByOffset(0, 0).Release().Perform();
                        break;
                case "max":
                         SliderAction.ClickAndHold(Slider)
                        .MoveByOffset((-(int)Slider.Size.Width / 2), 0)
                        .MoveByOffset(480, 0).Release().Perform();
                        break;


            }
        }

        [Then(@"Assert that the Slider is moved to (£200|£500)")]
        public void AssertMinMaxValue(string value)
        {
            IWebElement AmountElement = driver.FindElement(By.XPath("//p[@data-testid='loan-amount-value']"));
            string Amount = AmountElement.Text;
            Assert.AreEqual(value, Amount);
        }
        [When(@"Slider set to 230")]
        public void SetSliderTo230()
        {
            IWebElement Slider = driver.FindElement(By.XPath("//*[@data-testid='loan-calculator-slider']"));
            Actions SliderAction = new Actions(driver);
            SliderAction.MoveToElement(Slider);
            SliderAction.Perform();
            SliderAction.ClickAndHold(Slider)
                .MoveByOffset((-(int)Slider.Size.Width / 2), 0)
                .MoveByOffset(120, 0).Release().Perform();
        }

        [Then(@"Assert that the slider amount is equal to Loan amount")]
        public void AssertSliderAmountWithLoanAmount()
        {
            IWebElement AmountElement = driver.FindElement(By.XPath("//p[@data-testid='loan-amount-value']"));
            string Amount = AmountElement.Text;
            IWebElement LoanAmountElement = driver.FindElement(By.XPath("//p[text()='You want to borrow']//strong[@data-testid='loan-calculator-summary-amount']"));
            string LoanAmount = LoanAmountElement.Text;
            LoanAmount = LoanAmount.Substring(0, 4);
            Actions SliderAction = new Actions(driver);
            SliderAction.MoveToElement(LoanAmountElement);
            SliderAction.Perform();
            Assert.AreEqual(LoanAmount, Amount);
        }

        [AfterTestRun]
        public static void AfterWebFeature()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}

