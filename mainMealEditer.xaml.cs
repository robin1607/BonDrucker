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
        public mainMealEditer()
        {
            InitializeComponent();
        }

        private void btnSafeNewMeal_Click(object sender, RoutedEventArgs e)
        {
            MainMeal meal = getMealFromTextBox();
            clearAllTextBoxes();
            addMealToDataGrid(meal);
            CSVHandler.addMealToCSV(meal);
        }

        private void btnDeleteMarkedMeal_Click(object sender, RoutedEventArgs e)
        {
            deleteMealFromDataGrid();
        }

        private void mealListe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                deleteMealFromDataGrid();
            }
        }


        private MainMeal getMealFromTextBox()
        {
            MainMeal meal = new MainMeal();
            meal.mealName = txtBoxMainMeal.Text;
            meal.price = Convert.ToDecimal(txtBoxPrice.Text);
            meal.insertable = checkBoxInertable.IsChecked.HasValue ? checkBoxInertable.IsChecked.Value : false;
            return meal;
        }

        private void clearAllTextBoxes()
        {
            txtBoxMainMeal.Text = "";
            txtBoxPrice.Text = "";
            checkBoxInertable.IsChecked = false;
        }

        private void addMealToDataGrid(MainMeal meal)
        {
            mealList.Items.Add(meal);
        }

        private void deleteMealFromDataGrid()
        {
            if (mealList.SelectedItem != null && mealList.SelectedIndex >= 0)
            {
                mealList.Items.RemoveAt(mealList.SelectedIndex);
            }
        }
    }
}
