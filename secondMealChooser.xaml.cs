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
    /// Interaktionslogik für secondMealChooser.xaml
    /// </summary>
    public partial class secondMealChooser : Window
    {
        static bool Insertable;

        public secondMealChooser()
        {
            InitializeComponent();
        }

        public secondMealChooser(bool name)
        {
            Insertable = name;
        }
    }
}
