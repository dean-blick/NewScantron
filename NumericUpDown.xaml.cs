using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ScantronRevived
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// placeholder
        /// </summary>
        public uint Count
        {
            get
            {
                return (uint)GetValue(CountProperty);
            }
            set
            {
                SetValue(CountProperty, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CountProperty)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            }
        }

        /// <summary>
        /// placeholder
        /// </summary>
        public static readonly DependencyProperty CountProperty = DependencyProperty.Register(nameof(Count), typeof(uint), typeof(NumericUpDown), new PropertyMetadata(1u));

        public event PropertyChangedEventHandler? PropertyChanged;

        public NumericUpDown()
        {
            InitializeComponent();
        }

        /// <summary>
        /// placeholder
        /// </summary>
        /// <param name="sender">placeholder</param>
        /// <param name="e">placeholder</param>
        private void HandleIncrement(object sender, RoutedEventArgs e)
        {
            if (Count < uint.MaxValue) Count++;
        }

        /// <summary>
        /// placeholder
        /// </summary>
        /// <param name="sender">placeholder</param>
        /// <param name="e">placeholder</param>
        private void HandleDecrement(object sender, RoutedEventArgs e)
        {
            if (Count > 0) Count--;
        }
    }
}

