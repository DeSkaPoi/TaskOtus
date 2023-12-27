using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskOtus.State;

namespace TaskOtus.Command
{
    public class MoveToCommand : Command, ICommand
    {
        public MoveToCommand(Action action, IState state) : base(action, state)
        {
        }

        public override void ExecuteCom() 
        {
            _state.Handle();
            _action.Invoke();
        }
    }
}
