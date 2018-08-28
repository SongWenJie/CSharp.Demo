using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.RefactoreAbstraction
{
    public interface ITradeValidator
    {
        bool Validate(string[] tradeData,int currentLine);
    }
}
