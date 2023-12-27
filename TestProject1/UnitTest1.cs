using System.Collections.Concurrent;
using TaskOtus;
using TaskOtus.Command;
using TaskOtus.Exceprion;
using TaskOtus.State;

namespace TestProject1
{
    public class UnitTest1
    {


        Action<bool, BlockingCollection<ICommand>, IState> _behaviour = (bool stop, BlockingCollection<ICommand> commands, IState state) =>
        {
            while (!stop)
            {
                var c = commands.Take();
                try
                {
                    c.Execute();
                    Assert.Equal(c.State, state);
                }
                catch (SoftStopException e)
                {
                    // SS работает итак, если закончатся задачи. ДЗ не описано хорошо,
                    // суть SS в том, чтобы закончить все задачи на текущий момент,
                    // которые находятся в очереди.

                    commands.Add(new Command(() => throw new HardStopException(), null));

                }
                catch (HardStopException e)
                {
                    stop = true;
                }
                catch (Exception e)
                {
                    // Exception Handler
                }
            }
        };

        [Fact]
        public async Task Test1()
        {
            Dictionary<string, IState> states = new Dictionary<string, IState>();
            BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();

            var com1 = new Command(() => Console.WriteLine("Programm 1"));
            var com2 = new Command(() => Console.WriteLine("Programm 2"));
            var com3 = new Command(() => throw new HardStopException());
            var com4 = new Command(() => Console.WriteLine("Programm 4"));
            var com5 = new Command(() => Console.WriteLine("Programm 5"));

            commands.Add(com1);
            commands.Add(com2);
            commands.Add(com3);
            commands.Add(com4);
            commands.Add(com5);

            MyTaskExecute myTask = new MyTaskExecute(_behaviour, commands, new DefaultState());

            var task = myTask.Start();

            Assert.Equal(TaskStatus.WaitingForActivation, task.Status);

            await task;
            myTask.Stop();
        }

        [Fact]
        public async Task Test2()
        {
            Dictionary<string, IState> states = new Dictionary<string, IState>();
            var mState = new MoveToState(states);
            var uState = new UsusalState(states);

            states.Add("m", mState);
            states.Add("u", uState);

            BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();

            var com1 = new RunCommand(() => Console.WriteLine("Programm 1"), uState);
            var com2 = new MoveToCommand(() => Console.WriteLine("Programm 2"), mState);
            var com3 = new Command(() => Console.WriteLine("Programm 3"));
            var com4 = new Command(() => throw new SoftStopException());
            var com5 = new Command(() => Console.WriteLine("Programm 4"));

            commands.Add(com1);
            commands.Add(com2);
            commands.Add(com3);
            commands.Add(com4);
            commands.Add(com5);

            MyTaskExecute myTask = new MyTaskExecute(_behaviour, commands, mState);
                        
            var task = myTask.Start();



            await task;
            myTask.Stop();
        }

        [Fact]
        public async Task Test3()
        {
            Dictionary<string, IState> states = new Dictionary<string, IState>();
            var mState = new MoveToState(states);
            var uState = new UsusalState(states);

            states.Add("m", mState);
            states.Add("u", uState);

            BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();

            var com1 = new RunCommand(() => Console.WriteLine("Programm 1"), uState);
            var com2 = new Command(() => Console.WriteLine("Programm 2"));
            var com3 = new Command(() => Console.WriteLine("Programm 3"));
            var com4 = new Command(() => throw new SoftStopException());
            var com5 = new Command(() => Console.WriteLine("Programm 4"));

            commands.Add(com1);
            commands.Add(com2);
            commands.Add(com3);
            commands.Add(com4);
            commands.Add(com5);

            MyTaskExecute myTask = new MyTaskExecute(_behaviour, commands, uState);

            var task = myTask.Start();



            await task;
            myTask.Stop();
        }
    }
}