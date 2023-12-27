using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskOtus.State;

namespace TaskOtus.Command
{
    public interface ICommand
    {
        void Execute();
        IState State { get; }
    }
}
