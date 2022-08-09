using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Ball : GameObject
    {
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

        public Ball(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
            : base(game, graphics, spriteBatch) { }

        public void Initialize(int x, int y, int w, int h)
        {
            _drawRect = new Rectangle(0, 0, w, h);
            _position = new Vector2(x, y);
            _direction = Vector2.Normalize(Vector2.One);
            _speed = 0.2f;

            LoadContent();
        }

        override protected void LoadContent()
        {
            _texture = this.Game.Content.Load<Texture2D>("ball");
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
            Vector2 velocity = new Vector2(
                _direction.X * _speed * gameTime.ElapsedGameTime.Milliseconds,
                _direction.Y * _speed * gameTime.ElapsedGameTime.Milliseconds
            );
            _position += velocity;

            if (_position.X > court.width - _drawRect.Width)
            {
                _direction.X *= -1;

                if (BallExitedRight != null)
                {
                    BallExitedRight(this, EventArgs.Empty);
                }
            }
            else if (_position.X < 0)
            {
                _direction.X *= -1;

                if (BallExitedLeft != null)
                {
                    BallExitedLeft(this, EventArgs.Empty);
                }
            }

            if (
                isCollide(this.hitbox, paddleLeft.hitbox)
                || isCollide(this.hitbox, paddleRight.hitbox)
            )
            {
                _direction.X *= -1;
            }

            if (_position.Y > court.height - _drawRect.Height || _position.Y < 0)
            {
                _direction.Y *= -1;
            }
        }

        override public void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, this.hitbox, null, Color.White);
        }
    }
}
