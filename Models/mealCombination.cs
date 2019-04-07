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
            guid = Guid.NewGuid();
        }

        public Guid guid { get; set; }
        public Guid mainMealGUID { get; set; }
        public Guid secondMealGUID { get; set; }
        public string mainMealName { get; set; }
        public string secondMealName { get; set; }
        public decimal totalPrice { get; set; }

        public static string csvFileName = "essensCombiListe.csv";

        static public MealCombination getMealCombination(MainMeal mainMeal, SecondMeal secondMeal)
        {
            MealCombination mealCombo = new MealCombination();
            mealCombo.mainMealName = mainMeal.mealName;
            mealCombo.mainMealGUID = mainMeal.guid;
            mealCombo.secondMealName = secondMeal.mealName;
            mealCombo.secondMealGUID = secondMeal.guid;
            mealCombo.totalPrice = mainMeal.price + secondMeal.price;
            return mealCombo;
        }
    }
}
