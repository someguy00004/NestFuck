using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Basees
{
    class Program
    {
        static string fullCode;
        static List<int> cells = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        static List<char> chars = new List<char>();
        static List<int> openMarkers = new List<int>();
        static Dictionary<int, int> closeMarkers = new Dictionary<int, int>();
        static Dictionary<int, int> openMarkersDict = new Dictionary<int, int>();
        static List<int> unclosedMarkers = new List<int>();
        static void Main(string[] args)
        {
            int nestLevel = 0;
            int pointer = 0;
            char input;
            bool running = true;
            int openBrackets = 0;
            int closedBrackets = 0;
            //Console.WriteLine("Enter Code");
            //fullCode = Console.ReadLine();

            StreamReader program = new StreamReader("program.txt");
            fullCode = program.ReadToEnd();

            StreamWriter debug = new StreamWriter("debug.txt");

            int counter2 = 0;
            foreach (char op in fullCode)
            {
                chars.Add(op);
                if (op == '(')
                {
                    nestLevel += 1;
                    openBrackets += 1;
                }
                else if (op == ')')
                {
                    nestLevel -= 1;
                    closedBrackets += 1;
                }

                if (op == '.' && nestLevel == 6)
                {
                    openMarkers.Add(counter2);
                    closeMarkers.Add(counter2, 0);
                    unclosedMarkers.Add(counter2);
                }
                else if (op == '.' && nestLevel == 7)
                {
                    closeMarkers[unclosedMarkers.Last()] = counter2;
                    unclosedMarkers.Remove(unclosedMarkers.Last());
                }
                counter2 += 1;
            }

            if (openBrackets != closedBrackets)
            {
                Console.WriteLine("Unbalanced brackets");
                Exit();
            }

            openMarkersDict = closeMarkers.ToDictionary(x => x.Value, x => x.Key);
            nestLevel = 0;
            int counter = 0;
            while (running == true)
            {

                if(nestLevel < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Nest error at position " + counter);
                    Console.WriteLine("Nest level cannot be less than 0");
                    Exit();
                }
                if (pointer < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Pointer error at position  " + counter);
                    Console.WriteLine("Pointer value cannot be less than 0");
                    Exit();
                }

                //Console.WriteLine();
                //Console.WriteLine();
                //Console.WriteLine(counter + ", " + pointer + ", " + cells[pointer] + ", " + nestLevel);


                debug.WriteLine();
                debug.WriteLine();
                debug.WriteLine(counter + ", " + pointer + ", " + cells[pointer] + ", " + nestLevel);


                foreach (int cell in cells)
                {
                    if (cell == cells[pointer])
                    {
                        //Console.Write("[" + cell + "]");
                        debug.Write("[" + cell + "]");
                    }
                    else
                    {
                        //Console.Write(cell);
                        debug.Write(cell);
                    }
                    //Console.Write( ", ");
                    debug.Write(", ");
                }

                if (counter > chars.Count() - 1)
                {
                    Console.WriteLine("\nProgram completed successfully");
                    Exit();
                }

                char op = chars[counter];

                if (op == '(')
                {
                    nestLevel += 1;
                }
                else if (op == ')')
                {
                    nestLevel -= 1;
                }
                else if (op == '.')
                {
                    if (nestLevel == 0)
                    {
                        pointer += 1;
                        if(pointer > cells.Count())
                        {
                            cells.Add(0);
                        }
                    }
                    else if (nestLevel == 1)
                    {
                        pointer -= 1;
                    }
                    else if (nestLevel == 2)
                    {
                        try
                        {
                            cells[pointer] += 1;
                        }
                        catch
                        {
                            cells.Add(0);
                            cells[pointer] += 1;
                        }
                    }
                    else if (nestLevel == 3)
                    {
                        try
                        {
                            cells[pointer] -= 1;
                        }
                        catch
                        {
                            cells.Add(0);
                            cells[pointer] -= 1;
                        }
                    }
                    else if (nestLevel == 4)
                    {
                        try
                        {
                            Console.Write(Convert.ToChar(cells[pointer]));
                            debug.Write(Convert.ToChar(cells[pointer]));
                        }
                        catch
                        {
                            Console.Write(Convert.ToChar(0));
                        }
                    }
                    else if (nestLevel == 5)
                    {
                        bool validate = true;
                        while (validate)
                        {
                            Console.WriteLine("\nInput character into cell " + pointer.ToString());
                            try
                            { 
                              input = Convert.ToChar(Console.ReadLine());
                              cells[pointer] = (int)input;
                              validate = false;
                            }
                            catch
                            {
                                Console.WriteLine("Only enter 1 charcter at a time");
                            }
                        }
                    }
                    
                    else if (nestLevel == 6)
                    {
                        if (cells[pointer] == 0)
                        {
                            counter = closeMarkers[counter];
                            nestLevel = 7;
                        }
                    }

                    else if (nestLevel == 7)
                    {
                        if (cells[pointer] != 0)
                        {
                            counter = openMarkersDict[counter];
                            nestLevel = 6;
                        }
                    }
                    
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Error: Nest error at position " + counter);
                        Console.WriteLine("Nest level cannot exceed 7");
                        Exit();
                    }
                }
                counter += 1;
            }
            Console.Read();
        }
        static void Exit()
        {
            Console.WriteLine("Press enter to exit...");
            Console.Read();
            Environment.Exit(0);
        }
    }
}
