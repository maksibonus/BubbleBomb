using System;

namespace Window_Designer
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MainClass game = new MainClass())
            {
                game.Run();
            }
        }
    }
#endif
}

