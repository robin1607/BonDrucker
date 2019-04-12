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
    /// Interaktionslogik für MainMealEditer.xaml
    /// </summary>
    public partial class mainMealEditer : Window
    {
        private List<IMeal> mainMeals;
        private List<IMeal> secondMeals;
        private bool isOnInitProcess = true;
        public mainMealEditer()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainMeals = getMainMealsFromCSV();
            mainMeals.ForEach(i => Console.Write("{0}\t", i.insertable));
            secondMeals = getSecondMealsFromCSV();
            addMainMealsToDataGrid(mainMeals);
            addSecondMealsToDataGrid(secondMeals);
            isOnInitProcess = false;
        }

        private void btnSafeNewMainMeal_Click(object sender, RoutedEventArgs e)
        {
            MainMeal meal = getMainMealFromTextBox();
            if (meal != null)
            {
                clearAllMainMealTextBoxes();
                addMainMealToDataGrid(meal);
                CSVHandler.addToCSV(meal);
                generateMealCombos(meal);
            } 
        }

        // TODO: Die bestehenden MealCombos werden so immer überschrieben -> Preise weg -.-
        private void generateMealCombos(MainMeal mainMeal)
        {
            List<IMeal> secondMeals = getSecondMealsFromCSV();
            List<MealCombination> mealCombos = new List<MealCombination>();
            MealCombination mealCombo;
            foreach (SecondMeal secondMeal in secondMeals)
            {
                mealCombo = MealCombination.getMealCombination(mainMeal, secondMeal);
                mealCombos.Add(mealCombo);
            }
            CSVHandler.addToCSV(mealCombos);
        }

        private void btnDeleteMarkedMainMeal_Click(object sender, RoutedEventArgs e)
        {
            deleteMainMeal(mainMealList);
        }

        private void mealListe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                deleteMainMeal(mainMealList);
            }
        }


        private MainMeal getMainMealFromTextBox()
        {
            try
            {
                MainMeal meal = new MainMeal();
                meal.mealName = txtBoxMainMeal.Text;
                meal.price = Convert.ToDecimal(txtBoxMainMealSinglePrice.Text);
                meal.insertable = checkBoxInertable.IsChecked.HasValue ? checkBoxInertable.IsChecked.Value : false;
                return meal;
            }
            catch (Exception ex)
            {

                ExceptionHandler.Log(ex);
                return null;
            }
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

        private void deleteMainMeal(DataGrid mealList)
        {
            if (mealList.SelectedItem != null && mealList.SelectedIndex >= 0)
            {
                MainMeal meal = mealList.SelectedItem as MainMeal;
                mainMeals.RemoveAt(mainMeals.FindIndex(x => x.guid == meal.guid));
                CSVHandler.deleteRow(meal);
                CSVHandler.deleteComboRows(meal);
                mainMealList.Items.Refresh();
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
        //                       Shared:
        // ##############################################################

        private void checkBox_soldOut(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    var result = sender as CheckBox;
                    IMeal meal = result.DataContext as IMeal;
                    meal.soldOut = result.IsChecked.HasValue ? result.IsChecked.Value : false;
                    changeMeal(meal);
                }
            }
            catch (Exception)
            {

            }
        }

        private void checkBox_Insertable(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    var result = sender as CheckBox;
                    IMeal meal = result.DataContext as IMeal;
                    meal.insertable = result.IsChecked.HasValue ? result.IsChecked.Value : false;
                    changeMeal(meal);
                }
            }
            catch (Exception)
            {

            }
        }

        private void changeMeal(IMeal meal)
        {
            CSVHandler.updateCSV(meal);
        }

        // ##############################################################
        //                      Second Meal:
        // ##############################################################

        private void btnSafeNewSecondMeal_Click(object sender, RoutedEventArgs e)
        {
            SecondMeal meal = getSecondMealFromTextBox();
            if (meal != null)
            {
                clearAllSecondMealTextBoxes();
                addSecondMealToDataGrid(meal);
                generateMealCombos(meal);
                CSVHandler.addToCSV(meal);
            }
        }

        private void btnDeleteMarkedSecondMeal_Click(object sender, RoutedEventArgs e)
        {
            deleteSecondMeal(secondMealList);
        }

        private void deleteSecondMeal(DataGrid mealList)
        {
            if (mealList.SelectedItem != null && mealList.SelectedIndex >= 0)
            {
                SecondMeal meal = mealList.SelectedItem as SecondMeal;
                secondMeals.RemoveAt(secondMeals.FindIndex(x => x.guid == meal.guid));
                CSVHandler.deleteRow(meal);
                CSVHandler.deleteComboRows(meal);
                secondMealList.Items.Refresh();
            }
        }

        private List<IMeal> getSecondMealsFromCSV()
        {
            return CSVHandler.readMeals("BonDrucker.SecondMeal");
        }

        private void generateMealCombos(SecondMeal secondMeal)
        {
            List<IMeal> mainMeals = getMainMealsFromCSV();
            List<MealCombination> mealCombos = new List<MealCombination>();
            MealCombination mealCombo;
            foreach (MainMeal mainMeal in mainMeals)
            {
                mealCombo = MealCombination.getMealCombination(mainMeal, secondMeal);
                mealCombos.Add(mealCombo);
            }
            CSVHandler.addToCSV(mealCombos);
        }

        private void addSecondMealToDataGrid(IMeal meal)
        {
            secondMeals.Add(meal);
            secondMealList.Items.Refresh();
        }

        private void addSecondMealsToDataGrid(List<IMeal> meal)
        {
            secondMealList.ItemsSource = meal;
        }

        private SecondMeal getSecondMealFromTextBox()
        {
            try
            {
                SecondMeal meal = new SecondMeal();
                meal.mealName = txtBoxSecondMeal.Text;
                meal.price = Convert.ToDecimal(txtBoxSecondMealSinglePrice.Text);
                return meal;
            }
            catch (Exception ex)
            {

                ExceptionHandler.Log(ex);
                return null;
            }

        }
   
        private void clearAllSecondMealTextBoxes()
        {
            txtBoxSecondMeal.Text = "";
            txtBoxSecondMealSinglePrice.Text = "";
        }
    }
}
