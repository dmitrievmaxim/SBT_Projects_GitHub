using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSM_email
{
    class HPSMException:Exception
    {
        public HPSMException(string message):base(message)
        {
            
            //Добавить логи
        }
    }
}
