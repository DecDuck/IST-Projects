namespace IST_Projects;

public class PerfectSquares
{
    public static void Run()
    {
        Console.Write("Perfect squares up to: "); 
        int max = int.Parse(Console.ReadLine() ?? "100");
        for (int product = 0, i = 0; product <= max; product = (i++ + 1) * i)
        {
            Console.WriteLine("{0} squared is {1}", i, product);
        }
    }
}