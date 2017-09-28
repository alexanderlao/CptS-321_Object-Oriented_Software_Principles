// Alexander Lao
// 11481444

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace ExpTreeDemo
{
    class Program
    {
        private ExpTree userExpTree;

        public void mainMenu()
        {
            bool running = true;
            string userExp = "A1+B1+C1";
            string userVarName;
            string userVarStringValue;
            double userVarDoubleValue;

            // give the ExpTree a default value
            userExpTree = new ExpTree(userExp);

            while (running)
            {
                // prompt the user
                Console.WriteLine("Menu (current expression = \"" + userExp + "\")");
                Console.WriteLine("1. Enter a new expression");
                Console.WriteLine("2. Set a variable value");
                Console.WriteLine("3. Evaluate tree");
                Console.WriteLine("4. Quit");

                // read in their menu choice
                string userChoice = Console.ReadLine().ToString();

                // call the correct function based on the user's choice
                if (userChoice == "1")
                {
                    // prompt the user for an expression
                    Console.Write("Enter a new expression: ");
                    userExp = Console.ReadLine();

                    userExpTree = new ExpTree(userExp);
                }
                else if (userChoice == "2")
                {
                    // prompt the user for a variable name and value
                    Console.Write("Enter a variable name: ");
                    userVarName = Console.ReadLine();

                    Console.Write("Enter a variable value: ");
                    userVarStringValue = Console.ReadLine();
                    userVarDoubleValue = Convert.ToDouble(userVarStringValue);

                    userExpTree.SetVar(userVarName, userVarDoubleValue);
                }
                else if (userChoice == "3")
                {
                    Console.WriteLine(userExpTree.Eval());
                }
                else if (userChoice == "4")
                {
                    // stop running
                    running = false;
                }
                else continue;
            }

            Console.WriteLine("Done");
        }

        static void Main(string[] args)
        {
            Program newProgram = new Program();
            newProgram.mainMenu();
        }
    }
}