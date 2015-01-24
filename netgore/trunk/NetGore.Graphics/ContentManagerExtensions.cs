using System.Linq;
using Microsoft.Xna.Framework.Content;
using NetGore.IO;

namespace NetGore.Graphics
{
    public static class ContentManagerExtensions
    {
        public static T Load<T>(this ContentManager contentManager, ContentAssetName contentAssetName)
        {
            return contentManager.Load<T>(contentAssetName.Value);
        }
    }
}