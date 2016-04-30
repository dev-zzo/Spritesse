using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ThreeSheeps.Spritesse.Graphics;

namespace ThreeSheeps.Spritesse.Scene
{
    /// <summary>
    /// Manages the typical scene (backgrounds, objects, overlays)
    /// </summary>
    public class SceneComponent : GameComponent, ISceneService
    {
        public SceneComponent(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(ISceneService), this);
            this.content = new ContentManager(game.Services, game.Content.RootDirectory);
        }

        #region ISceneService implementation

        public ContentManager Content { get { return this.content; } }

        public void LoadContent()
        {
            ISceneRendererService renderer = this.Game.Services.GetService(typeof(ISceneRendererService)) as ISceneRendererService;
            foreach (ISceneObject obj in this.objects)
            {
                obj.LoadContent(this.Content);
                IRenderableSceneObject renderable = obj as IRenderableSceneObject;
                if (renderer != null && renderable != null)
                {
                    renderable.RegisterRenderables(renderer);
                }
            }
        }

        public void AddObject(ISceneObject obj)
        {
            this.objects.Add(obj);
        }

        public void RemoveObject(ISceneObject obj)
        {
            this.objects.Remove(obj);
        }

        public void Clear()
        {
            this.objects.Clear();
        }

        #endregion

        #region Game component overrides

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            foreach (ISceneObject obj in this.objects)
            {
                obj.Update(gameTime);
            }
        }

        #endregion

        private readonly ContentManager content;
        private readonly HashSet<ISceneObject> objects = new HashSet<ISceneObject>();
    }
}
