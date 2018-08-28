using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class TradeProcessor1
    {
        public void ProcessTrade(Stream stream)
        {
            var lines = ReadTradeData(stream);
            var trades = ParseTrades(lines);
            StoreTrades(trades);
        }

        /// <summary>
        /// 从流中读取交易数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private IEnumerable<string> ReadTradeData(Stream stream)
        {
            var tradeData = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    tradeData.Add(line);
                }
            }
            return tradeData;
        }

        /// <summary>
        /// 将字符串数据装换位TradeRecord实例
        /// </summary>
        /// <param name="tradeData"></param>
        /// <returns></returns>
        private IEnumerable<TradeRecord> ParseTrades(IEnumerable<string> tradeData)
        {
            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in tradeData)
            {
                var fields = line.Split(new char[] { ',' });

                if(!ValidateTradeData(fields,lineCount))
                {
                    continue;
                }

                var tradeRecord = MapTradeDataToTradeRecord(fields);
                trades.Add(tradeRecord);

                lineCount++;
            }
            return trades;
        }

        /// <summary>
        /// 验证交易数据
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        private bool ValidateTradeData(string[] fields,int currentLine)
        {
            if (fields.Length != 3)
            {
                LogMessage("WARN: Line {0} malformed. Only {1} fields found", currentLine, fields.Length);
                return false;
            }

            int tradeAmount;
            if (!int.TryParse(fields[0], out tradeAmount))
            {
                LogMessage("WARN: Trade amount on line {0} not a valid integer :{1}", currentLine, fields[0]);
                return false;
            }

            decimal tradePrice;
            if (!decimal.TryParse(fields[1], out tradePrice))
            {
                LogMessage("WARN: Trade Price on line {0} not a valid decimal :{1}", currentLine, fields[1]);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 组装TradeRecord实例
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private TradeRecord MapTradeDataToTradeRecord(string[] fields)
        {
            int tradeAmount = int.Parse(fields[0]);
            decimal tradePrice = decimal.Parse(fields[1]);
            var tradeRecord = new TradeRecord
            {
                TradeAmount = tradeAmount,
                TradePrice = tradePrice
            };
            return tradeRecord;
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        private void LogMessage(string message,params object[] args)
        {
            Console.WriteLine(message,args);
        }

        /// <summary>
        /// 交易数据持久化
        /// </summary>
        /// <param name="trades"></param>
        private void StoreTrades(IEnumerable<TradeRecord> trades)
        {
            using (var connection = new SqlConnection("DataSource=(local);Initial Catalog=TradeDataBase;Integrated Security = True;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in trades)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "insert_trade";

                        command.Parameters.AddWithValue("@tradeamount", trade.TradeAmount);
                        command.Parameters.AddWithValue("@tradeprice", trade.TradePrice);
                    }
                    transaction.Commit();
                }
                connection.Close();
            }

            Console.WriteLine("INFO: {0} trades processed", trades.Count());
        }
    }
}
