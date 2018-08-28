using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.RefactoreAbstraction
{
    public class TradeParser : ITradeParser
    {
        private readonly ITradeValidator tradeValidator;
        private readonly ITradeMapper tradeMapper;
        public TradeParser(ITradeValidator tradeValidator, ITradeMapper tradeMapper)
        {
            this.tradeValidator = tradeValidator;
            this.tradeMapper = tradeMapper;
        }


        public IEnumerable<TradeRecord> Parse(IEnumerable<string> tradeData)
        {
            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in tradeData)
            {
                var fields = line.Split(new char[] { ',' });

                if (!tradeValidator.Validate(fields, lineCount))
                {
                    continue;
                }

                var tradeRecord = tradeMapper.MapTradeDataToTradeRecord(fields);
                trades.Add(tradeRecord);

                lineCount++;
            }
            return trades;
        }
    }
}
