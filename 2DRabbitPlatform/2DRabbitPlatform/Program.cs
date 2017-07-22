using System;

namespace _2DRabbitPlatform
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            TestClass cls = new TestClass();
            cls.dosome();

            using (RabbitPlatform game = new RabbitPlatform())
            {
                game.Run();
            }
        }
    }
#endif
}

