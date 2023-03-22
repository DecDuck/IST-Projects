namespace IST_Projects;

public class Prime_Number_Calculator
{
    public static void Run()
    {
        bool IsPrime(int x)
        {
            // Handle edge cases
            if (x <= 1) return false;
            for (int i = x / 2; i > 1; i--)
            {
                if (x % i == 0) return false;
            }

            return true;
        }

        for (int i = 1; i <= 100; i++)
        {
            Console.WriteLine("{0}: is prime? {1}", i, IsPrime(i));
        }
    }
}