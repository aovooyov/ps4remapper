using PS4Remapper.Hooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS4Remapper.MouseConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MOUSE HOOK");

            var mouse = new MouseHook();
            mouse.MouseEvent += _mouse_MouseEvent;

            Console.ReadKey();

            mouse.Dispose();

            Console.ReadKey();
        }

        private static void _mouse_MouseEvent(object sender, Hooks.EventArgs.MouseHookEventArgs e)
        {
            Console.WriteLine($"{e.MouseData.Point.X} {e.MouseData.Point.Y}");
        }
    }
}
