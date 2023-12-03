using System.Collections.Concurrent;
using TaskOtus;

namespace TestProject1
{
    public class UnitTest1
    {
        Action<bool, BlockingCollection<ICommand>> _behaviour = (bool stop, BlockingCollection<ICommand> commands) =>
        {
            while (!stop)
            {
                var c = commands.Take();
                try
                {
                    c.Execute();
                }
                catch (SoftStopException e)
                {
                    // SS работает итак, если закончатся задачи. ДЗ не описано хорошо,
                    // суть SS в том, чтобы закончить все задачи на текущий момент,
                    // которые находятся в очереди.

                    commands.Add(new Command(() => throw new HardStopException()));

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
        public async Task Test2()
        {
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

            MyTaskExecute myTask = new MyTaskExecute(_behaviour, commands);
            await myTask.Start();
            myTask.Stop();
            Console.WriteLine(commands.Count);

            Assert.Equal(2, commands.Count);
        }

        [Fact]
        public async Task Test3()
        {
            BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();

            var com1 = new Command(() => { Task.Delay(2000); Console.WriteLine("Programm 1"); });
            var com2 = new Command(() => { Task.Delay(2000); Console.WriteLine("Programm 2"); });
            var com3 = new Command(() => throw new SoftStopException());
            var com4 = new Command(() => { Task.Delay(2000); Console.WriteLine("Programm 4"); });
            var com5 = new Command(() => { Task.Delay(2000); Console.WriteLine("Programm 5"); });


            commands.Add(com1);
            commands.Add(com2);
            commands.Add(com3);
            commands.Add(com4);
            commands.Add(com5);

            MyTaskExecute myTask = new MyTaskExecute(_behaviour, commands);
            await myTask.Start();
            var com6 = new Command(() => { Task.Delay(1000); Console.WriteLine("Programm 6"); });
            var com7 = new Command(() => { Task.Delay(1000); Console.WriteLine("Programm 7"); });
            commands.Add(com6);
            commands.Add(com7);
            myTask.Stop();
            Console.WriteLine(commands.Count);

            Assert.Equal(2, commands.Count);
        }
    }
}