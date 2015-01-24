using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Attribute for a method that handles a console command.
    /// </summary>
    public class ConsoleCommandAttribute : StringCommandBaseAttribute
    {
        public ConsoleCommandAttribute(string command) : base(command)
        {
        }
    }
}