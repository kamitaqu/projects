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

namespace Captcha
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        CaptchaGen captcha = new CaptchaGen();
        private void gen_Click(object sender, RoutedEventArgs e) //Generating our captcha and show it in label
        {
            try //Need this so this programm wont stop if we didnt input parameters
            {
                captcha.SetQuantity(Convert.ToInt32(qt.Text));
                captcha.Gen();
                res.Content = captcha.GetCaptcha();
            }
            catch (FormatException)
            {
                MessageBox.Show("Type quantity in numbers");
            }
        }
        private void check_Click(object sender, RoutedEventArgs e) // Basically checking is our captcha right or wr0ng and show messages
        {
            try //Need this so this programm wont stop if we didnt input parameters
            {
                string check = check1.Text;
                if (captcha.CheckCaptcha(check) == true)
                    MessageBox.Show("Captcha is right");
                else if (captcha.CheckCaptcha(check) == false)
                    MessageBox.Show("Captcha is wrong");
            }
            catch
            {
                MessageBox.Show("Write answer");
            }
        }
    }
}
