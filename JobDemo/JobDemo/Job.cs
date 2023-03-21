using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobDemo
{
    class Job
    {
        protected double hours;
        protected double price;
        public const double RATE = 45.00;
        public Job(int num, string cust, string desc, double hrs)
        {
            //TODO:
            
        }

        //TODO: auto implement property JobNumber, Customer, Description 
        

        //TODO: implment Hours, Price property. Consider to make Hours virtual for derived class to override

 

        public override string ToString()
        {
            
            return (GetType() + " " + JobNumber + " " + Customer + " " +
               Description + " " + Hours + " hours @" + RATE.ToString("C") +
               " per hour. Total price is " + Price.ToString("C"));
        }

        public override bool Equals(Object e)
        {
            //TODO:


        }

        public override int GetHashCode()
        {
            //TODO:
           
        }

    }

}
