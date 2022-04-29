using BonDrucker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;
using System.Data;
using System.Drawing.Imaging;

namespace BonDrucker
{
    static class PrintingHandler
    {
        static private MealCombination _combo;
        static public void Print(List<MealCombination> mealCombos)
        {
            try
            {
                foreach (var combo in mealCombos)
                {
                    var doc = new PrintDocument();
                    _combo = combo;
                    doc.PrintPage += new PrintPageEventHandler(ProvideContent);
                    doc.Print();
                }
            }
            catch(Exception ex)
            {
                ExceptionHandler.Log(ex);
            }
        }

        static private void ProvideContent(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Courier New", 10);

            float fontHeight = font.GetHeight();

            int startX = 0;
            int startY = 0;
            int Offset = 20;


            graphics.DrawString(_combo.mainMealName,
                        new Font("Courier New", 20, FontStyle.Bold),
                        new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 30;

            if (_combo.secondMealName.Contains("ohne"))
            {
                graphics.DrawString("ohne Beilage",
                new Font("Courier New", 16, FontStyle.Bold),
                new SolidBrush(Color.Black), startX, startY + Offset);
            }
            else
            {
                graphics.DrawString("mit " + _combo.secondMealName,
                new Font("Courier New", 16, FontStyle.Bold),
                new SolidBrush(Color.Black), startX, startY + Offset);
               
            }

            Offset = Offset + 35;

            graphics.DrawString("Danke für Ihre Bestellung", new Font("Courier New", 10),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("und guten Appetit!", new Font("Courier New", 10),
                    new SolidBrush(Color.Black), startX, startY + Offset);
            e.PageSettings.PaperSize.Width = 50;


        }

    }
}
