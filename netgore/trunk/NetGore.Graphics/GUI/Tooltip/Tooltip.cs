using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Handles displaying pop-up text for <see cref="Control"/>s after they have been hovered over by the cursor
    /// for the appropriate amount of time.
    /// </summary>
    public class Tooltip
    {
        readonly TooltipArgs _args = new TooltipArgs();
        readonly StyledTextsDrawer _drawer;
        readonly GUIManagerBase _guiManager;

        Color _bgColor = new Color(0, 0, 0, 200);
        ControlBorder _border;
        Vector2 _borderPadding = new Vector2(4, 4);
        Vector2 _borderSize;
        int _delay = 800;
        Vector2 _drawOffset = new Vector2(8, 8);
        SpriteFont _font;
        Color _fontColor = Color.White;
        bool _isVisible = true;
        int _lastRefreshTime;
        Control _lastUnderCursor;
        int _maxWidth = 150;
        int _retryDelay = 500;

        /// <summary>
        /// The time that the current control under the cursor started being hovered over.
        /// </summary>
        int _startHoverTime;

        int _timeout = 3000;

        bool _tooltipTimedOut = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tooltip"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager.</param>
        public Tooltip(GUIManagerBase guiManager)
        {
            if (guiManager == null)
                throw new ArgumentNullException("guiManager");

            _guiManager = guiManager;
            _font = guiManager.Font;
            _drawer = new StyledTextsDrawer(Font);

            // Set the Tooltip in the GUIManager
            GUIManager.Tooltip = this;

            Debug.Assert(GUIManager.Tooltip == this);
            Debug.Assert(Font != null);
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to draw the background of this <see cref="Tooltip"/>. This value
        /// is only valid for when the <see cref="Border"/> is null.
        /// </summary>
        public Color BackgroundColor
        {
            get { return _bgColor; }
            set { _bgColor = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="ControlBorder"/> for this <see cref="Tooltip"/>. Can be null.
        /// </summary>
        public ControlBorder Border
        {
            get { return _border; }
            set { _border = value; }
        }

        /// <summary>
        /// Gets or sets number of pixels the border is padded on each side, where the X value corresponds to the
        /// number of pixels padded on the left and right side, and the Y value corresponds to the top and bottom.
        /// </summary>
        public Vector2 BorderPadding
        {
            get { return _borderPadding; }
            set { _borderPadding = value; }
        }

        /// <summary>
        /// Gets or sets the delay in milliseconds for the tooltip to be shown once a <see cref="Control"/> is
        /// hovered over. If this value is zero, the tooltip will be displayed immediately. If this value is set
        /// to less than zero, zero will be used instead.
        /// </summary>
        public int Delay
        {
            get { return _delay; }
            set { _delay = Math.Max(0, value); }
        }

        /// <summary>
        /// Gets or sets the offset from the cursor position to draw the <see cref="Tooltip"/>.
        /// </summary>
        public Vector2 DrawOffset
        {
            get { return _drawOffset; }
            set { _drawOffset = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SpriteFont"/> used by this <see cref="Tooltip"/>. By default, this value will
        /// be equal to the <see cref="GUIManager"/>'s Font. Cannot be null.
        /// </summary>
        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (_font == value)
                    return;

                _font = value;
                _drawer.Font = _font;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to draw the text when no color is specified.
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        /// <summary>
        /// Gets the GUIManager that this <see cref="Tooltip"/> is for.
        /// </summary>
        public GUIManagerBase GUIManager
        {
            get { return _guiManager; }
        }

        /// <summary>
        /// Gets if the <see cref="Tooltip"/> is currently being displayed.
        /// </summary>
        public bool IsDisplayed
        {
            get { return IsVisible && _drawer.Texts != null && !_tooltipTimedOut; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Tooltip"/> is currently visible. If false, the <see cref="Tooltip"/> will
        /// not make requests for the tooltip text from <see cref="Control"/>s and will not be drawn.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        /// <summary>
        /// Gets or sets the maximum width of the Tooltip.
        /// </summary>
        public int MaxWidth
        {
            get { return _maxWidth; }
            set { _maxWidth = value; }
        }

        /// <summary>
        /// Gets or sets the delay in milliseconds between retry attempts in getting the tooltip text. This
        /// only applies to when the <see cref="Control"/> returns a null collection for the tooltip text.
        /// </summary>
        public int RetryGetTooltipDelay
        {
            get { return _retryDelay; }
            set { _retryDelay = value; }
        }

        /// <summary>
        /// Gets or sets the timeout in milliseconds for the tooltip. After the tooltip has been displayed
        /// for the given amount of time, it will hide. If this value is zero, the tooltip will be displayed
        /// indefinitely. If the value is set to less than zero, zero will be used instead.
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = Math.Max(0, value); }
        }

        /// <summary>
        /// Draws the <see cref="Tooltip"/>.
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw the <see cref="Tooltip"/> with.</param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (sb == null)
                throw new ArgumentNullException("sb");

            // Do nothing if not being displayed
            if (!IsDisplayed)
                return;

            Vector2 pos = GUIManager.CursorPosition + DrawOffset + BorderPadding;

            // Draw the border
            Rectangle borderRect = new Rectangle((int)(pos.X - BorderPadding.X), (int)(pos.Y - BorderPadding.Y),
                                                 (int)(_borderSize.X + (BorderPadding.X * 2)),
                                                 (int)(_borderSize.Y + (BorderPadding.Y * 2)));

            ControlBorder b = _args.Border;
            if (b != null)
                b.Draw(sb, borderRect);
            else
                XNARectangle.Draw(sb, borderRect, _args.BackgroundColor);

            // Draw the text
            _drawer.Draw(sb, _args.FontColor, pos);
        }

        void RefreshText(int currentTime)
        {
            // Request the tooltip text
            var tooltipTexts = _lastUnderCursor.Tooltip.Invoke(_lastUnderCursor, _args);
            _lastRefreshTime = currentTime;

            // If the tooltip text is null, increase the _startHoverTime to result in the needed retry delay
            if (tooltipTexts == null)
                _startHoverTime = currentTime - Delay + RetryGetTooltipDelay;
            else
            {
                if (tooltipTexts.Count() > 0)
                {
                    var texts = StyledText.ToMultiline(tooltipTexts, false, Font, MaxWidth);
                    _drawer.SetStyledTexts(texts);
                    UpdateBackground();
                }
            }
        }

        /// <summary>
        /// Updates the <see cref="Tooltip"/>.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        public virtual void Update(int currentTime)
        {
            // Do nothing if not visible
            if (!IsVisible)
                return;

            Control currentUnderCursor = GUIManager.UnderCursor;

            // Check if the control under the cursor has changed
            if (_lastUnderCursor != currentUnderCursor)
            {
                _lastUnderCursor = currentUnderCursor;
                _startHoverTime = currentTime;
                _drawer.ClearTexts();
                _tooltipTimedOut = false;
            }

            // Check for no control under the cursor or the control has no tooltip
            if (currentUnderCursor == null || currentUnderCursor.Tooltip == null || _tooltipTimedOut)
                return;

            if (_drawer.Texts != null)
            {
                // If we already have the text, check if it is time to hide it
                if (_args.Timeout <= 0)
                    return;

                int timeoutTime = _startHoverTime + Delay + _args.Timeout;
                if (currentTime > timeoutTime)
                    _tooltipTimedOut = true;

                // Check to refresh the text
                if (_args.RefreshRate > 0 && currentTime - _lastRefreshTime > _args.RefreshRate)
                    RefreshText(currentTime);
            }
            else
            {
                // Check if enough time has elapsed for the tooltip to be displayed
                if (currentTime - _startHoverTime > Delay)
                {
                    _args.RestoreDefaults(this);
                    RefreshText(currentTime);
                }
            }
        }

        /// <summary>
        /// Handles updating the information needed for background of the <see cref="Tooltip"/>.
        /// </summary>
        protected virtual void UpdateBackground()
        {
            float maxWidth = _drawer.Texts.Max(x => x.Sum(y => y.GetWidth(Font)));
            float height = _drawer.Texts.Count() * Font.LineSpacing;
            _borderSize = new Vector2(maxWidth, height);
        }
    }
}