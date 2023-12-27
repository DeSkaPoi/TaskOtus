using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOtus.State
{
    public class UsusalState : IState
    {
        private readonly Dictionary<string, IState> _state = new Dictionary<string, IState>();

        public UsusalState(Dictionary<string, IState> state)
        {
            _state = state;
        }

        public IState Handle()
        {
            _state.TryGetValue("m", out var state);
            return state;
        }

        public void WritelineState()
        {
            Console.WriteLine("Usual");
        }
    }
}
