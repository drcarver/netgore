using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.Audio;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Base of a game screen for the <see cref="ScreenManager"/>. Each screen should ideally be independent of
    /// one another.
    /// </summary>
    public abstract class GameScreen
    {
        readonly string _name;

        bool _playMusic = true;

        IMusic _screenMusic;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameScreen"/> class.
        /// </summary>
        /// <param name="name">Unique name of the screen that can be used to identify and
        /// call it from other screens</param>
        protected GameScreen(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets the <see cref="MusicManager"/> to use for the music to play on this <see cref="GameScreen"/>.
        /// </summary>
        public MusicManager MusicManager
        {
            get { return ScreenManager.MusicManager; }
        }

        /// <summary>
        /// Gets the unique name of this screen.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets or sets if music is to be played during this screen. If true, the <see cref="IMusic"/> specified
        /// in <see cref="ScreenMusic"/> will be played. If false, any music will be turned off while this screen
        /// is active.
        /// </summary>
        public bool PlayMusic
        {
            get { return _playMusic; }
            set
            {
                // Value changed?
                if (_playMusic == value)
                    return;

                // Update value and update music
                _playMusic = value;
                UpdateMusic();
            }
        }

        /// <summary>
        /// Gets the <see cref="ScreenManager"/> that manages this <see cref="GameScreen"/>.
        /// </summary>
        public ScreenManager ScreenManager { get; internal set; }

        /// <summary>
        /// Gets or sets the <see cref="IMusic"/> to play while this screen is active. Only valid if
        /// <see cref="PlayMusic"/> is set. If null, the track will not be changed, preserving the music
        /// currently playing.
        /// </summary>
        public IMusic ScreenMusic
        {
            get { return _screenMusic; }
            set
            {
                // Value changed?
                if (_screenMusic == value)
                    return;

                // Update value and update music
                _screenMusic = value;
                UpdateMusic();
            }
        }

        /// <summary>
        /// Gets the <see cref="SoundManager"/> to use for the sound to play on this <see cref="GameScreen"/>.
        /// </summary>
        public SoundManager SoundManager
        {
            get { return ScreenManager.SoundManager; }
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on <see cref="Deactivate"/>().
        /// </summary>
        public virtual void Activate()
        {
            UpdateMusic();
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in <see cref="Activate"/>().
        /// </summary>
        public virtual void Deactivate()
        {
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the 
        /// active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime.</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with an additional time for
        /// every time XNA notifies the ScreenManager that the game content needs to be reloaded.
        /// </summary>
        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// Handles the unloading of game content. This is raised whenever XNA notifies the ScreenManager
        /// that the content is to be unloaded.
        /// </summary>
        public virtual void UnloadContent()
        {
        }

        /// <summary>
        /// Handles updating of the screen. This will only be called while the screen is the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Updates the currently playing music based off the values defined by <see cref="PlayMusic"/>
        /// and <see cref="ScreenMusic"/>.
        /// </summary>
        void UpdateMusic()
        {
            // Make sure we have the screen enabled
            if (ScreenManager == null || ScreenManager.ActiveScreen != this)
                return;

            // Turn off music
            if (!PlayMusic)
            {
                MusicManager.Stop();
                return;
            }

            // Change track
            if (ScreenMusic != null)
                ScreenMusic.Play();
        }
    }
}