using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOtus.State
{
    public class DefaultState : IState
    {
        public IState Handle()
        {
            return null;
        }

        public void WritelineState()
        {
            
        }
    }
}
