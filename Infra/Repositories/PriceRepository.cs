using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoCheck.Domain;

namespace CryptoCheck.Infra.Repositories
{
    public class PriceRepository(DatabaseConnection connection)
    {
        private string createSQL(CollectResult cr)
        {
            return $"INSERT INTO prices(price, collect_date, currency) VALUES ({cr.Price.Value}, \"{cr.CollectionTime.ToString("yyyy-MM-ddTHH:mm:ssZ")}\", \"{cr.Price.Currency}\")";
        }
        public void Save(CollectResult result)
        {
            connection.PerformQuery(command =>
            {
                command.CommandText = createSQL(result);
                command.ExecuteNonQuery();
                return null;
            });
        }
        public IEnumerable<CollectResult> FindAll()
        {
            return (IEnumerable<CollectResult>) connection.PerformQuery(command =>
            {
                command.CommandText = "SELECT price, collect_date, currency FROM prices;";
                var r = command.ExecuteReader();
                List<CollectResult> result = new List<CollectResult>();
                while (r.Read())
                {
                    decimal price = r.GetDecimal(0);
                    string collectDate = r.GetString(1);
                    string currency = r.GetString(2);

                    Price p = Price.Build(price, currency);
                    DateTime collectDateParsed = DateTime.Parse(collectDate);
                    result.Add(new CollectResult(p, collectDateParsed));
                }
                return result;
            });
        }
    }
}
