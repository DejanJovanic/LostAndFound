using Client.Model.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Model
{
    class CommandMachine : BindableBase, IExecutor
    {
        private Stack<IItemServiceCommand> _Undo;
        private Stack<IItemServiceCommand> _Redo;

        public CommandMachine()
        {
            _Undo = new Stack<IItemServiceCommand>();
            _Redo = new Stack<IItemServiceCommand>();
        }

        public bool IsRedoAvailable
        {
            get
            {
                return _Redo.Count > 0;
            }

        }
        public bool IsUndoAvailable
        {
            get
            {
                return _Undo.Count > 0;
            }
        }
        public async Task<IItemReturnValue> Undo()
        {
            if (_Undo.Count > 0)
            {
                var command = _Undo.Pop();
                OnPropertyChanged("IsUndoAvailable");
                var ret = await command.UnexecuteAsync();

                if (ret.Response == Response.OK)
                {
                    _Redo.Push(command);
                    OnPropertyChanged("IsRedoAvailable");
                }
                return ret;
            }
            else
            {
                return null;
            }
        }

        public async Task<IItemReturnValue> Do(IItemServiceCommand command)
        {
            var ret = await command.ExecuteAsync();
            if (ret.Response == Response.OK)
            {
                _Undo.Push(command);
                _Redo.Clear();
                OnPropertyChanged("IsUndoAvailable");
                OnPropertyChanged("IsRedoAvailable");
            }

            return ret;
        }

        public async Task<IItemReturnValue> Redo()
        {

            if (_Redo.Count > 0)
            {
                var command = _Redo.Pop();
                OnPropertyChanged("IsRedoAvailable");
                var ret = await command.ExecuteAsync();
                if (ret.Response == Response.OK)
                {
                    _Undo.Push(command);
                    OnPropertyChanged("IsUndoAvailable");

                }
                return ret;
            }
            else
            {
                return null;
            }
        }
    }
}
