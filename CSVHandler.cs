using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BonDrucker
{
    static class CSVHandler
    {
        // TODO: Get this from settings
        static string _filePath = @"C:\Users\Robin Klos\Documents\Visual Studio 2015\Projects\BonDrucker\";
        static string _fileName = "";

        public static void addMealToCSV(IMeal meal)
        {
            string type = meal.GetType().ToString();
            writeFileNameToProperty(type);
            var mealList = read(type);
            mealList.Add(meal);
            write(mealList);
        }

        private static void writeFileNameToProperty(string type)
        {
            if (type.Contains("MainMeal"))
            {
                _fileName = MainMeal.csvFileName;
                return;
            }

            if (type.Contains("SecondMeal"))
            {
                _fileName = SecondMeal.csvFileName;
                return;
            }
        }

        private static void write(List<IMeal> meal)
        {
            try
            {
                writeFileNameToProperty(meal.First().GetType().ToString());
                using (var writer = new StreamWriter(_filePath + _fileName))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(meal);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex);
            }
        }

        public static List<IMeal> read(string type)
        {
            try
            {
                writeFileNameToProperty(type);
                using (var reader = new StreamReader(_filePath + _fileName))
                using (var csv = new CsvReader(reader))
                {
                    var records = new List<IMeal>();
                    csv.Read();
                    csv.ReadHeader();
                    while(csv.Read())
                    {
                        var record = (IMeal)Activator.CreateInstance(Type.GetType(type));
                        record.guid = csv.GetField<Guid>("guid");
                        record.mealName = csv.GetField<string>("mealName");
                        record.price = csv.GetField<decimal>("price");
                        record.soldOut = csv.GetField<bool>("soldOut");
                        record.insertable = csv.GetField<bool>("insertable");
                        records.Add(record);
                    }
                    return records;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex);
                return new List<IMeal>();
            }
        }
    }
}
