using System.Collections.Concurrent;
using TaskOtus;

public class Program
{
    static BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();

    static Action<bool, BlockingCollection<ICommand>> _behaviour = (bool stop, BlockingCollection<ICommand> commands) =>
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
    private static async Task Main(string[] args)
    {

        BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>();

        var com1 = new Command(() => Console.WriteLine("Programm 1"));
        var com2 = new Command(() => Console.WriteLine("Programm 2"));
        var com3 = new Command(() => Console.WriteLine("Programm 3"));
        var com4 = new Command(() => Console.WriteLine("Programm 4"));
        var com5 = new Command(() => Console.WriteLine("Programm 5"));

        commands.Add(com1);
        commands.Add(com2);
        commands.Add(com3);
        commands.Add(com4);
        commands.Add(com5);

        MyTaskExecute myTask = new MyTaskExecute(_behaviour, commands);
        await myTask.Start();
        Console.WriteLine(commands.Count);

    }
}