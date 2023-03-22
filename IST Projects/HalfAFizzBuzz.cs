namespace IST_Projects;

public class HalfAFizzBuzz
{
    public static void Run()
    {
        for (int i = 1; i <= 100; i++)
        {
            if(i % 5 == 0 && i % 3 == 0) Console.WriteLine(i);
        }
    }
}