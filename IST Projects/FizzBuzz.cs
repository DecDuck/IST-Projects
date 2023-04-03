namespace IST_Projects;

public class FizzBuzz
{
    public static void Run()
    {
        for (int i = 1; i <= 100; i++)
        {
            bool fizzed = i % 3 == 0;
            if (i % 5 == 0)
            {
                Console.WriteLine(fizzed ? "FizzBuzz!" : "Buzz");
            }
            else if (fizzed)
            {
                Console.WriteLine("Fizz");
            }
            else
            {
                Console.WriteLine(i);
            }
        }
    }
}