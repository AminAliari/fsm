using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Data;
using GraphVizWrapper.Queries;
using GraphVizWrapper;
using GraphVizWrapper.Commands;

namespace DS_FinalProject {
    public partial class AdjacentWindow : Window {

        Graph g;
        DataTable dt;
        GraphGeneration gg;
        GetStartProcessQuery gspq;
        GetProcessStartInfoQuery gsiq;
        RegisterLayoutPluginCommand rlpc;

        public AdjacentWindow(List<string> c) {
            InitializeComponent();

            dt = new DataTable();

            gspq = new GetStartProcessQuery();
            gsiq = new GetProcessStartInfoQuery();
            rlpc = new RegisterLayoutPluginCommand(gsiq, gspq);

            dt.Columns.Add("Adjacent", typeof(string));

            foreach (string adj in c) { 
                DataRow dr = dt.NewRow();
                dr[0] = adj;
                dt.Rows.Add(dr);
            }

            dataGrid.ItemsSource = dt.DefaultView;
        }
    }
}
