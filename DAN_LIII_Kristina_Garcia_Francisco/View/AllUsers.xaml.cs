﻿using DAN_LIII_Kristina_Garcia_Francisco.ViewModel;
using System.Windows;

namespace DAN_LIII_Kristina_Garcia_Francisco.View
{
    /// <summary>
    /// Interaction logic for AllUsers.xaml
    /// </summary>
    public partial class AllUsers : Window
    {
        public AllUsers()
        {
            InitializeComponent();
            this.DataContext = new AllUsersViewModel(this);
        }
    }
}
