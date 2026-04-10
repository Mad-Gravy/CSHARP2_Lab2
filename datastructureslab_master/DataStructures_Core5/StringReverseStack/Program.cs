using System;
using System.Collections.Generic;

namespace StringReverseStack
{
    class Program
    {
        static void Main(string[] args)
        {
            // Deliver instructions and store input in a variable
            Console.WriteLine("Olleh! Everybody loves talking backwards! Let's get some practice in...");
            Console.WriteLine("Enter some text, and I'll reverse it for you so you can work on your backwards talking skills: ");
            string input = Console.ReadLine();


            // Turn the input into a stack with a foreach
            Stack<char> chars = new Stack<char>();
            foreach (char c in input)
            {
                chars.Push((char)c);
            }


            // Use the Pop method to store the backwards string in a variable and output to user
            string backwards = "";
            while (chars.Count > 0)
            {
                backwards += chars.Pop();
            }

            Console.WriteLine("In backwards talk, that would be...  " + backwards);
        }

    }
}
