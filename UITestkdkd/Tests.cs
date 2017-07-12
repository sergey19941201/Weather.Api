using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;

namespace UITestkdkd
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            // TODO: If the Android app being tested is included in the solution then open
            // the Unit Tests window, right click Test Apps, select Add App Project
            // and select the app projects that should be tested.
            app = ConfigureApp
                .Android
                // TODO: Update this path to point to your Android app and uncomment the
                // code if the app is not included in the solution.
                //.ApkFile ("../../../Android/bin/Debug/UITestsAndroid.apk")
                .StartApp();
        }

        [Test]
        public void AppLaunches()
        {
            //app.Repl();
            app.Tap(c => c.Marked("startBn"));
            app.EnterText(c => c.Marked("cityNameET"), "kiev");
            app.Tap(c => c.Marked("EnterYourCity"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Edit"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Kiev"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Settings"));
            app.WaitForElement(c => c.Marked("Celsius"));
            app.Tap(c => c.Marked("Celsius"));
            app.Tap(c => c.Marked("FahrenheitRB"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Edit"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Kiev"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Home"));
            app.EnterText(c => c.Marked("cityNameET"), "london");
            app.Tap(c => c.Marked("EnterYourCity"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Report"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.EnterText(c => c.Marked("reportET"), "KEK");
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Settings"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Home"));
            app.EnterText(c => c.Marked("cityNameET"), "london");
            app.Tap(c => c.Marked("EnterYourCity"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Settings"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Edit"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Settings"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Report"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Settings"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Edit"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Home"));
            app.EnterText(c => c.Marked("cityNameET"), "london");
            app.Tap(c => c.Marked("EnterYourCity"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Edit"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Report"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Home"));
            app.EnterText(c => c.Marked("cityNameET"), "london");
            app.Tap(c => c.Marked("EnterYourCity"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Report"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Edit"));
            app.Tap(c => c.Marked("deleteBn"));
            app.Tap(c => c.Marked("No"));
            app.Tap(c => c.Marked("deleteBn"));
            app.Tap(c => c.Marked("Yes"));
            app.WaitForElement(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("leftDrawerBN"));
            app.Tap(c => c.Marked("Home"));
            app.Tap(c => c.Marked("get_address_button"));
            //app.Screenshot("First screen.");
        }
    }
}

