using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonDrucker.Models
{
    class MealCombination
    {
        public MealCombination()
        {
            guid = new Guid();
        }
        public Guid guid { get; set; }
        public string mainMealName { get; set; }
        public string secondMealName { get; set; }

        public decimal totalPrice { get; set; }

        public static string csvFileName = "essensCombiListe.csv";
    }
}
