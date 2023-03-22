namespace IST_Projects;

public class Factorial
{
    public static void Run()
    {
        int input = 6;
        long sum = 1;
        for (int i = input; i > 1; i--)
        {
            sum *= i;
        }
        Console.WriteLine(sum);
    }
}