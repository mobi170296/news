using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApplication.Library.Database
{
    public interface IDBDataType
    {
        object Value();
        string SqlValue();
    }
}
