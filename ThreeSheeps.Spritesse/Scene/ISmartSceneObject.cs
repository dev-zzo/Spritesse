using ThreeSheeps.Spritesse.Behaviour;

namespace ThreeSheeps.Spritesse.Scene
{
    public interface ISmartSceneObject
    {
        IBehaviour Behaviour { get; set; }
    }
}
