using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface_segregation_principle
{
    interface IMessage
    {
        string ToAddress { get; set; }
        string FromAddress { get; set; }
        void Send();
    }

    interface IVoiceMessage : IMessage
    {
        byte[] Voice { get; set; }
    }

    interface ITextMessage : IMessage
    {
        string Text { get; set; }
    }

    interface IEmailMessage : ITextMessage
    {
        string Subject { get; set; }
    }

    class VoiceMessage : IVoiceMessage
    {
       public string ToAddress { get; set; }
       public string FromAddress { get; set; }
       public byte[] Voice { get; set; }

       public void Send()
       {
           Console.WriteLine("Передача голосовой почты");
       }
    }

    class EmailMessage : IEmailMessage
    {
        public string Text { get; set; }
        public string Subject { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }

        public void Send()
        {
            Console.WriteLine("Отправляем по Email сообщение: {0}", Text);
        }
    }

    class SmsMessage : ITextMessage
    {
        public string Text { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public void Send()
        {
            Console.WriteLine("Отправляем по Sms сообщение", Text);
        }
    }

}
