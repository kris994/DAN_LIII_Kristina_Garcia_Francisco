﻿using DAN_LIII_Kristina_Garcia_Francisco.ViewModel;
using System.Windows;

namespace DAN_LIII_Kristina_Garcia_Francisco.View
{
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : Window
    {
        public Employee()
        {
            InitializeComponent();
            this.DataContext = new AllUsersViewModel(this);
        }

        /// <summary>
        /// Closes the Window and opens the Login window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Login login = new Login();
            login.Show();
        }
    }
}
