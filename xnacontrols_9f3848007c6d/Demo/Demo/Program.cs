using System;

namespace Demo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (RamGecXNAControlsDemo game = new RamGecXNAControlsDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

