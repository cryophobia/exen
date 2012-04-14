using System;
using Microsoft.Xna.Framework;

namespace CatGirls
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
			Point size = NamedScreenSizes.Get(args.Length > 0 ? args[0] : null);

            using (CatGirlsGame game = new CatGirlsGame(size))
            {
                game.Run();
            }
        }
    }
#endif
}

