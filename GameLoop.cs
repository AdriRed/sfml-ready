using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace SFMLReady
{
    public abstract class GameLoop
    {
        private int TargetFPS;
        private float TimeUntilUpdate;

        public RenderWindow Window
        {
            get;
            protected set;
        }

        public GameTime GameTime
        {
            get;
            protected set;
        }

        public Color ClearColor
        {
            get;
            protected set;
        }

        protected GameLoop(uint width, uint height, string title, Color clearColor, int targetFPS = 60)
        {
            TargetFPS = targetFPS;
            TimeUntilUpdate = 1f / TargetFPS;
            ClearColor = clearColor;
            Window = new RenderWindow(new VideoMode(width, height), title);
            GameTime = new GameTime();
            Window.Closed += Window_Closed;
        }


        protected void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }

        public void Run()
        {
            float timeBeforeUpdate = 0f;
            float timeElapsed = 0f;

            Clock clock;

            LoadContent();
            Initialize();

            clock = new Clock();

            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                float nextTimeElapsed = clock.ElapsedTime.AsSeconds();
                timeBeforeUpdate += nextTimeElapsed - timeElapsed;
                timeElapsed = nextTimeElapsed;

                if (timeBeforeUpdate >= TimeUntilUpdate)
                {
                    GameTime.Update(timeBeforeUpdate, clock.ElapsedTime.AsSeconds());
                    Update(GameTime);

                    timeBeforeUpdate = 0f;

                    Window.Clear(ClearColor);
                    Draw(GameTime);
                    Window.Display();
                }
            }
        }

        public abstract void LoadContent();
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);


    }
}
