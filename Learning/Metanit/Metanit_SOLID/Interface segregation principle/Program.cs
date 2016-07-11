using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_segregation_principle
{
    class Program
    {
        static void Main(string[] args)
        {
            IMessage obj = new VoiceMessage();
            obj.Send();
            obj = new EmailMessage();
            obj.Send();
            Console.ReadKey();
        }
    }
}
