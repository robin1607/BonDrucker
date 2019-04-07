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
    /// Interaktionslogik für MainMealEditer.xaml
    /// </summary>
    public partial class mainMealEditer : Window
    {
        private List<IMeal> mainMeals;
        private List<IMeal> secondMeals;
        public mainMealEditer()
        {
            InitializeComponent();
            mainMeals = getMainMealsFromCSV();
            mainMeals.ForEach(i => Console.Write("{0}\t", i.insertable));
            secondMeals = getSecondMealsFromCSV();
            addMainMealsToDataGrid(mainMeals);
            addSecondMealsToDataGrid(secondMeals);
        }

        private void btnSafeNewMainMeal_Click(object sender, RoutedEventArgs e)
        {
            MainMeal meal = getMainMealFromTextBox();
            clearAllMainMealTextBoxes();
            addMainMealToDataGrid(meal);
            CSVHandler.addToCSV(meal);
        }

        private void btnDeleteMarkedMainMeal_Click(object sender, RoutedEventArgs e)
        {
            deleteMealFromDataGrid(mainMealList);
        }

        private void mealListe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                deleteMealFromDataGrid(mainMealList);
            }
        }


        private MainMeal getMainMealFromTextBox()
        {
            MainMeal meal = new MainMeal();
            meal.mealName = txtBoxMainMeal.Text;
            meal.price = Convert.ToDecimal(txtBoxMainMealSinglePrice.Text);
            meal.insertable = checkBoxInertable.IsChecked.HasValue ? checkBoxInertable.IsChecked.Value : false;
            return meal;
        }

        private List<IMeal> getMainMealsFromCSV()
        {
            return CSVHandler.readMeals("BonDrucker.MainMeal");
        }

        private void clearAllMainMealTextBoxes()
        {
            txtBoxMainMeal.Text = "";
            txtBoxMainMealSinglePrice.Text = "";
            checkBoxInertable.IsChecked = false;
        }

        private void addMainMealToDataGrid(IMeal meal)
        {
            mainMeals.Add(meal);
            mainMealList.Items.Refresh();
        }

        private void addMainMealsToDataGrid(List<IMeal> meals)
        {
            mainMealList.ItemsSource = meals;
        }

        private void deleteMealFromDataGrid(DataGrid mealList)
        {
            if (mealList.SelectedItem != null && mealList.SelectedIndex >= 0)
            {
                mealList.Items.RemoveAt(mealList.SelectedIndex);
            }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            MainMeal meal = row.Item as MainMeal;

            if (meal.insertable)
            {
                mealCombinationEditer mCE = new mealCombinationEditer(meal);
                mCE.ShowDialog();
            }
        }

        // ##############################################################
        //                      Second Meal:
        // ##############################################################

        private void btnSafeNewSecondMeal_Click(object sender, RoutedEventArgs e)
        {
            SecondMeal meal = getSecondMealFromTextBox();
            clearAllSecondMealTextBoxes();
            addSecondMealToDataGrid(meal);
            CSVHandler.addToCSV(meal);
        }

        private void btnDeleteMarkedSecondMeal_Click(object sender, RoutedEventArgs e)
        {
            deleteMealFromDataGrid(secondMealList);

        }

        private List<IMeal> getSecondMealsFromCSV()
        {
            return CSVHandler.readMeals("BonDrucker.SecondMeal");
        }

        private void addSecondMealToDataGrid(IMeal meal)
        {
            secondMeals.Add(meal);
            secondMealList.Items.Refresh();
        }

        private void addSecondMealsToDataGrid(List<IMeal> meal)
        {
            secondMealList.Items.Add(meal);
        }

        private SecondMeal getSecondMealFromTextBox()
        {
            SecondMeal meal = new SecondMeal();
            meal.mealName = txtBoxSecondMeal.Text;
            meal.price = Convert.ToDecimal(txtBoxSecondMealSinglePrice.Text);
            return meal;
        }
   
        private void clearAllSecondMealTextBoxes()
        {
            txtBoxSecondMeal.Text = "";
            txtBoxSecondMealSinglePrice.Text = "";
        }
    }
}
