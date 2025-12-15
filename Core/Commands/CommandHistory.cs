namespace Core.Commands
{
    /// <summary>
    /// Manages the undo/redo stack and command execution.
    /// This is a singleton that maintains the history of all executed commands.
    /// </summary>
    public class CommandHistory
    {
        private readonly Stack<ICommand> undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> redoStack = new Stack<ICommand>();
        private readonly int maxHistorySize;

        public event EventHandler? HistoryChanged;

        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        public string NextUndoDescription => CanUndo ? undoStack.Peek().Description : string.Empty;
        public string NextRedoDescription => CanRedo ? redoStack.Peek().Description : string.Empty;

        public CommandHistory(int maxHistorySize = 100)
        {
            this.maxHistorySize = maxHistorySize;
        }

        /// <summary>
        /// Executes a command and adds it to the undo stack.
        /// This clears the redo stack since we're creating a new timeline.
        /// </summary>
        public void ExecuteCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            try
            {
                command.Execute();

                // Try to merge with previous command if possible
                if (undoStack.Count > 0 &&
                    undoStack.Peek().CanMergeWith(command))
                {
                    var previousCommand = undoStack.Pop();
                    previousCommand.MergeWith(command);
                    undoStack.Push(previousCommand);
                }
                else
                {
                    undoStack.Push(command);

                    // Limit history size
                    if (undoStack.Count > maxHistorySize)
                    {
                        var commands = undoStack.ToArray();
                        undoStack.Clear();
                        for (int i = commands.Length - maxHistorySize; i < commands.Length; i++)
                        {
                            undoStack.Push(commands[i]);
                        }
                    }
                }

                // Clear redo stack - we've created a new timeline
                redoStack.Clear();

                OnHistoryChanged();
            }
            catch (Exception ex)
            {
                // Log error and notify user
                throw new CommandExecutionException(
                    $"Failed to execute command '{command.Description}'", ex);
            }
        }

        public void Undo()
        {
            if (!CanUndo)
                throw new InvalidOperationException("Nothing to undo");

            try
            {
                var command = undoStack.Pop();
                command.Undo();
                redoStack.Push(command);

                OnHistoryChanged();
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException("Failed to undo command", ex);
            }
        }

        public void Redo()
        {
            if (!CanRedo)
                throw new InvalidOperationException("Nothing to redo");

            try
            {
                var command = redoStack.Pop();
                command.Redo();
                undoStack.Push(command);

                OnHistoryChanged();
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException("Failed to redo command", ex);
            }
        }

        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
            OnHistoryChanged();
        }

        private void OnHistoryChanged()
        {
            HistoryChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class CommandExecutionException : Exception
    {
        public CommandExecutionException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}