using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.RefactoreAbstraction
{
    public class TradeRecordMapper : ITradeMapper
    {
        public TradeRecord MapTradeDataToTradeRecord(string[] tradeData)
        {
            int tradeAmount = int.Parse(tradeData[0]);
            decimal tradePrice = decimal.Parse(tradeData[1]);
            var tradeRecord = new TradeRecord
            {
                TradeAmount = tradeAmount,
                TradePrice = tradePrice
            };
            return tradeRecord;
        }
    }
}
