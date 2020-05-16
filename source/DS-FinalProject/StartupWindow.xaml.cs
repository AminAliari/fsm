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

namespace DS_FinalProject {

    public partial class StartupWindow : Window {
        public StartupWindow() {
            InitializeComponent();
            //MainWindow window = new MainWindow(4);
            //window.Show();
            //this.Close();
        }

        string defaultText = "number of states";

        private void textBox_LostFocus(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(textBox.Text) || string.IsNullOrWhiteSpace(textBox.Text)) {
                textBox.Text = defaultText;
            }
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e) {
            if (textBox.Text.Equals(defaultText)) {
                textBox.Text = "";
            }
        }

        private void BuildBtn_Click(object sender, RoutedEventArgs e) {
            
            try {
                int states = Int32.Parse(textBox.Text);
                if (states > 1) {
                    MainWindow window = new MainWindow(states);
                    window.Show();
                    this.Close();
                } else {
                    MessageBox.Show("minimum state number is 2.");
                }                    
            } catch {
                MessageBox.Show("enter an integer number.");
            }
        }
    }
}
