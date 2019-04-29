using BonDrucker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BonDrucker
{
    /// <summary>
    /// Interaktionslogik für StatisticViewer.xaml
    /// </summary>
    public partial class StatisticViewer : Window
    {
        public StatisticViewer()
        {
            InitializeComponent();
            addStatisticToForm();
        }


        private void addStatisticToForm()
        {
            List<Statistic> statistic = CSVHandler.readStatistic();
            addLastOrdersToDataGrid(statistic);
            addCountedMealCombosToDataGrid(statistic);
            addCountedMealsToDataGrid(statistic);

        }

        private void addLastOrdersToDataGrid(List<Statistic> statistic)
        {
            dgLastOrders.ItemsSource = statistic.OrderByDescending(x => x.timeStamp);
        }

        private void addCountedMealCombosToDataGrid(List<Statistic> statistic)
        {
            var counted =  statistic.GroupBy(n => n.mealCombination.guid)
                    .Select(c => new { Key = c.Key, counter = c.Count() });            
            foreach (var item in counted)
            {
                dgMenus.Items.Add(new { counter = item.counter, mealCombination = statistic.Where(x => x.mealCombination.guid == item.Key).First() });
            }
        }

        private void addCountedMealsToDataGrid(List<Statistic> statistic)
        {
            var countedMainMeals = statistic.GroupBy(n => n.mealCombination.mainMealGUID)
                    .Select(c => new { Key = c.Key, counter = c.Count() });
            var countedSecondMeals = statistic.GroupBy(n => n.mealCombination.secondMealGUID)
                    .Select(c => new { Key = c.Key, counter = c.Count() });

            countedMainMeals.OrderBy(x => x.counter);
            countedSecondMeals.OrderBy(x => x.counter);
            foreach (var item in countedMainMeals)
            {
                dgMainMeals.Items.Add(new { counter = item.counter, mealCombination = statistic.Where(x => x.mealCombination.mainMealGUID == item.Key).First() });
            }

            foreach (var item in countedSecondMeals)
            {
                dgSecondMeals.Items.Add(new { counter = item.counter, mealCombination = statistic.Where(x => x.mealCombination.secondMealGUID == item.Key).First() });
            }
        }
    }
}
