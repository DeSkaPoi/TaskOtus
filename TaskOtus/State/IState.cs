using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOtus.State
{
    public interface IState
    {
        IState Handle();
        void WritelineState();
    }
}
