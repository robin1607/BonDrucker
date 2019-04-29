using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonDrucker.Models
{
    public class Statistic
    {
        public Statistic()
        {
            this.mealCombination = new MealCombination(true);
        }
        public DateTime timeStamp { get; set; }
        public MealCombination mealCombination { get; set; }
        public static string csvFileName = "statistik.csv";
    }
}
