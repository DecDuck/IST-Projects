namespace IST_Projects;

public class EvenSum
{
    public static void Run()
    {
        int sum = 0;
        int i = 0;
        while(i <= 100)
        {
            if (i % 2 == 0) sum += i;
            i++;
        }
        Console.WriteLine(sum);
    }
}