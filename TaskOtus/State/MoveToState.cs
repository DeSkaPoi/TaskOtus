using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOtus.State
{
    public class MoveToState : IState
    {
        private readonly Dictionary<string, IState> _state = new Dictionary<string, IState>();

        public MoveToState(Dictionary<string, IState> state)
        {
            _state = state;
        }

        public IState Handle()
        {
            _state.TryGetValue("u", out var state);
            return state;
        }

        public void WritelineState()
        {
            Console.WriteLine("MoveTo");
        }
    }
}
