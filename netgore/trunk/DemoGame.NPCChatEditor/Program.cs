using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Server.NPCChat.Conditionals;
using NetGore.EditorTools;

namespace DemoGame.NPCChatEditor
{
    static class Program
    {
        /// <summary>
        /// Forces loading the Server assembly. We need to do this to ensure we can do reflection on the
        /// NPCChatConditionalBase derived types.
        /// </summary>
        static void ForceLoadServerAssembly()
        {
            var typeToLoad = typeof(NPCChatConditional);

            var a = Assembly.GetAssembly(typeToLoad);
            if (a == null)
                throw new Exception("Why did the assembly fail to load...?");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            WinFormExceptionHelper.AddUnhandledExceptionHooks();
#endif

            ForceLoadServerAssembly();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}