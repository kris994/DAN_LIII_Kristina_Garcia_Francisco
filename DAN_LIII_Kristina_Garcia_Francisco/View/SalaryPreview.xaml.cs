using DAN_LIII_Kristina_Garcia_Francisco.Model;
using DAN_LIII_Kristina_Garcia_Francisco.ViewModel;
using System.Windows;

namespace DAN_LIII_Kristina_Garcia_Francisco.View
{
    /// <summary>
    /// Interaction logic for SalaryPreview.xaml
    /// </summary>
    public partial class SalaryPreview : Window
    {
        public SalaryPreview(vwEmployee employeeEdit)
        {
            InitializeComponent();
            this.DataContext = new AllUsersViewModel(this, employeeEdit);
        }
    }
}
