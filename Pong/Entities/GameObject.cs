using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class GameObject : DrawableGameComponent
    {
        protected SpriteBatch _spriteBatch;
        protected Vector2 _position;
        protected bool _active;

        public GameObject(Game game, SpriteBatch spriteBatch) : base(game)
        {
            _spriteBatch = spriteBatch;
        }

        public void Activate()
        {
            this._active = true;
        }

        public void Deactivate()
        {
            this._active = false;
        }
    }
}
