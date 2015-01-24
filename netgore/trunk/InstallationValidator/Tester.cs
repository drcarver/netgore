using System;

namespace InstallationValidator
{
    public static class Tester
    {
        static bool _hasErrors = false;

        /// <summary>
        /// Gets if there has been an error in any of the tests.
        /// </summary>
        public static bool HasErrors
        {
            get { return _hasErrors; }
        }

        /// <summary>
        /// The main entry point for running a test.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <param name="passed">True if the test passed; otherwise false.</param>
        /// <param name="failMessage">If the test failed, this is the message that will be displayed. This should contain
        /// why the test failed, and how to resolve it so it will pass.</param>
        public static void Test(string testName, bool passed, string failMessage)
        {
            if (!passed)
                _hasErrors = true;

            const ConsoleColor normalColor = ConsoleColor.White;
            const ConsoleColor passColor = ConsoleColor.Green;
            const ConsoleColor failColor = ConsoleColor.Red;
            const ConsoleColor testColor = ConsoleColor.Yellow;
            const ConsoleColor msgColor = ConsoleColor.Red;

            if (Console.CursorLeft > 0)
                Write("\n", normalColor);

            Write("[", normalColor);
            Write(passed ? "PASS" : "FAIL", passed ? passColor : failColor);
            Write("] ", normalColor);

            Write(testName, testColor);

            if (!passed && !string.IsNullOrEmpty(failMessage))
            {
                Write(" - ", normalColor);
                Write(failMessage, msgColor);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Writes a string to the test output.
        /// </summary>
        /// <param name="msg">The message to write. Line breaks must be entered manually with \r.</param>
        /// <param name="color">The color of the message.</param>
        public static void Write(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
        }
    }
}