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
    /// Interaktionslogik für mealEditer.xaml
    /// </summary>
    public partial class mealEditer : Window
    {
        public mealEditer()
        {
            InitializeComponent();
        }

        private void btnSafeNewMeal_Click(object sender, RoutedEventArgs e)
        {
            Meal meal = getMealFromTextBox();
            clearAllTextBoxes();
            addMealToDataGrid(meal);
            XMLHandler.saveOrUpdateMealList(meal);
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


        private Meal getMealFromTextBox()
        {
            Meal meal = new Meal();
            meal.mainMeal = txtBoxMainMeal.Text;
            meal.secondMeal = txtBoxSecondMeal.Text;
            return meal;
        }

        private void clearAllTextBoxes()
        {
            txtBoxMainMeal.Text = "";
            txtBoxSecondMeal.Text = "";
        }

        private void addMealToDataGrid(Meal meal)
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
