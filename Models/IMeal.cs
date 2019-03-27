using System;

namespace BonDrucker
{
    public interface IMeal
    {
        Guid guid { get; set; }
        string mealName { get; set; }
        decimal price { get; set; }
        bool soldOut { get; set; }
        bool insertable { get; set; }
    }
}
