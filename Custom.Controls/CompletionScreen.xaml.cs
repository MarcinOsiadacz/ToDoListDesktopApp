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

namespace Custom.Controls
{
    /// <summary>
    /// Interaction logic for CompletionScreen.xaml
    /// </summary>
    public partial class CompletionScreen : UserControl
    {
        public CompletionScreen()
        {
            InitializeComponent();
        }

        private void MarkCompleteStoryboard_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
