using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WalletMentor;

namespace WalletMentor
{
    public class MonthlyReport
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("month")]
        public string Month { get; set; }
        [BsonElement("year")]
        public int Year { get; set; }
        [BsonElement("income")]
        public double Income { get; set; }
        [BsonElement("expense")]
        public double Expense { get; set; }
        [BsonElement("budget")]
        public double Budget { get; set; }

        public MonthlyReport(string month, int year, double income, double expense, double budget)
        {
            Month = month;
            Year = year;
            Income = income;
            Expense = expense;
            Budget = budget;
        }


    }

    

}

