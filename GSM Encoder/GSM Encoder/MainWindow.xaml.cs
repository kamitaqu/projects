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

namespace GSM_Encoder
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
        public static string Encode7bit(string s)
        {
            string empty = string.Empty;
            for (int index = s.Length - 1; index >= 0; --index)
                empty += Convert.ToString((byte)s[index], 2).PadLeft(8, '0').Substring(1);
            string str1 = empty.PadLeft((int)Math.Ceiling((Decimal)empty.Length / new Decimal(8)) * 8, '0');
            List<byte> byteList = new List<byte>();
            while (str1 != string.Empty)
            {
                string str2 = str1.Substring(0, str1.Length > 7 ? 8 : str1.Length).PadRight(8, '0');
                str1 = str1.Length > 7 ? str1.Substring(8) : string.Empty;
                byteList.Add(Convert.ToByte(str2, 2));
            }
            byteList.Reverse();
            var messageBytes = byteList.ToArray();
            var encodedData = "";
            foreach (byte b in messageBytes)
            {
                encodedData += Convert.ToString(b, 16).PadLeft(2, '0');
            }
            return encodedData.ToUpper();
        }
        // Basic Character Set
        private const string BASIC_SET =
                "@£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞ\x1bÆæßÉ !\"#¤%&'()*+,-./0123456789:;<=>?" +
                "¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijklmnopqrstuvwxyzäöñüà";

        // Basic Character Set Extension 
        private const string EXTENSION_SET =
                "````````````````````^```````````````````{}`````\\````````````[~]`" +
                "|````````````````````````````````````€``````````````````````````";


        string[] BASIC_SET_ARRAY = BASIC_SET.Select(x => x.ToString()).ToArray();
        string[] EXTENSION_SET_ARRAY = EXTENSION_SET.Select(x => x.ToString()).ToArray();

        enum circle { Start = 1, Complete = 8 }

        string GetChar(string bin)
        {
            try
            {
                if (Convert.ToInt32(bin, 2).Equals(27))
                    return EXTENSION_SET_ARRAY[Convert.ToInt32(bin, 2)];
                else
                    return BASIC_SET_ARRAY[Convert.ToInt32(bin, 2)];
            }
            catch { return string.Empty; }
        }
        public string Decode7bit(string strGsm7bit)
        {

            var suffix = string.Empty;
            var septet = string.Empty;
            var CurSubstr = string.Empty;
            var counter = 1;
            List<string> septets = new List<string>();
            List<string> sectets = new List<string>();

            //Prepare Octets
            var octets = Enumerable.Range(0, strGsm7bit.Length / 2).Select(i =>
            {
                return Convert.ToString(Convert.ToInt64(strGsm7bit.Substring(i * 2, 2), 16), 2).PadLeft(8, '0');

            }).ToList();


            for (var index = 0; index < octets.Count; index = index + 1)
            {
                //Generate Septets
                septet = octets[index].Substring(counter);
                CurSubstr = octets[index].Substring(0, counter);

                if (counter.Equals((int)circle.Start))
                    septets.Add(septet);
                else
                    septets.Add(septet + suffix);

                //Organize Sectets
                sectets.Add(GetChar(septets[index]));

                suffix = CurSubstr;
                counter++;

                //Reset counter when the circle is complete.
                if (counter == (int)circle.Complete)
                {
                    counter = (int)circle.Start;
                    sectets.Add(GetChar(suffix));
                }

            }
            return string.Join("", sectets);
        }

        private void Encode_Click(object sender, RoutedEventArgs e)
        {
            res.Content = Encode7bit(need.Text);
        }

        private void Decode_Click(object sender, RoutedEventArgs e)
        {
            res.Content = Decode7bit(need.Text);
        }
    }
}
