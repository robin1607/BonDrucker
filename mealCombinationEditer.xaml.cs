using BonDrucker.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BonDrucker
{
    /// <summary>
    /// Interaktionslogik für mealCombinationEditer.xaml
    /// </summary>
    public partial class mealCombinationEditer : Window
    {
        private MainMeal meal;
        private List<MealCombination> mealCombos = new List<MealCombination>();

        public mealCombinationEditer()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public mealCombinationEditer(MainMeal meal)
        {
            InitializeComponent();
            addMealCombinationsToDataGrid(meal);
            this.meal = meal;
        }

        private void addMealCombinationsToDataGrid(MainMeal mainMeal)
        {
            mealCombos = CSVHandler.getMealCombinationsFromCSV(mainMeal);
            dataGrid.ItemsSource = mealCombos;
        }

        static private List<IMeal> getSecondMealsFromCSV()  
        {
            return CSVHandler.readMeals("BonDrucker.SecondMeal");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (MealCombination combo in mealCombos)
            {
                CSVHandler.updateCSV(combo);
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
