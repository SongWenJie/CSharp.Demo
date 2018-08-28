using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.RefactoreAbstraction
{
    public  interface ITradeMapper
    {
        TradeRecord MapTradeDataToTradeRecord(string[] tradeData);
      
    }
}
