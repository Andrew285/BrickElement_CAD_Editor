using System.Runtime.InteropServices;

namespace App.Utils.ConsoleLogging
{
    public static class ConsoleAllocator
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public static void Create()
        {
            AllocConsole();
        }
    }
}
