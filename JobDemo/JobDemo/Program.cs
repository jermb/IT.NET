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

                int num = int.Parse(Input("Enter job number: "));
                int 
            }

            //TODO: Use the jobs array, display all jobs information
        }

        static string Input(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
    }
}
