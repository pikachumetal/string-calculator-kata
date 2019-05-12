using InitialKata.App.Business;
using System;

namespace InitialKata.App
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("StringCalculator");

            var sc = new StringCalculator();

            //var numbers = @"//[;]\n332;345\n34;6";
            // var numbers = @"-5";
            var numbers = @"//[*][%]\n1*2%3";

            var result = sc.Add(numbers);

            Console.WriteLine($"Numbers: {numbers} | Add | Result: {result}");
            Console.ReadKey();
        }
    }
}
