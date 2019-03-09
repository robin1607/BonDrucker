using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BonDrucker
{
    static class XMLHandler
    {
        // TODO: Get this from settings
        static string _filePath = @"C:\Users\Robin Klos\Documents\Visual Studio 2015\Projects\BonDrucker\BonDrucker\";
        static string _fileName = "essensListe.xml";

        public static void saveOrUpdateMealList(Meal meal)
        {
            if (checkIfFileExists(_filePath))
            {
                update(_fileName, _filePath);
            } else
            {
                save<Meal>(meal, _fileName, _filePath);
            }
        }


        private static void save<T>(T objectToSave,string fileName, string filePath)
        {
            if (objectToSave == null) { return; }

            try
            {
                XmlSerializer serializer = new XmlSerializer(objectToSave.GetType());
                // TODO: Check if fileName ends with xml, otherwise add it here!
                using (FileStream stream = new FileStream(filePath + fileName, FileMode.Create))
                {
                    serializer.Serialize(stream, objectToSave);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ExceptionHandler.Log(ex);
            }


        }

        private static void update(string fileName, string filePath)
        {



        }

        private static T read<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex);
            }

            return objectOut;
        }

        private static bool checkIfFileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
