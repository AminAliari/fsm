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
    public partial class ImageWindow : Window {
        public ImageWindow(ImageSource img) {
            InitializeComponent();
            imgBox.Source = img;
            Width += img.Width;
            Height += img.Height;
        }
    }
}
