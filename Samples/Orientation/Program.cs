using System;

namespace Orientation
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (OrientationGame game = new OrientationGame())
            {
                game.Run();
            }
        }
    }
#endif
}

