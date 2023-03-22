namespace IST_Projects;

public class Fibonacci
{
    public static void Run()
    {
        Console.Write("Fibonacci sequence up to: ");
        int max = int.Parse(Console.ReadLine() ?? "100");
        List<int> trackers = new() {0, 1};
        Console.Write("0\n1\n");
        while (trackers.Sum() < max)
        {
            int item = trackers.Sum();
            Console.WriteLine(item);
            trackers.RemoveAt(0);
            trackers.Add(item);
        }
    }
}