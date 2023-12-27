using System;
using System.Collections.Concurrent;
using TaskOtus;
using TaskOtus.Command;
using TaskOtus.Exceprion;
using TaskOtus.State;

public class Program
{
    static BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();
    static Dictionary<string, IState> states = new Dictionary<string, IState>();

    static Action<bool, BlockingCollection<ICommand>, IState> _behaviour = (bool stop, BlockingCollection<ICommand> commands, IState state) =>
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
    private static async Task Main(string[] args)
    {
        var mState = new MoveToState(states);
        var uState = new UsusalState(states);

        states.Add("m", mState);
        states.Add("u", uState);

        BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();

        var com1 = new RunCommand(() => Console.WriteLine("Programm 1"), uState);
        var com2 = new Command(() => Console.WriteLine("Programm 2"), new DefaultState());
        var com3 = new MoveToCommand(() => Console.WriteLine("Programm 3"), mState);
        var com4 = new Command(() => throw new SoftStopException(), new DefaultState());
        var com5 = new RunCommand(() => Console.WriteLine("Programm 4"), uState);

        commands.Add(com1);
        commands.Add(com2);
        commands.Add(com3);
        commands.Add(com4);
        commands.Add(com5);

        MyTaskExecute myTask = new MyTaskExecute(_behaviour, commands, mState);
        await myTask.Start();
        Console.WriteLine(commands.Count);

    }
}