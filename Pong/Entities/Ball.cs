using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Ball : GameObject
    {
        private Vector2 _initialPosition;
        private float _initialSpeed;

        protected Texture2D _texture;
        protected Rectangle _drawRect;
        private Vector2 _direction;
        private float _speed;

        public Rectangle hitbox
        {
            get
            {
                return new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
                    _drawRect.Width,
                    _drawRect.Height
                );
            }
        }

        public event EventHandler BallExitedLeft;
        public event EventHandler BallExitedRight;

        public Ball(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch) { }

        public void Initialize(int x, int y, int w, int h)
        {
            _drawRect = new Rectangle(0, 0, w, h);
            _initialPosition = new Vector2(x, y);
            _initialSpeed = 0.4f;

            LoadContent();
        }

        override protected void LoadContent()
        {
            _texture = this.Game.Content.Load<Texture2D>("ball");
            Sound.LoadSoundEffect(Constants.SFX_BOUNCE);
        }

        private bool isCollide(Rectangle r1, Rectangle r2)
        {
            return (
                r1.X < r2.X + r2.Width
                && r1.X + r1.Width > r2.X
                && r1.Y < r2.Y + r2.Height
                && r1.Y + r1.Height > r2.Y
            );
        }

        public void Update(GameTime gameTime, Court court, Paddle paddleLeft, Paddle paddleRight)
        {
            if (!_active)
            {
                return;
            }

            Vector2 velocity = new Vector2(
                _direction.X * _speed * gameTime.ElapsedGameTime.Milliseconds,
                _direction.Y * _speed * gameTime.ElapsedGameTime.Milliseconds
            );
            _position += velocity;

            if (_position.X > court.width)
            {
                Deactivate();
                Sound.PlaySfx(Constants.SFX_BOUNCE);
                _direction.X *= -1;

                if (BallExitedRight != null)
                {
                    BallExitedRight(this, EventArgs.Empty);
                }
            }
            else if (_position.X < 0 - _drawRect.Width)
            {
                Deactivate();
                Sound.PlaySfx(Constants.SFX_BOUNCE);
                _direction.X *= -1;

                if (BallExitedLeft != null)
                {
                    BallExitedLeft(this, EventArgs.Empty);
                }
            }

            if (_direction.X < 0 && isCollide(this.hitbox, paddleLeft.hitbox))
            {
                Sound.PlaySfx(Constants.SFX_BOUNCE);
                _direction.X = Math.Abs(_direction.X);
            }
            else if (_direction.X > 0 && isCollide(this.hitbox, paddleRight.hitbox))
            {
                Sound.PlaySfx(Constants.SFX_BOUNCE);
                _direction.X = -Math.Abs(_direction.X);
            }

            if (_direction.Y < 0 && _position.Y < 0)
            {
                Sound.PlaySfx(Constants.SFX_BOUNCE);
                _direction.Y = Math.Abs(_direction.Y);
            }
            else if (_direction.Y > 0 && _position.Y > court.height - _drawRect.Height)
            {
                Sound.PlaySfx(Constants.SFX_BOUNCE);
                _direction.Y = -Math.Abs(_direction.Y);
            }
        }

        override public void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, this.hitbox, null, Color.White);
        }

        /// <summary>
        /// Resets the ball position and speed. Does not change active state.
        /// </summary>
        public void Reset(int direction)
        {
            _position = _initialPosition;

            double angle = new Random().NextDouble();
            var newDirection = new Vector2(
                (float)(Math.Cos(angle) * (double)direction),
                (float)(Math.Sin(angle * 2.0 - 1.0))
            );
            _direction = Vector2.Normalize(newDirection);

            _speed = _initialSpeed;
        }
    }
}
