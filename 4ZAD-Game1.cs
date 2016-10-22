using ClassLibrary2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using System;


namespace Game1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<Wall> Walls { get; set; }
        public List<Wall> Goals { get; set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
           {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500
            };
            Content.RootDirectory = "Content";
        }

        public Paddle PaddleBottom { get; private set; }
        public Paddle PaddleTop { get; private set; }
        public Ball Ball { get; private set; }
        public Background Background { get; private set; }
        public SoundEffect HitSound { get; private set; }
        public Song Music { get; private set; }
       
        private IGenericList<Sprite> SpritesForDrawList = new GenericList <Sprite>();

        protected override void Initialize()
        {
            var screenBounds = GraphicsDevice.Viewport.Bounds;

            PaddleBottom = new Paddle(GameConstants.PaddleDefaultWidth,
                GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);

            PaddleBottom.X = 500/2 - GameConstants.PaddleDefaultWidth/2;
            PaddleBottom.Y = 900 - GameConstants.PaddleDefaulHeight;

            PaddleTop = new Paddle(GameConstants.PaddleDefaultWidth,
                GameConstants.PaddleDefaulHeight, GameConstants.PaddleDefaulSpeed);

            PaddleTop.X = 500/2 - GameConstants.PaddleDefaultWidth/2;
            PaddleTop.Y = 0;

            Ball = new Ball(GameConstants.DefaultBallSize, GameConstants.DefaultInitialBallSpeed,
                GameConstants.DefaultBallBumpSpeedIncreaseFactor)
            {
                X = 250 - GameConstants.DefaultBallSize/2,
                Y = 450 - GameConstants.DefaultBallSize/2
            };

            Background = new Background ( 500 , 900);

            SpritesForDrawList.Add(Background);
            SpritesForDrawList.Add(PaddleBottom);
            SpritesForDrawList.Add(PaddleTop);
            SpritesForDrawList.Add(Ball);

            Walls = new List<Wall>()
            {
                new Wall (screenBounds.Left, screenBounds.Top - GameConstants.DefaultBallSize/2,
                            (int)GameConstants.WallDefaultSize, screenBounds.Height + GameConstants.DefaultBallSize/2),
                new Wall (screenBounds.Right, screenBounds.Top - GameConstants.DefaultBallSize/2,
                            (int)GameConstants.WallDefaultSize, screenBounds.Height + GameConstants.DefaultBallSize/2)
            };

            Goals = new List<Wall>()
            {
                new Wall (screenBounds.Left, screenBounds.Top - GameConstants.DefaultBallSize/2,
                            screenBounds.Width, (int)GameConstants.WallDefaultSize),
                new Wall (screenBounds.Left, screenBounds.Bottom + GameConstants.DefaultBallSize/2,
                            screenBounds.Width, (int)GameConstants.WallDefaultSize)
            };
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
            PaddleBottom.Texture = paddleTexture;
            PaddleTop.Texture = paddleTexture;
            Ball.Texture = Content.Load<Texture2D>("ball");
            Background.Texture = Content.Load<Texture2D>("background");

            HitSound = Content.Load<SoundEffect>("hit");
            Music = Content.Load<Song>("music");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Music);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            var screenBounds = GraphicsDevice.Viewport.Bounds;
            var touchState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (touchState.IsKeyDown(Keys.Left))
            {
                PaddleBottom.X -= (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            if (touchState.IsKeyDown(Keys.Right))
            {
                PaddleBottom.X += (float)(PaddleBottom.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            if (touchState.IsKeyDown(Keys.A))
            {
                PaddleTop.X -= (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            if (touchState.IsKeyDown(Keys.D))
            {
                PaddleTop.X += (float)(PaddleTop.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            if (PaddleBottom.X < 0)
            {
                PaddleBottom.X = 0;
            }

            if (PaddleBottom.X > 300)
            {
                PaddleBottom.X = 300;
            }

            if (PaddleTop.X < 0)
            {
                PaddleTop.X = 0;
            }

            if (PaddleTop.X > 300)
            {
                PaddleTop.X = 300;
            }


            var ballPositionChange = Ball.Direction *
                (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Ball.Speed);
            Ball.X += ballPositionChange.X;
            Ball.Y += ballPositionChange.Y;

            if (Walls.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Vector2 newDirection = new Vector2(-1,1);
                Ball.Direction = Ball.Direction*newDirection;
                Ball.Speed = Ball.Speed * Ball.BumpSpeedIncreaseFactor;
            }

            if (Goals.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Ball.X = screenBounds.Center.ToVector2().X;
                Ball.Y = screenBounds.Center.ToVector2().Y;
                Ball.Speed = GameConstants.DefaultInitialBallSpeed;
                HitSound.Play();
            }

            if (CollisionDetector.Overlaps(Ball, PaddleTop) && Ball.Direction.Y < 0
                || (CollisionDetector.Overlaps(Ball, PaddleBottom) && Ball.Direction.Y > 0))
            {
                Vector2 newDirection = new Vector2(1,-1);
                Ball.Direction = newDirection*Ball.Direction;
                Ball.Speed *= Ball.BumpSpeedIncreaseFactor;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            for (int i = 0; i < SpritesForDrawList.Count; i++)
            {
                SpritesForDrawList.GetElement(i).DrawSpriteOnScreen(spriteBatch);
            }

            /*Background.DrawSpriteOnScreen(spriteBatch);
            PaddleBottom.DrawSpriteOnScreen(spriteBatch);
            PaddleTop.DrawSpriteOnScreen(spriteBatch);
            Ball.DrawSpriteOnScreen(spriteBatch);*/
            spriteBatch.End();
            base.Draw(gameTime);
       }
    }

    public abstract class Sprite : IPhysicalObject2D, IComparable <Sprite>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Texture2D Texture { get; set; }

        protected Sprite(int width, int height, float x = 0, float y = 0)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }

        public virtual void DrawSpriteOnScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0,
            Width, Height), Color.White);
        }
        public abstract int CompareTo(Sprite obj);
    }

    public class Background : Sprite
    {
        public Background(int width, int height) : base(width, height)
        {
        }

        public override int CompareTo(Sprite obj)
        {
            Background obj1 = (Background)obj;
            return this.CompareTo(obj1);
        }
    }

    public class Ball : Sprite
    {
        public float Speed { get; set; }
        public float BumpSpeedIncreaseFactor { get; set; }
       
        public Vector2 Direction { get; set; }
        public Ball(int size, float speed, float
            defaultBallBumpSpeedIncreaseFactor) : base(size, size)
        {
            Speed = speed;
            BumpSpeedIncreaseFactor = defaultBallBumpSpeedIncreaseFactor;
            Direction = new Vector2(1, 1);
        }
        public override int CompareTo(Sprite obj)
        {
            Ball obj1 = (Ball)obj;
            return this.CompareTo(obj1);
        }
    }

    public class Paddle : Sprite
    {
        public float Speed { get; set; }
        public Paddle(int width, int height, float initialSpeed) : base(width,height)
        {
            Speed = initialSpeed;
        }

        public override void DrawSpriteOnScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), new Rectangle(0, 0,
            Width, Height), Color.GhostWhite);
        }
        public override int CompareTo(Sprite obj)
        {
            Paddle obj1 = (Paddle)obj;
            return this.CompareTo(obj1);
        }
    }

    public class GameConstants
    {
        public const float PaddleDefaulSpeed = 0.9f  ;
        public const int PaddleDefaultWidth = 200;
        public const int PaddleDefaulHeight = 20;
        public const float DefaultInitialBallSpeed = 0.4f ;
        public const float DefaultBallBumpSpeedIncreaseFactor = 1.05f ;
        public const int DefaultBallSize = 40;
        public const float WallDefaultSize=0f;
    }

    public interface IPhysicalObject2D
    {
        float X { get; set; }
        float Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }
    }

    public class CollisionDetector
    {
        public static bool Overlaps(IPhysicalObject2D a, IPhysicalObject2D b)
        {
            if (a.X <= b.X + b.Width &&
                a.X + a.Width >= b.X &&
                a.Y <= b.Y + b.Height &&
                a.Y + a.Height >= b.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public class Wall : IPhysicalObject2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Wall(float x, float y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

}
