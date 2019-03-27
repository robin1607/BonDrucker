//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Xml;
//using System.Xml.Serialization;

//namespace BonDrucker
//{
//    static class XMLHandler
//    {
//        // TODO: Get this from settings
//        static string _filePath = @"C:\Users\Robin Klos\Documents\Visual Studio 2015\Projects\BonDrucker\";

//        public static void saveOrUpdateMealList(IMeal meal)
//        {
//            if (checkIfFileExists(_filePath + meal.csvFileName))
//            {
//                // update(meal, _filePath);
//            } else
//            {
//                save(meal);
//            }
//        }

//        public static List<MainMeal> getMealsFromXml<T>(IMeal meal)
//        {
//            read<T>(meal.csvFileName);
//            return new List<MainMeal>();
//        }


//        private static void save(IMeal objectToSave)
//        {
//            if (objectToSave == null) { return; }

//            try
//            {
//                XmlSerializer serializer = new XmlSerializer(objectToSave.GetType());
//                // TODO: Check if fileName ends with xml, otherwise add it here!
//                using (FileStream stream = new FileStream(_filePath + objectToSave.csvFileName, FileMode.CreateNew))
//                {
//                    serializer.Serialize(stream, objectToSave);
//                    stream.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.Log(ex);
//            }


//        }

//        private static void update<T>(T objectToSave, string fileName, string filePath)
//        {



//        }

//        private static T read<T>(string fileName)
//        {
//            if (string.IsNullOrEmpty(fileName)) { return default(T); }

//            T objectOut = default(T);

//            try
//            {
//                string attributeXml = string.Empty;

//                XmlDocument xmlDocument = new XmlDocument();
//                xmlDocument.Load(_filePath + fileName);
//                string xmlString = xmlDocument.OuterXml;
//                using (StringReader read = new StringReader(xmlString))
//                {
//                    Type outType = typeof(T);

//                    XmlSerializer serializer = new XmlSerializer(outType);
//                    using (XmlReader reader = new XmlTextReader(read))
//                    {
//                        objectOut = (T)serializer.Deserialize(reader);
//                        reader.Close();
//                    }

//                    read.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                ExceptionHandler.Log(ex);
//            }

//            return objectOut;
//        }

//        private static bool checkIfFileExists(string filePath)
//        {
//            return File.Exists(filePath);
//        }
//    }
//}
