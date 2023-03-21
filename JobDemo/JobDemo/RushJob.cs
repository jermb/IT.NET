using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobDemo
{
    class RushJob : Job
    {
        public const double PREMIUM = 150.00;

        //TODO: Constructor with no parameters: public RushJob() 
        public RushJob() : base(0, null, null, 0)
        {
            
        }



        //TODO: update Hour property with new price calculation: price = hours * RATE + PREMIUM;
        public new double Hours { get => hours; set { hours = value; price = hours * RATE + PREMIUM; } }

        public override string ToString()
        {
            //TODO:
            return $"{GetType()} {JobNumber} {Customer} {Description} {Hours} hours @{RATE.ToString("C")} per hour. Rush job adds {PREMIUM.ToString("C")}. Total Price is {Price.ToString("C")}";
        }
    }
}
