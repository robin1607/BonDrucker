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
        private MealCombination mealCombo;

        public mealCombinationEditer()
        {
            InitializeComponent();
        }

        public mealCombinationEditer(MainMeal meal)
        {
            InitializeComponent();
            addMealCombinationsToDataGrid(meal);
            this.meal = meal;
        }

        private void addMealCombinationsToDataGrid(MainMeal mainMeal)
        {
            List<IMeal> secondMeals = getSecondMealsFromCSV();
            List<MealCombination> mealCombos = new List<MealCombination>();
            foreach (SecondMeal secondMeal in secondMeals)
            {
                mealCombo = getMealCombination(mainMeal, secondMeal);
                mealCombos.Add(mealCombo);
            }
            dataGrid.ItemsSource = mealCombos;
        }

        private MealCombination getMealCombination(MainMeal mainMeal, SecondMeal secondMeal)
        {
            MealCombination mealCombo = new MealCombination();
            mealCombo.mainMealName = mainMeal.mealName;
            mealCombo.secondMealName = secondMeal.mealName;
            mealCombo.totalPrice = mainMeal.price + secondMeal.price;
            return mealCombo;
        }

        static private List<IMeal> getSecondMealsFromCSV()  
        {
            return CSVHandler.readMeals("BonDrucker.SecondMeal");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(mealCombo);
        }
    }
}
