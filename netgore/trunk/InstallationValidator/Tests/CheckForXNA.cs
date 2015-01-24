namespace InstallationValidator.Tests
{
    public class CheckForXNA : ITestable
    {
        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "XNA 3.1 installed";
            const string failInfo = "Please install XNA 3.1";

            Tester.Test(testName, AssemblyHelper.IsAssemblyInstalled(KnownAssembly.Xna), failInfo);
        }

        #endregion
    }
}