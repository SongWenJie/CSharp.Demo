using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.RefactoreAbstraction
{
    public class SqlTradeStorage : ITradeStorage
    {
        private readonly string connectString = "DataSource=(local);Initial Catalog=TradeDataBase;Integrated Security = True;";
        private readonly ILogger logger;
        public SqlTradeStorage(ILogger logger)
        {
            this.logger = logger;
        }


        public void Persist(IEnumerable<TradeRecord> trades)
        {
            using (var connection = new SqlConnection(connectString))
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

            logger.LogInfo("INFO: {0} trades processed", trades.Count());
        }
    }
}
