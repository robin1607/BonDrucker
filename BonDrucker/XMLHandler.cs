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
        static void save<T>(T objectToSave,string fileName)
        {
            if (objectToSave == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(objectToSave.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, objectToSave);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex);
            }


        }

        static void update(string fileName, string filePath)
        {



        }

        static string read(string fileName, string filePath)
        {



        }
    }
}
