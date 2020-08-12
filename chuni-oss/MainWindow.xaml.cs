using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace chuni_oss
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer Timer;
        List<bool> slider;

        public MainWindow()
        {
            InitializeComponent();
            addGrid();
            this.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(HandleEsc);
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            
            var ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            outerGrid.RowDefinitions[1].Height = new GridLength(ScreenWidth * 0.24);

            Width = ScreenWidth;
            Height = ScreenWidth * 0.24;


            Timer = new Timer();
            Timer.Interval = 1;
            Timer.Tick += Timer_Tick;
            Timer.Start();
            slider = new List<bool>();
            initSliderList(slider);
        }

        private void initSliderList(List<bool> list)
        {
            for(int i = 0; i < 32; i++)
            {
                list.Add(false);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string str = "";
            foreach(bool grid in slider)
            {
                str += grid ? 1 : 0;
            }
            label1.Content = str;
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice != null)
                return; // スタイラス操作／タッチ操作のときはスルー
            this.DragMove();
        }

        public void addGrid()
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var rectangle = new Rectangle();
                    rectangle.Stroke = Brushes.Black;
                    rectangle.Fill = Brushes.SkyBlue;
                    rectangle.TouchEnter += Rectangle_TouchEnter;
                    rectangle.TouchLeave += Rectangle_TouchLeave;
                    Grid.SetColumn(rectangle, i * 2);
                    Grid.SetRow(rectangle, j);
                    grid1.Children.Add(rectangle);
                }
            }
        }

        private void Rectangle_TouchLeave(object sender, TouchEventArgs e)
        {
            var rect = sender as Rectangle;
            rect.Fill = Brushes.SkyBlue;

            var col = Grid.GetColumn(rect) / 2;
            var row = Grid.GetRow(rect);
            if (row == 0) slider[col] = false;
            else slider[16 + col] = false;
        }

        private void Rectangle_TouchEnter(object sender, TouchEventArgs e)
        {
            var rect = sender as Rectangle;
            rect.Fill = Brushes.Orange;

            var col = Grid.GetColumn(rect) / 2;
            var row = Grid.GetRow(rect);
            Debug.WriteLine("Down:" + row.ToString() + "," + col.ToString());

            if (row == 0) slider[col] = true;
            else slider[16 + col] = true;

        }



        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
            if (e.Key == Key.F11)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                }
            }
        }
    }
}
