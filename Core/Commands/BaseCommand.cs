using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands
{
    /// <summary>
    /// Base abstract class providing common functionality for commands
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        public abstract string Description { get; }

        public abstract void Execute();
        public abstract void Undo();

        public virtual void Redo()
        {
            // By default, redo is the same as execute
            Execute();
        }

        public virtual bool CanMergeWith(ICommand other)
        {
            // Most commands can't be merged
            return false;
        }

        public virtual void MergeWith(ICommand other)
        {
            throw new NotSupportedException("This command does not support merging");
        }
    }
}
