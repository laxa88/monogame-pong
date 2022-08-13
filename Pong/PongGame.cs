using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    enum GameState
    {
        GAME_READY,
        GAME_ACTIVE,
        GAME_END
    }

    public class PongGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderBuffer;
        private Rectangle _renderRectangle;

        private Court _court;
        private Ball _ball;
        private Paddle _paddleLeft;
        private Paddle _paddleRight;
        private Score _score;
        private GameState _gameState;
        private float _readyTimeout;

        public PongGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;

            // Don't initialize local variables here because Monogame stuff
            // hasn't been initialized yet, e.g. GraphicsDevice
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Note: Doesn't work if you set in constructor
            Window.Title = "My Pong Game";

            // Initialize variables first
            Sound.Initialize(this);
            Sound.LoadSoundEffect(Constants.SFX_ROUND_END);

            Music.Initialize(this);
            Music.LoadMusic(Constants.BGM);
            Music.PlayMusic(Constants.BGM, true);

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _renderBuffer = new RenderTarget2D(GraphicsDevice, 640, 480);

            _court = new Court(this, _graphics, _spriteBatch, 640, 480);
            _court.Initialize();

            _ball = new Ball(this, _graphics, _spriteBatch);
            _ball.Initialize(_court.width / 2 - 10, _court.height / 2 - 10, 20, 20);
            _ball.BallExitedLeft += OnLeftPlayerWin;
            _ball.BallExitedRight += OnRightPlayerWin;

            _paddleLeft = new Paddle(this, _graphics, _spriteBatch);
            _paddleLeft.Initialize(10, (_court.height / 2) - 50, 10, 100);
            _paddleRight = new Paddle(this, _graphics, _spriteBatch);
            _paddleRight.Initialize(_court.width - 20, (_court.height / 2) - 50, 10, 100);

            _score = new Score(this, _graphics, _spriteBatch);
            _score.Initialize();

            // Add callbacks

            Window.ClientSizeChanged += OnWindowSizeChange;
            OnWindowSizeChange(null, null);

            // Start game
            ResetGame();
        }

        private void ResetGame()
        {
            _gameState = GameState.GAME_READY;
            _readyTimeout = 2000f;
            _ball.Reset();
            _paddleLeft.Reset();
            _paddleRight.Reset();
        }

        private void StartGame()
        {
            _gameState = GameState.GAME_ACTIVE;
            _readyTimeout = 0f;
            _ball.Activate();
        }

        private void EndGame()
        {
            _gameState = GameState.GAME_END;
            Sound.PlaySfx(Constants.SFX_ROUND_END);
        }

        private void OnLeftPlayerWin(object sender, EventArgs e)
        {
            _score.AddScore(1);
            EndGame();
        }

        private void OnRightPlayerWin(object sender, EventArgs e)
        {
            _score.AddScore(2);
            EndGame();
        }

        private void OnWindowSizeChange(object sender, EventArgs e)
        {
            var width = Window.ClientBounds.Width;
            var height = Window.ClientBounds.Height;

            if (height < width / (float)_renderBuffer.Width * _renderBuffer.Height)
            {
                width = (int)(height / (float)_renderBuffer.Height * _renderBuffer.Width);
            }
            else
            {
                height = (int)(width / (float)_renderBuffer.Width * _renderBuffer.Height);
            }

            var x = (Window.ClientBounds.Width - width) / 2;
            var y = (Window.ClientBounds.Height - height) / 2;
            _renderRectangle = new Rectangle(x, y, width, height);
        }

        protected override void LoadContent()
        {
            // Load global content here if necessary

            // Note that this is protected, so LoadContent within GameObjects
            // should be invoked within themselves whenever necessary.
        }

        private void UpdatePaddles(KeyboardState kstate, GameTime gameTime)
        {
            if (kstate.IsKeyDown(Keys.W))
                _paddleLeft.MoveUp(gameTime);

            if (kstate.IsKeyDown(Keys.S))
                _paddleLeft.MoveDown(gameTime, _court);

            if (kstate.IsKeyDown(Keys.Up))
                _paddleRight.MoveUp(gameTime);

            if (kstate.IsKeyDown(Keys.Down))
                _paddleRight.MoveDown(gameTime, _court);
        }

        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || kstate.IsKeyDown(Keys.Escape)
            )
            {
                Exit();
            }

            switch (_gameState)
            {
                case GameState.GAME_READY:
                    _readyTimeout -= +gameTime.ElapsedGameTime.Milliseconds;
                    if (_readyTimeout <= 0)
                    {
                        StartGame();
                    }
                    UpdatePaddles(kstate, gameTime);
                    break;

                case GameState.GAME_ACTIVE:
                    UpdatePaddles(kstate, gameTime);
                    _ball.Update(gameTime, _court, _paddleLeft, _paddleRight);
                    break;

                case GameState.GAME_END:
                default:
                    ResetGame();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Target buffer screen (original resolution)
            GraphicsDevice.SetRenderTarget(_renderBuffer);
            GraphicsDevice.Clear(Color.Black);

            // Draw everything game-related on this buffer screen.
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            _court.Draw(gameTime);
            _paddleLeft.Draw(gameTime);
            _paddleRight.Draw(gameTime);
            _ball.Draw(gameTime);
            _score.Draw(gameTime, _court);
            _spriteBatch.End();

            // Target main window, reset background colour
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the buffer screen onto the main window,
            // resized with PointClamp to create pixel-perfect effect.
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            _spriteBatch.Draw(_renderBuffer, _renderRectangle, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
