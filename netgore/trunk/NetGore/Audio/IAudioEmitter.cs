using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Interface for an object that can be the source of audio.
    /// </summary>
    public interface IAudioEmitter
    {
        /// <summary>
        /// Gets the position of the audio emitter.
        /// </summary>
        Vector2 Position { get; }
    }
}