using System;

namespace BoneTestGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (var game = new TestGame())
            {

                
                game.Run();
            }
        }
    }
#endif
}

