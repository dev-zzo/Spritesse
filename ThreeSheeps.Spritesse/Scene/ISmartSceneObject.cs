using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeSheeps.Spritesse.Scene
{
    public interface ISmartSceneObject
    {
        IBehaviour Behaviour { get; set; }
    }
}
