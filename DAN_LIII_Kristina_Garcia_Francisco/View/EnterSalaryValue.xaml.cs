using DAN_LIII_Kristina_Garcia_Francisco.Model;
using DAN_LIII_Kristina_Garcia_Francisco.ViewModel;
using System.Windows;

namespace DAN_LIII_Kristina_Garcia_Francisco.View
{
    /// <summary>
    /// Interaction logic for EnterSalaryValue.xaml
    /// </summary>
    public partial class EnterSalaryValue : Window
    {
        public EnterSalaryValue()
        {
            InitializeComponent();
            this.DataContext = new AllUsersViewModel(this);
        }

        public EnterSalaryValue(vwEmployee employeeEdit)
        {
            InitializeComponent();
            this.DataContext = new AllUsersViewModel(this, employeeEdit);
        }
    }
}
