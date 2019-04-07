using BonDrucker.Models;
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

        public static void addToCSV(IMeal meal)
        {
            string type = meal.GetType().ToString();
            writeFileNameToProperty(type);
            var mealList = readMeals(type);
            mealList.Add(meal);
            write(mealList);
        }

        public static void addToCSV(MealCombination mealCombo)
        {
            string type = mealCombo.GetType().ToString();
            writeFileNameToProperty(type);
            var mealList = readCombos(type);
            mealList.Add(mealCombo);
            write(mealList);
        }

        public static void addToCSV(List<MealCombination> mealCombo)
        {
            string type = mealCombo.GetType().ToString();
            writeFileNameToProperty(type);
            var mealList = readCombos(type);
            foreach (MealCombination combo in mealCombo)
            {
                mealList.Add(combo);
            }

            write(mealList);
        }

        public static List<MealCombination> getMealCombinationsFromCSV(MainMeal mainMeal)
        {
            return readCombos("BonDrucker.MealCombination");
        }

        public static void updateCSV(IMeal meal)
        {
            string type = meal.GetType().ToString();
            writeFileNameToProperty(type);
            var mealList = readMeals(type);
            // Suche Index des zu aktualisierenden Element ueber die GUID
            int index = mealList.FindIndex(x => x.guid == meal.guid);
            mealList[index] = meal;
            write(mealList);
        }

        public static void updateCSV(MealCombination combo)
        {
            List<MealCombination> mealCombos = readCombos("BonDrucker.MealCombination");
            // Suche Index des zu aktualisierenden Element ueber die GUID
            int index = mealCombos.FindIndex(x => x.guid == combo.guid);
            mealCombos[index] = combo;
            write(mealCombos);
        }

        public static void deleteRow(IMeal meal)
        {
            string type = meal.GetType().ToString();
            writeFileNameToProperty(type);
            var mealList = readMeals(type);
            // Suche Index des zu aktualisierenden Element ueber die GUID
            int index = mealList.FindIndex(x => x.guid == meal.guid);
            mealList.RemoveAt(index);
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

            if (type.Contains("MealCombination"))
            {
                _fileName = MealCombination.csvFileName;
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
                    meal.ForEach(i => Console.Write("{0}\t", i.insertable));
                    csv.WriteRecords(meal);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex);
            }
        }

        private static void write(List<MealCombination> mealCombo)
        {
            try
            {
                writeFileNameToProperty(mealCombo.First().GetType().ToString());
                using (var writer = new StreamWriter(_filePath + _fileName))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(mealCombo);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex);
            }
        }


        public static List<IMeal> readMeals(string type)
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
                        if (type.Contains("MainMeal"))
                        {
                            record.insertable = csv.GetField<bool>("insertable");
                        }
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

        public static List<MealCombination> readCombos(string type)
        {
            try
            {
                writeFileNameToProperty(type);
                using (var reader = new StreamReader(_filePath + _fileName))
                using (var csv = new CsvReader(reader))
                {
                    var records = new List<MealCombination>();
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = new MealCombination();
                        record.guid = csv.GetField<Guid>("guid");
                        record.mainMealGUID = csv.GetField<Guid>("mainMealGUID");
                        record.secondMealGUID = csv.GetField<Guid>("secondMealGUID");
                        record.mainMealName = csv.GetField<string>("mainMealName");
                        record.secondMealName = csv.GetField<string>("secondMealName");
                        record.totalPrice = csv.GetField<decimal>("totalPrice");
                        records.Add(record);
                    }
                    return records;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex);
                return new List<MealCombination>();
            }
        }
    }
}
