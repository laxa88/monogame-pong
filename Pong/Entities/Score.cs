using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Score : GameObject
    {
        private SpriteFont _font;
        private int _scoreLeft;
        private int _scoreRight;

        public Score(Game game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
            : base(game, graphics, spriteBatch) { }

        override public void Initialize()
        {
            base.Initialize();

            _scoreLeft = 0;
            _scoreRight = 0;

            LoadContent();
        }

        override protected void LoadContent()
        {
            _font = this.Game.Content.Load<SpriteFont>("score");
        }

        public void Draw(GameTime gameTime, Court court)
        {
            String str = $"{_scoreLeft} - {_scoreRight}";
            Vector2 rect = _font.MeasureString(str);
            _spriteBatch.DrawString(
                _font,
                str,
                new Vector2(court.width / 2 - rect.X / 2, 10),
                Color.White
            );
        }

        public void AddScore(int player)
        {
            if (player == 1)
            {
                _scoreLeft += 1;
            }
            else if (player == 2)
            {
                _scoreRight += 1;
            }
        }
    }
}
