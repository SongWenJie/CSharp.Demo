using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.RefactoreAbstraction
{
    public class SimpleTradeValidator : ITradeValidator
    {
        private readonly ILogger logger;
        public SimpleTradeValidator(ILogger logger)
        {
            this.logger = logger;
        }


        public bool Validate(string[] tradeData, int currentLine)
        {
            if (tradeData.Length != 3)
            {
                logger.LogWarning("WARN: Line {0} malformed. Only {1} fields found", currentLine, tradeData.Length);
                return false;
            }

            int tradeAmount;
            if (!int.TryParse(tradeData[0], out tradeAmount))
            {
                logger.LogWarning("WARN: Trade amount on line {0} not a valid integer :{1}", currentLine, tradeData[0]);
                return false;
            }

            decimal tradePrice;
            if (!decimal.TryParse(tradeData[1], out tradePrice))
            {
                logger.LogWarning("WARN: Trade Price on line {0} not a valid decimal :{1}", currentLine, tradeData[1]);
                return false;
            }
            return true;
        }
    }
}
