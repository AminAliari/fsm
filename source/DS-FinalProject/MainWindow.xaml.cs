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
    public partial class MainWindow : Window {

        int n;
        bool hasBuilt = true;
        string drawString;
        const string defaultText = "enter a pattern to check";
        List<string> tempLabels;

        Graph g;
        DataTable dt;

        AdjacentWindow aw;
        ImageWindow iw;

        Stopwatch st;

        GraphGeneration gg;
        GetStartProcessQuery gspq;
        GetProcessStartInfoQuery gsiq;
        RegisterLayoutPluginCommand rlpc;

        public MainWindow(int states) {
            InitializeComponent();

            Width = 175 + (states - 2) * 50;
            Height = 250 + (states - 2) * 50;

            n = states;
            dt = new DataTable();

            st = new Stopwatch();

            gspq = new GetStartProcessQuery();
            gsiq = new GetProcessStartInfoQuery();
            rlpc = new RegisterLayoutPluginCommand(gsiq, gspq);

            for (int i = 0; i < states; i++) {
                dt.Columns.Add(i.ToString(), typeof(string));
            }

            for (int row = 0; row < states; row++) {
                DataRow dr = dt.NewRow();
                for (int col = 0; col < states; col++) {
                    dr[col] = "-";
                }
                dt.Rows.Add(dr);
            }
            dataGrid.ItemsSource = dt.DefaultView;


        }

        private void build(object sender, RoutedEventArgs e) {
            if (hasBuilt) {
                MessageBox.Show("make some changes to the graph.");
                return;
            }
            hasBuilt = true;
            g = new Graph(n);
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < n; j++) {
                    tempLabels = new List<string>();
                    foreach (string s in dt.Rows[i][j].ToString().Split(","[0])) {
                        if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s) || s.Equals("-")) continue;
                        tempLabels.Add(s);
                    }
                    if (tempLabels.Count > 0) {
                        g.addEdge(i, j, tempLabels);
                    }
                }
            }

            if (g.hasCycle()) {
                MessageBox.Show("graph has cycls.");
                // show which ones
            } else {
                MessageBox.Show("graph doesn't have cycles.");
            }
        }

        private void drawGraph(object sender, RoutedEventArgs e) {
            if (hasBuilt && g!=null) {
                drawString = g.ToString();
                if (drawString.Length > 0) {
                    gg = new GraphGeneration(gspq, gsiq, rlpc);

                    byte[] output = gg.GenerateGraph(drawString, Enums.GraphReturnType.Jpg);
                    File.WriteAllBytes("output.jpeg", output);
                    iw = new ImageWindow(ByteToImage(output));
                    iw.Show();

                } else {
                    MessageBox.Show("build the graph first.");
                }
            } else {
                MessageBox.Show("build the graph first.");
            }
        }

        public static ImageSource ByteToImage(byte[] imageData) {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            if (e.EditAction == DataGridEditAction.Commit) {
                var column = e.Column as DataGridBoundColumn;
                if (column != null) {
                    int rowIndex = e.Row.GetIndex();
                    var el = e.EditingElement as TextBox;
                    if (el.Text.Equals("-")) return;
                    if (string.IsNullOrWhiteSpace(el.Text)) { el.Text = "-"; hasBuilt = false;  return; }
                    foreach (string s in el.Text.Split(","[0])) {
                        if (!Regex.IsMatch(s, @"[a-zA-Z]") || s.Length != 1) {
                            el.Text = "-";
                            MessageBox.Show("enter valid parameter");
                            return;
                        }
                        el.Text = el.Text.ToLower();
                    }
                    hasBuilt = false;
                }
            }
        }

        private void convert(object sender, RoutedEventArgs e) {
            bool isReady = false;
            if (hasBuilt && g != null) {
                List<string> c = new List<string>();
                List<Edge> edgs;
                StringBuilder sb;
                for (int i = 0; i < g.nV; i++) {
                    edgs = g.Adj(i);
                    if (edgs.Count > 0) {

                        sb = new StringBuilder();
                        foreach (Edge t in edgs) {
                            sb.Append(string.Format("({0},", t.w));
                            foreach (string l in t.labels) {
                                sb.Append(string.Format("{0}, ", l));
                            }
                            sb.Remove(sb.Length - 2, 2);
                            sb.Append(") ");
                        }
                        c.Add(sb.ToString());
                        isReady = true;
                    } else {
                        c.Add("-");
                    }
                }
                if (isReady) {
                    aw = new AdjacentWindow(c);
                    aw.Show();
                } else {
                    MessageBox.Show("empty graph.");
                }
            } else {
                MessageBox.Show("build the graph first.");
            }
        }

        private void input(object sender, RoutedEventArgs e) {
            if (hasBuilt) {
                resultPop.IsOpen = true;
            } else {
                MessageBox.Show("build the graph first.");
            }
        }

        private void result_GotFocus(object sender, RoutedEventArgs e) {
            if (textBox.Text.Equals(defaultText)) {
                textBox.Text = "";
            }
        }

        private void result_LostFocus(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(textBox.Text) || string.IsNullOrWhiteSpace(textBox.Text)) {
                textBox.Text = defaultText;
            }
        }

        private void doneResult(object sender, RoutedEventArgs e) {
            resultPop.IsOpen = false;
            if (string.IsNullOrEmpty(textBox.Text) || string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text.Equals(defaultText)
                || !Regex.IsMatch(textBox.Text, @"^[a-zA-Z]")) {
                MessageBox.Show("enter a valid pattern");
                return;
            } else {
                if (hasBuilt) {
                    st.Restart();
                    int currentState = 0;
                    bool isDone;
                    List<Edge> edgs;

                    foreach (char c in textBox.Text) {
                        isDone = false;
                        edgs = g.Adj(currentState);
                        if (edgs.Count > 0) {
                            foreach (Edge t in edgs) {
                                foreach (string l in t.labels) {
                                    if (c.ToString() == l) {
                                        currentState = t.w;
                                        isDone = true;
                                        break;
                                    }
                                }
                                if (isDone) break;
                                st.Stop();
                                MessageBox.Show(string.Format("invalid pattern.\nelapsed time: {0} ms",st.ElapsedMilliseconds)); return;
                            }
                        } else {
                            MessageBox.Show(string.Format("invalid pattern.\nelapsed time: {0} ms", st.ElapsedMilliseconds));
                            return;
                        }
                    }
                    MessageBox.Show(string.Format("valid pattern.\nelapsed time: {0} ms", st.ElapsedMilliseconds));
                } else {
                    MessageBox.Show("build the graph first.");
                }
            }
        }

        private void removeCycle(object sender, RoutedEventArgs e) {
            //g.removeCycle(true);
            MessageBox.Show("all cycls has been removed");
        }
    }
}
