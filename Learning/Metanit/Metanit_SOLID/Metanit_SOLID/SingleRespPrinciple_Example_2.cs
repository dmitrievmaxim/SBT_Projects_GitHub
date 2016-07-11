using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metanit_SOLID
{
    class Phone
    {
        public string Model { get; set; }
        public int Price { get; set; }
    }

    class MobileStore
    {
        List<Phone> phones = new List<Phone>();
        public IPhoneReader Reader { get; set; }
        public IPhoneBinder Binder { get; set; }
        public IPhoneValidator Validator { get; set; }
        public IPhoneSaver Saver { get; set; }

        public MobileStore(IPhoneReader reader, IPhoneBinder binder, IPhoneValidator validator, IPhoneSaver saver)
        {
            Reader = reader;
            Binder = binder;
            Validator = validator;
            Saver = saver;
        }

        public void Process()
        {
            string[] data = Reader.getInputData();
            Phone phone = Binder.CreatePhone(data);
            if (Validator.IsValid(phone))
            {
                phones.Add(phone);
                Saver.Save(phone);
                Console.WriteLine("Data saved");
            }
            else
                Console.WriteLine("Uncorrect data");
        }
    }

    interface IPhoneReader
    {
        string[] getInputData();
    }

    class ConsolePhoneReader : IPhoneReader
    {
        public string[] getInputData()
        {
            Console.WriteLine("Input model: ");
            string model = Console.ReadLine();
            Console.WriteLine("Input price: ");
            string price = Console.ReadLine();
            return new string[] { model, price };
        }
    }

    interface IPhoneBinder
    {
        Phone CreatePhone(string[] data);
    }

    class GeneralPhoneBinder : IPhoneBinder
    {
        public Phone CreatePhone(string[] data)
        {
            if (data.Length >= 2)
            {
                int price = 0;
                if (Int32.TryParse(data[1], out price))
                {
                    return new Phone { Model = data[0], Price = price };
                }
                else
                    throw new Exception("Uncorrect data for price property");
            }
            else
                throw new Exception("Insufficiently of data");
        }
    }

    interface IPhoneValidator
    {
        bool IsValid(Phone phone);
    }

    class GeneralPhoneValidator : IPhoneValidator
    {
        public bool IsValid(Phone phone)
        {
            if (String.IsNullOrEmpty(phone.Model) || phone.Price <= 0)
                return false;

            return true;
        }
    }

    interface IPhoneSaver
    {
        void Save(Phone phone);
    }

    class PhoneSaver : IPhoneSaver
    {
        public void Save(Phone phone)
        {
            Console.WriteLine(phone.Model + " saved");
        }
    }
}
