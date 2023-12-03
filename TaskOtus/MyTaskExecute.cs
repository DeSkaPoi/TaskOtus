using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOtus
{
    public class MyTaskExecute
    {
        bool is_stop = false;
        Action<bool, BlockingCollection<ICommand>> _behaviour;
        BlockingCollection<ICommand> _commands;

        public MyTaskExecute(Action<bool, BlockingCollection<ICommand>> behaviour, BlockingCollection<ICommand> commands)
        {
            _behaviour = behaviour;
            _commands = commands;
        }

        public async Task Start()
        {
            await Task.Run(() => _behaviour(is_stop, _commands));
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
