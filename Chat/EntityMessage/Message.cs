using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityExtension
{
    [Serializable]
    public class Message
    {
        public Guid Id { get; set; }
        public string stringMessage { get; set; }
        public object Content { get; set; }
        public override string ToString()
        {
            return stringMessage;
        }

    }
}
