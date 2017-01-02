using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;

namespace UserInteractionsTutorial
{
    [TestFixture]
    public class InteractionsDemo
    {

        private ChromeDriver _driver = new ChromeDriver(@"C:\Users\Kat\Documents\Visual Studio 2015\Projects\3_frameworki\UserInteractionsTutorial\Drivers");
        private Actions _actions;
        private WebDriverWait _wait;

        [Test]
        public void DragAndDropExample()
        {
            //var driver = new FirefoxDriver();
            var driver = new ChromeDriver(@"C:\Users\Kat\Documents\Visual Studio 2015\Projects\3_frameworki\UserInteractionsTutorial\Drivers");
            var actions = new Actions(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));

            driver.Navigate().GoToUrl("http://jqueryui.com/droppable/");
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.ClassName("demo-frame")));

            IWebElement targetElement = driver.FindElement(By.Id("droppable"));
            IWebElement sourceElement = driver.FindElement(By.Id("draggable"));

            //actions.DragAndDrop(sourceElement, targetElement).Perform();
            var x = actions.DragAndDrop(sourceElement, targetElement).Build();
            x.Perform();
            Assert.AreEqual("Dropped!", targetElement.Text);
        }

        [Test]
        public void DragAndDropExample2()
        {
            _driver.Navigate().GoToUrl("http://jqueryui.com/droppable/");
            _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.ClassName("demo-frame")));

            IWebElement targetElement = _driver.FindElement(By.Id("droppable"));
            IWebElement sourceElement = _driver.FindElement(By.Id("draggable"));

            var drag = _actions
                .ClickAndHold(sourceElement)
                .MoveToElement(targetElement)
                .Release(targetElement)
                .Build();

            drag.Perform();

            //actions.DragAndDrop(sourceElement, targetElement).Perform();
            //var x = _actions.DragAndDrop(sourceElement, targetElement).Build();
            //x.Perform();
            Assert.AreEqual("Dropped!", targetElement.Text);
        }

        [Test]
        public void DragAndDropExample3()
        {
            _driver.Navigate().GoToUrl("http://www.pureexample.com/jquery-ui/basic-droppable.html");
            _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("ExampleFrame-94")));
            //or:
            //_wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath(".//*[@id='ExampleFrame-94']")));

            //this is not allowed cause this is compound (div's) class name:
            //IWebElement sourceElement = _driver.FindElement(By.ClassName("square ui-draggable"));
            IWebElement sourceElement = _driver.FindElement(By.XPath(".//*[@class='square ui-draggable']"));
            IWebElement targetElement = _driver.FindElement(By.XPath(".//*[@class='squaredotted ui-droppable']"));


            var drag = _actions
                .ClickAndHold(sourceElement)
                .MoveToElement(targetElement)
                .Release(targetElement)
                .Build();

            drag.Perform();

            IWebElement info = _driver.FindElement(By.Id("info"));
            Assert.AreEqual("dropped!", info.Text);
            //same as:
            //var info2 = _driver.FindElement(By.Id("info")).Text;
            //Assert.AreEqual("dropped!", info2);
        }

        [Test]
        public void ResizingExample()
        {
            _driver.Navigate().GoToUrl("http://jqueryui.com/resizable/");
            _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.ClassName("demo-frame")));

            IWebElement resizeHandle = _driver.FindElement(By.XPath(".//*[@class='ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se']"));

            _actions.ClickAndHold(resizeHandle).MoveByOffset(100, 100).Perform();

            Assert.IsTrue(_driver.FindElement(By.XPath(".//*[@id='resizable' and @style]")).Displayed);
            System.Threading.Thread.Sleep(1000);
        }

        [Test]
        public void OpenNetworkTabUsingFirefox()
        {
            //doesn't work in firefox
            var driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://www.google.com");
            _actions.KeyDown(Keys.Control).KeyDown(Keys.Shift).SendKeys("Q").Perform();
            _actions.KeyDown(Keys.Control).KeyDown(Keys.Shift).Perform();

            driver.Navigate().GoToUrl("http://www.pureexample.com/jquery-ui/basic-droppable.html");
            System.Threading.Thread.Sleep(1000);
        }

        [Test]
        public void OpenNetworkTabUsingChrome()
        {
            _driver.Navigate().GoToUrl("http://www.google.com");
            _actions.KeyDown(Keys.Control).KeyDown(Keys.Shift).SendKeys("I").Perform();
            _actions.KeyDown(Keys.Control).KeyDown(Keys.Shift).Perform();

            _driver.Navigate().GoToUrl("http://www.pureexample.com/jquery-ui/basic-droppable.html");
            System.Threading.Thread.Sleep(1000);
        }

        [Test]
        public void DragDropHtml5()
        {
            _driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/drag_and_drop");
            var source = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("column-a")));
            var jsFile = File.ReadAllText((@"C:\Users\Kat\Documents\Visual Studio 2015\Github\drag_and_drop_helper.js"));

            IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
            js.ExecuteScript(jsFile + "$('#column-a').simulateDragDrop({dropTarget: '#column-b'});");
            //IWebElement boxA = _driver.FindElement(By.XPath(".//*[@id='column-a']"));
            //IWebElement boxB = _driver.FindElement(By.XPath(".//*[@id='column-b']"));
            //var dragDrop = _actions
            //    .ClickAndHold(boxA)
            //    .MoveByOffset(100,0)
            //    .Release(boxB)
            //    .Build();
            //dragDrop.Perform();
            //System.Threading.Thread.Sleep(3000);

            //IWebElement divWithBoxes = _driver.FindElement(By.XPath(".//*[@id='columns']/div[2]"));
            var header = _driver.FindElement(By.XPath("//*[@id='column-b']/header")).Text;
            Assert.AreEqual("A", header);
            System.Threading.Thread.Sleep(10000);
        }

        [Test]
        public void DrawQuiz()
        {
            _driver.Navigate().GoToUrl("http://www.compendiumdev.co.uk/selenium/gui_user_interactions.html");
            //_wait.Until(ExpectedConditions.ElementIsVisible(By.Id("canvas")));
            _driver.Manage().Window.Maximize();

            //IWebElement canvas = _driver.FindElement(By.Id("canvas"));

            //_actions.MoveToElement(canvas).ClickAndHold().MoveByOffset(20, 20).Release().Build().Perform();
            //_actions.MoveToElement(canvas).ClickAndHold().MoveByOffset(1, 0).Release().Build().Perform();
            //_actions.MoveToElement(canvas).ClickAndHold().MoveByOffset(2, 0).Release().Build().Perform();
            //_actions.MoveToElement(canvas).ClickAndHold().MoveByOffset(4, 0).Release().Build().Perform();

            AddingOffsets();
            var someEvent = _driver.FindElement(By.XPath(".//*[@id='keyeventslist']/li[1]"));
            Assert.IsTrue(someEvent.Displayed);
            System.Threading.Thread.Sleep(10000);
        }

        public void AddingOffsets()
        {
            //int offsetY = 0;
            //int offsetX = 0;
            for (int offsetX = 0; offsetX < 10; )
            {
                for (int offsetY = 0; offsetY < 5; offsetY++)
                {
                    IWebElement canvas = _driver.FindElement(By.Id("canvas"));
                    _actions.MoveToElement(canvas).ClickAndHold().MoveByOffset(offsetX, offsetY).Release().Build().Perform();

                }
                offsetX++;
            }
        }

        [SetUp]
        public void Setup()
        {
            //_driver = new ChromeDriver(@"C:\Users\Kat\Documents\Visual Studio 2015\Projects\3_frameworki\UserInteractionsTutorial\Drivers");
            _actions = new Actions(_driver);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Close();
            _driver.Quit();
        }
    }
}
