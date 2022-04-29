using BonDrucker.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Configuration;

namespace BonDrucker
{
    static class CSVHandler
    {
        static string _filePath = ConfigurationManager.AppSettings["dataFileLocation"];
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
            // Write the new List sorted by mainMeals
            write(mealList.OrderBy(x => x.mainMealGUID).ToList());
        }

        public static void addToStatistic(List<MealCombination> mealCombo)
        {
            List<Statistic> statisticList = readStatistic();
            foreach (MealCombination combo in mealCombo)
            {
                Statistic stat = new Statistic
                {
                    timeStamp = DateTime.Now,
                    mealCombination = combo
                };
                statisticList.Add(stat);
            }
            // Write the Statistics sorted by TimeStamp
            writeStatistic(statisticList.OrderBy(x => x.timeStamp).ToList());
        }

        public static List<MealCombination> getMealCombinationsFromCSV(MainMeal mainMeal)
        {
            List<MealCombination> combos = readCombos("BonDrucker.MealCombination");
            return combos.FindAll(x => x.mainMealGUID == mainMeal.guid);
        }

        public static bool isSecondMealDeactivated(Guid guid)
        {
            List<IMeal> secondMeals = readMeals("BonDrucker.SecondMeal");
            return ((secondMeals.FindAll(x => x.guid == guid && x.soldOut == true)).Count > 0);
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
            // Suche Index des zu loeschenden Element ueber die GUID
            int index = mealList.FindIndex(x => x.guid == meal.guid);
            mealList.RemoveAt(index);
            if (mealList.Count == 0)
            {
                deleteAll(type);
            }
            else
            {
                write(mealList);
            }
        }

        public static void deleteComboRows(IMeal meal)
        {
            string type = "BonDrucker.MealCombination";
            writeFileNameToProperty(type);
            var mealList = readCombos(type);
            // Suche Index des zu aktualisierenden Element ueber die GUID
            if (meal.GetType().ToString().Contains("MainMeal"))
            {
                mealList.RemoveAll(x => x.mainMealGUID == meal.guid);
            }
            else
            {
                mealList.RemoveAll(x => x.secondMealGUID == meal.guid);
            }
            if (mealList.Count == 0)
            {
                deleteAll(type);
            }
            else
            {
                write(mealList);
            }
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

            if (type.Contains("Statistic"))
            {
                _fileName = Statistic.csvFileName;
                return;
            }

        }

        private static void write(List<IMeal> meal)
        {
            try
            {
                writeFileNameToProperty(meal.First().GetType().ToString());
                using (var writer = new StreamWriter(Path.Combine(_filePath, _fileName)))
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

        private static void deleteAll(string type)
        {
            try
            {
                writeFileNameToProperty(type);
                using (var writer = new StreamWriter(Path.Combine(_filePath, _fileName)))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords((List<IMeal>)Activator.CreateInstance(Type.GetType(type)));
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
                using (var writer = new StreamWriter(Path.Combine(_filePath, _fileName)))
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

        private static void writeStatistic(List<Statistic> statistic)
        {
            try
            {
                writeFileNameToProperty(statistic.First().GetType().ToString());
                using (var writer = new StreamWriter(Path.Combine(_filePath, _fileName)))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(statistic);
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
                using (var reader = new StreamReader(Path.Combine(_filePath, _fileName)))
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
                using (var reader = new StreamReader(Path.Combine(_filePath, _fileName)))
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

        public static List<Statistic> readStatistic()
        {
            try
            {
                writeFileNameToProperty("BonDrucker.Statistic");
                using (var reader = new StreamReader(Path.Combine(_filePath, _fileName)))
                using (var csv = new CsvReader(reader))
                {
                    var records = new List<Statistic>();
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = new Statistic();
                        record.timeStamp = csv.GetField<DateTime>("timeStamp");
                        record.mealCombination.guid = csv.GetField<Guid>("guid");
                        record.mealCombination.mainMealGUID = csv.GetField<Guid>("mainMealGUID");
                        record.mealCombination.secondMealGUID = csv.GetField<Guid>("secondMealGUID");
                        record.mealCombination.mainMealName = csv.GetField<string>("mainMealName");
                        record.mealCombination.secondMealName = csv.GetField<string>("secondMealName");
                        record.mealCombination.totalPrice = csv.GetField<decimal>("totalPrice");
                        records.Add(record);
                    }
                    return records;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex);
                return new List<Statistic>();
            }
        }
    }
}
