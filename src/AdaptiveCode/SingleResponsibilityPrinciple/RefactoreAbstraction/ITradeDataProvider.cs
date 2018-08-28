using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.RefactoreAbstraction
{
    public interface ITradeDataProvider
    {
        IEnumerable<string> GetTradeData();
    }
}
