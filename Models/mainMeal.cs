using System;

namespace BonDrucker
{
    public class MainMeal : IMeal
    {
        public MainMeal()
        {
            guid = Guid.NewGuid();
        }
        public Guid guid { get; set; }
        public string mealName { get; set; }
        public decimal price { get; set; }
        public bool soldOut { get; set; }
        public bool insertable { get; set; }

        static public string csvFileName = "essensListe.csv";
    }
}
