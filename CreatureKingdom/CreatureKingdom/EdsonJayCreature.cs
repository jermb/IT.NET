using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CreatureKingdom
{
    internal class EdsonJayCreature : Creature
    {
        public EdsonJayCreature(Canvas kingdom, Dispatcher dispatcher, int waitTime = 100) : base(kingdom, dispatcher, waitTime)
        {
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }
    }
}
