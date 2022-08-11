using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    public class Music
    {
        private static Music _instance;
        public static Music instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Music();
                }
                return _instance;
            }
        }

        private Dictionary<string, Song> _songs;
        private Game _game;

        public Music()
        {
            _songs = new Dictionary<string, Song>();
        }

        public static void Initialize(Game game)
        {
            instance._game = game;
        }

        public static void LoadMusic(string filename)
        {
            try
            {
                instance._songs.Add(filename, instance._game.Content.Load<Song>(filename));
            }
            catch (ArgumentException)
            {
                Debug.Print($"Duplicate music file loaded: {filename}");
            }
            catch (ContentLoadException)
            {
                Debug.Print($"File not found: {filename} (is it defined in Content.mgcb?)");
            }
        }

        public static void PlayMusic(string filename, bool repeat = false)
        {
            MediaPlayer.Play(instance._songs[filename]);
            MediaPlayer.IsRepeating = repeat;
        }
    }
}
