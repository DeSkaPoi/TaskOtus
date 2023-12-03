using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOtus
{
    public class Command : ICommand
    {
        public Action _action;
        public Command(Action action) => _action = action;
        public void Execute()
        {
            _action();
        }
    }
}
