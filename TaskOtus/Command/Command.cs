using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskOtus.State;

namespace TaskOtus.Command
{
    public class Command : ICommand
    {
        protected Action _action;
        protected IState _state;

        public IState State => _state;

        public Command(Action action, IState state = default)
        {
            _action = action;
            _state = state ?? new DefaultState();
        }
        public void Execute()
        {
            ExecuteCom();
            _state.WritelineState();
        }

        public virtual void ExecuteCom()
        {
            _action();
        }
    }
}
