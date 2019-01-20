using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Platform_Racing_3_Web.Responses
{
    public class SingleRowResponse<T>
    {
        [XmlElement("Row")]
        public T Row;

        public SingleRowResponse()
        {

        }

        public SingleRowResponse(T row)
        {
            this.Row = row;
        }
    }
}
