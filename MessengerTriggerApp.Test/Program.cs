using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerTriggerApp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            JazMax.Core.Messenger.Service.MessengerTriggerLogic o = new JazMax.Core.Messenger.Service.MessengerTriggerLogic();
            o.LogicalProcessing();
        }
    }
}
