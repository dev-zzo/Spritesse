using Microsoft.Xna.Framework.Input;

namespace ThreeSheeps.Spritesse.Input
{
    /// <summary>
    /// Represents a more functional in-game input handler.
    /// </summary>
    /// <typeparam name="TAction">An enum type representing bindable actions.</typeparam>
    public interface IInputManagerService<TAction>
    {
        /// <summary>
        /// Bind an action to a specific key.
        /// </summary>
        /// <param name="action">Action to be bound to</param>
        /// <param name="key">Key to be bound</param>
        void SetKeyBinding(TAction action, Keys key);
        /// <summary>
        /// Query the currently bound key for an action.
        /// </summary>
        /// <param name="action">Action to query for</param>
        /// <returns>The key if bound, null otherwise</returns>
        Keys? GetKeyBinding(TAction action);

        /// <summary>
        /// Query whether an action has been activated (e.g. the key has just been pressed).
        /// This is an "edge triggered" event.
        /// </summary>
        /// <param name="action">Action to query for</param>
        bool HasBeenActivated(TAction action);

        /// <summary>
        /// Query whether an action has been deactivated (e.g. the key has just been released).
        /// This is an "edge triggered" event.
        /// </summary>
        /// <param name="action">Action to query for</param>
        bool HasBeenDeactivated(TAction action);

        /// <summary>
        /// Query whether an action is active (the key is pressed down).
        /// This is a "level triggered" event.
        /// </summary>
        /// <param name="action">Action to query for</param>
        /// <returns>True if the action is currently active, false otherwise (also if not bound to any key)</returns>
        bool IsActive(TAction action);
    }
}
