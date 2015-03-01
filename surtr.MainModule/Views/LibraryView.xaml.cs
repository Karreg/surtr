﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace surtr.MainModule.Views
{
    using ViewModels;

    /// <summary>
    /// Interaction logic for LibraryView.xaml
    /// </summary>
    public partial class LibraryView : UserControl
    {
        public LibraryView(LibraryViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void libraryGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            this.LibraryGrid.Focus();
        }

        private void synchronizationGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            this.SynchronizationGrid.Focus();
        }
    }
}