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
    /// Interaktionslogik für Calculator.xaml
    /// </summary>
    public partial class Calculator : Window
    {
        decimal _price;
        public Calculator(decimal price)
        {
            InitializeComponent();
            _price = price;
        }

        private void txtMoneyIn_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal givenMoney = Convert.ToDecimal(txtMoneyIn.Text);
                if (txtMoneyIn.Text != null && givenMoney > 0)
                {
                    decimal returnMoney = calc(_price, givenMoney);
                    txtMoneyOut.Text = returnMoney + " €";
                }
                else
                {
                    txtMoneyOut.Text = "0,00 €";
                }
            }
            catch (Exception)
            {
                txtMoneyOut.Text = "0,00 €";
            }
        }

        private decimal calc(decimal price, decimal givenMoney)
        {
            return givenMoney - price;
        }
    }
}
