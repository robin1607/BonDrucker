using System;

namespace BonDrucker
{
    public class SecondMeal : IMeal
    {
        public SecondMeal()
        {
            guid = Guid.NewGuid();
        }

        public Guid guid { get; set; }
        public string mealName { get; set; }
        public decimal price { get; set; }
        public bool soldOut { get; set; }
        // secondMeal is never insertable
        public bool insertable { get; set; }

        public static string csvFileName = "beilagenListe.csv";

    }
}
 