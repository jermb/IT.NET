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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum Mode {ADD, SUBTRACT, DIVIDE, MULTIPLY, NONE};
        private static readonly char[] ModeSymbols = { '+', '-', '/', 'x' };
        private Mode mode;
        private string symbol;

        public MainWindow()
        {
            InitializeComponent();
            Display.Text = "0";
        }

        private void Add(object sender, RoutedEventArgs e)
        {

        }

        private void Subtract(object sender, RoutedEventArgs e)
        {

        }
        private void Multiply(object sender, RoutedEventArgs e)
        {

        }
        private void Divide(object sender, RoutedEventArgs e)
        {
            //string txt = "wow";
            //txt.
        }
        private void Equals(object sender, RoutedEventArgs e)
        {

        }

        private void Number(object sender, RoutedEventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            AddNumber(tag);
        }

        private void AddNumber(string num)
        {
            if (Display.Text == "0") { Display.Text = num; }
            else
            {
                Display.Text += num;
            }
        }
        private void Decimal()
        {
            if (Display.Text.IndexOfAny(ModeSymbols) == Display.Text.Length - 1)
            {
                Display.Text += "0.";
            }
            else
            {
                Display.Text += ".";
            }
        }
        private void Symbol(object sender, RoutedEventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();

            switch (tag)
            {
                case "+":  SetMode("+", Mode.ADD); break;
                case "-": SetMode("-", Mode.SUBTRACT); break;
                case "/": SetMode("/", Mode.DIVIDE); break;
                case "x": SetMode("x", Mode.MULTIPLY); break;
                case ".": Decimal();  break;
                case "=": Solve(); break;
            }
        }

        private void SetMode(string symbol, Mode mode)
        {
            this.mode = mode;
            this.symbol = symbol;

            if (Display.Text.IndexOfAny(ModeSymbols) != -1) {
                Solve();
            }

            Display.Text += symbol;
        }

        private void Solve()
        {
            if (mode == Mode.NONE) { return; }
            string[] txtNums = Display.Text.Split(symbol);
            float[] nums = new float[txtNums.Length];
            float value = 0;

            if (txtNums.Length == 1) { return; }
            for (int i = 0; i < txtNums.Length; i++)
            {
                nums[i] = float.Parse(txtNums[i]);
            }

            switch (mode)
            {
                case Mode.ADD: value = nums[0] + nums[1]; break;
                case Mode.SUBTRACT: value = nums[0] - nums[1]; break;
                case Mode.DIVIDE: value = nums[0] / nums[1]; break;
                case Mode.MULTIPLY: value = nums[0] * nums[1];  break;
            }

            Display.Text = value.ToString();
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            mode = Mode.NONE;
            symbol = "";

            Display.Text = "0";
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            char last = Display.Text[Display.Text.Length - 1];
            if (ModeSymbols.Contains(last))
            {
                mode = Mode.NONE;
                symbol = "";
            }
            Display.Text = Display.Text.Remove(Display.Text.Length - 1);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter: Solve(); break;
                case Key.Back: Delete(null, null); break;
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9: AddNumber(e.Key.ToString().Replace("D", "")); break;
            }
        }

        //private void Window_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key.ToString() == "D1")
        //    {
        //        Btn1.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        //    }
        //}
    }
}
