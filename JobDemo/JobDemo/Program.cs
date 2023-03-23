using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace JobDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Job[] jobs = new Job[3]; //Create an array of three Jobs. Don’t change this statement. 

            //TODO: take user input, construct Job or RushJob object, and save it to the Job array declared above. 

            for (int i = 0; i < 3; i++)
            {
                bool rush = Input("Rush Job (y/n)?").Equals("y");
                int num;
                while (true)
                {
                    try
                    {
                        num = int.Parse(Input("Enter job number: "));
                        for (int k = i; k-- > 0; k--)
                        {
                            if (jobs[k].JobNumber == num)
                            {
                                Console.WriteLine($"Sorry, job number {num} is a duplicate. Please reenter.");
                                continue; 
                            }
                        }
                    }
                    catch (FormatException) 
                    {
                        Console.WriteLine("Please enter a valid number.");
                        continue;
                    }
                    break;
                }
                
                
                string cust = Input("Enter customer's name: ");
                string desc = Input("Enter job description: ");
                double hours;
                while (true)
                {
                    try
                    {
                        hours = double.Parse(Input("Enter estimated hours "));
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid number.");
                    }
                }

                Job job;
                if (!rush) job = new Job(num, cust, desc, hours);
                else
                {
                    job = new RushJob
                    {
                        JobNumber = num,
                        Customer = cust,
                        Description = desc,
                        Hours = hours
                    };
                }
                jobs[i] = job;
                Console.WriteLine();
            }

            //TODO: Use the jobs array, display all jobs information
            double total = 0;
            foreach (Job j in jobs)
            {
                Console.WriteLine(j);
                total += j.Price;
            }

            Console.WriteLine($"Total for all jobs is {total}");

            Input("Press any key to continue...");
        }

        static string Input(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
    }
}
