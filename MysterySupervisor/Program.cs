using System;
using MysteryHelper;

namespace MysteryInstaller
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            ConsoleHelper.HelloWorld();
            ConsoleHelper.WriteLine();
            ConsoleHelper.PressAnyKeyToExit();
        }
    }
}