using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;

namespace DemoGame.ParticleEffectEditor
{
    public partial class ScreenForm : Form, IGetTime
    {
        readonly Stopwatch _watch = new Stopwatch();

        ContentManager _content;
        ParticleEmitter _emitter;
        IParticleRenderer _renderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenForm"/> class.
        /// </summary>
        public ScreenForm()
        {
            InitializeComponent();
            _watch.Start();
        }

        public ParticleEmitter Emitter
        {
            get { return _emitter; }
            set
            {
                if (_emitter == value)
                    return;

                _emitter = value;

                pgEffect.SelectedObject = Emitter;
            }
        }

        GraphicsDevice GraphicsDevice
        {
            get { return GameScreen.GraphicsDevice; }
        }

        /// <summary>
        /// Main entry point for all the screen drawing.
        /// </summary>
        public void DrawGame()
        {
            // Clear the background
            GraphicsDevice.Clear(Color.Black);
            _renderer.RenderEmitter(Emitter);
        }

        void GameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Emitter.Origin = new Vector2(e.X, e.Y);
        }

        void ScreenForm_Load(object sender, EventArgs e)
        {
            GameScreen.ScreenForm = this;

            _content = new ContentManager(GameScreen.Services, ContentPaths.Build.Root);
            GrhInfo.Load(ContentPaths.Build, _content);

            _renderer = new SpriteBatchRenderer(GraphicsDevice);
            Emitter = new ParticleEmitter
            { SpriteCategorization = new SpriteCategorization("Particle", "skull"), Origin = new Vector2(GameScreen.Width, GameScreen.Height) / 2f };
        }

        /// <summary>
        /// Main entry point for all the screen content updating.
        /// </summary>
        public void UpdateGame()
        {
            Emitter.Update(GetTime());
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public int GetTime()
        {
            return (int)_watch.ElapsedMilliseconds;
        }

        #endregion
    }
}