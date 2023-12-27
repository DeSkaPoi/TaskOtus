using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskOtus.Command;
using TaskOtus.State;

namespace TaskOtus
{
    public class MyTaskExecute
    {
        bool is_stop = false;
        Action<bool, BlockingCollection<ICommand>, IState> _behaviour;
        BlockingCollection<ICommand> _commands;

        private IState _state;

        public MyTaskExecute(Action<bool, BlockingCollection<ICommand>, IState> behaviour, BlockingCollection<ICommand> commands, IState state)
        {
            _behaviour = behaviour;
            _commands = commands;
            _state = state;
        }

        public async Task Start()
        {
            await Task.Run(() => _behaviour(is_stop, _commands, _state));
        }

        public void Stop()
        {
            is_stop = true;
        }

        public void setBehaviour()
        {
            is_stop = true;
        }
    }
}
