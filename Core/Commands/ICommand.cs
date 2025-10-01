using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands
{
    /// <summary>
    /// Base interface for all undoable commands.
    /// Represents a single atomic operation that can be executed, undone, and redone.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command, performing its action on the scene
        /// </summary>
        void Execute();

        /// <summary>
        /// Reverses the command's effects
        /// </summary>
        void Undo();

        /// <summary>
        /// Re-applies the command after it was undone
        /// </summary>
        void Redo();

        /// <summary>
        /// Human-readable description for UI display (e.g., "Add Brick Element")
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Whether this command can be merged with another (for optimization)
        /// </summary>
        bool CanMergeWith(ICommand other);

        /// <summary>
        /// Merge this command with another similar command
        /// </summary>
        void MergeWith(ICommand other);
    }

}
