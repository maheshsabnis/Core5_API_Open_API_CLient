using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blaz_AppForTest.Pages;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace Blaz_AppForTest.Pages.Tests
{
	[TestClass()]
	public class Tests
	{

		public Tests()
		{
			testContext = new Bunit.TestContext();
			loginProcessor = new Mock<ILoginProcessor>();
		}
		 
		private Bunit.TestContext testContext;
		private Mock<ILoginProcessor> loginProcessor;
		 
		[TestMethod()]
		public void No_Email_Password()
		{
			// mock the DI service
			testContext.Services.AddScoped(x => loginProcessor.Object);
			// render the component
			var component = testContext.RenderComponent<Index>();
			// verify the text exist on the page
			NUnit.Framework.Assert.IsTrue(component.Markup.Contains("<h1>Hello, world!</h1>"));
			// check if the button exist
			var buttons = component.FindAll("button");
			// check if only one button
			NUnit.Framework.Assert.AreEqual(1, buttons.Count);
			// check if the button is submit button
			var submit = buttons.FirstOrDefault(b => b.OuterHtml.Contains("Submit"));
			NUnit.Framework.Assert.IsNotNull(submit);
			// raise the onclick event
			// this will call the login function
			submit.Click();
			// verify the no email and no password condition to be executed successfully
			loginProcessor.Verify(l => l.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}

		[TestMethod()]
		public void Proper_Email_Password_Condition()
		{
			// mock the DI service
			testContext.Services.AddScoped(x => loginProcessor.Object);
			// render the component
			var component = testContext.RenderComponent<Index>();

			// check if the button exist
			var buttons = component.FindAll("button");
			// check if only one button
			NUnit.Framework.Assert.AreEqual(1, buttons.Count);
			// check if the button is submit button
			var submit = buttons.FirstOrDefault(b => b.OuterHtml.Contains("Submit"));

			// read the text elements based on id selector
			var email = component.Find("#agentEmail");
			// set value using chage() method this will call the onchange event
			email.Change("testuser");
			var password = component.Find("#agentPassword");
			password.Change("testpassword");
			// set the return false for the login method
			loginProcessor.Setup(l => l.Login(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
			
			// dispatch lik event
			submit.Click();
			// verify the email and  password condition to be executed successfully
			loginProcessor.Verify(l => l.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
			// search the div tag with alert class
			var alert = component.Find("div.alert");
			// check if the div contains 'Invalid Login' message
			NUnit.Framework.Assert.AreEqual("Invalid Login", alert.InnerHtml);

		}
	}
}

//namespace Balz_ComponentTest
//{
	 
//	public class Tests
//	{
//		private Bunit.TestContext testContext;
//		private Mock<ILoginProcessor> loginProcessor;
//		[SetUp]
//		public void Setup()
//		{
//			testContext = new Bunit.TestContext();
//			loginProcessor = new Mock<ILoginProcessor>();
//		}
//		[TearDown]
//		public void Teardown()
//		{
//			testContext.Dispose();
//		}

//		[Test]
//		public void No_Email_Password()
//		{
//			// mock the DI service
//			testContext.Services.AddScoped(x=>loginProcessor.Object);
//			// render the component
//			var component = testContext.RenderComponent<Index>();
//			// verify the text exist on the page
//			Assert.IsTrue(component.Markup.Contains("<h1> Hello, world! </h1>"));
//			// check if the button exist
//			var buttons = component.FindAll("button");
//			// check if only one button
//			Assert.AreEqual(1, buttons.Count);
//			// check if the button is submit button
//			var submit = buttons.FirstOrDefault(b => b.OuterHtml.Contains("Subit"));
//			Assert.IsNotNull(submit);
//			// raise the onclick event
//			// this will call the login function
//			submit.Click();
//			// verify the no email and no password condition to be executed successfully
//			loginProcessor.Verify(l => l.Login(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
//		}
//	}
//}
