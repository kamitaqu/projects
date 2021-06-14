using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captcha
{
    class CaptchaGen
    {
        const string Symbols = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890qwertyuiopasdfghjklzxcvbnm";
        private int quantity;
        private string captcha;
        //Set quantity of letters in captcha
        public void SetQuantity(int value)
        {
            quantity = value;
        }
        //Generate captcha with required quantity of lettes
        public void Gen()
        {
            captcha = "";
            Random r = new Random();
            char[] symbols = Symbols.ToCharArray();
            for (int i=0; i<quantity;i++)
            {
                captcha += symbols[r.Next(0, symbols.Length - 1)];
            }
        }
        // Get captcha so we can show it in text or label
        public string GetCaptcha()
        {
            return captcha;
        }
        // Method for match checking our captcha
        public bool CheckCaptcha(string text)
        {
            if (captcha == text)
                return true;
            else
                return false;
        }
    }
}
