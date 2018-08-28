using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class TradeProcessor
    {
        public void ProcessTrades(Stream stream)
        {
            var lines = new List<string>();

            using (var reader = new StreamReader(stream))
            {
                string line;
                while((line =reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            var trades = new List<TradeRecord>();
            var lineCount = 1;
            foreach (var line in lines)
            {
                var fields = line.Split(new char[] { ',' });

                if(fields.Length != 3 )
                {
                    Console.WriteLine("WARN: Line {0} malformed. Only {1} fields found",lineCount, fields.Length);
                }

                int tradeAmount;
                if (!int.TryParse(fields[0], out tradeAmount))
                {
                    Console.WriteLine("WARN: Trade amount on line {0} not a valid integer :{1}",lineCount, fields[0]);
                }

                decimal tradePrice;
                if (!decimal.TryParse(fields[1], out tradePrice))
                {
                    Console.WriteLine("WARN: Trade Price on line {0} not a valid decimal :{1}", lineCount, fields[1]);
                }

                var tradeRecord = new TradeRecord
                {
                    TradeAmount = tradeAmount,
                    TradePrice = tradePrice
                };
                trades.Add(tradeRecord);
                lineCount++;
            }
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

            Console.WriteLine("INFO: {0} trades processed",trades.Count);
        }
    }

    public class TradeRecord
    {
        public int TradeAmount { get; set; }

        public decimal TradePrice { get; set; }
    }
}
