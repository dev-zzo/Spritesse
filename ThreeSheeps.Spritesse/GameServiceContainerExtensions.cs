using Microsoft.Xna.Framework;

namespace ThreeSheeps.Spritesse
{
    public static class GameServiceContainerExtensions
    {
        public static T GetService<T>(this GameServiceContainer container)
        {
            return (T)container.GetService(typeof(T));
        }
    }
}
