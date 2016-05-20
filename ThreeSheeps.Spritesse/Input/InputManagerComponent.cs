using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ThreeSheeps.Spritesse.Input
{
    public class InputManagerComponent<TAction> : GameComponent, IInputManagerService<TAction>
    {
        public InputManagerComponent(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IInputManagerService<TAction>), this);
        }

        public void SetKeyBinding(TAction action, Keys key)
        {
            this.keyBindings[action] = key;
        }

        public Keys? GetKeyBinding(TAction action)
        {
            Keys key;
            if (this.keyBindings.TryGetValue(action, out key))
                return key;
            return null;
        }

        public bool HasBeenActivated(TAction action)
        {
            Keys key;
            if (this.keyBindings.TryGetValue(action, out key))
            {
                return this.currentKeyboardState[key] == KeyState.Down && this.previousKeyboardState[key] == KeyState.Up;
            }
            return false;
        }

        public bool IsActive(TAction action)
        {
            Keys key;
            if (this.keyBindings.TryGetValue(action, out key))
            {
                return this.currentKeyboardState[key] == KeyState.Down;
            }
            return false;
        }

        public bool HasBeenDeactivated(TAction action)
        {
            Keys key;
            if (this.keyBindings.TryGetValue(action, out key))
            {
                return this.currentKeyboardState[key] == KeyState.Up && this.previousKeyboardState[key] == KeyState.Down;
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            this.previousKeyboardState = this.currentKeyboardState;
            this.currentKeyboardState = Keyboard.GetState();
        }

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private readonly Dictionary<TAction, Keys> keyBindings = new Dictionary<TAction, Keys>();
    }
}
