using ThreeSheeps.Spritesse.Graphics;

namespace ThreeSheeps.Spritesse.Scene
{
    public interface IRenderableSceneObject
    {
        void RegisterRenderables(ISceneRendererService service);
    }
}
