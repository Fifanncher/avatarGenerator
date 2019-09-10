using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Drawing;
using System.Windows.Documents;

namespace Avatar
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите ваше имя: ");
            var name = Console.ReadLine();

            var message = Encoding.ASCII.GetBytes(name + Convert.ToString(DateTime.UtcNow)); // уникальная строка
            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            var hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x); // получение уникального хэша пользователя
                Console.Write(x);
            }

            Console.WriteLine();
            Console.WriteLine("Имя - {0}, хэш - {1}", name, hex);
     
            Bitmap bmp = new Bitmap(250, 250);
            Brush Brush = new SolidBrush(ColorTranslator.FromHtml($"#{hex.Substring(0, 6)}")) ; // устанавливаем цвет

            List<bool> FillOrNot = new List<bool>();
            int n = 3;
            bool check = true;

            while (check)
            {
                for (int i = 0; i <= 7; i++)
                {
                    if (FillOrNot.Count < 15)
                    {
                        FillOrNot.Add((hashValue[n] & (1 << i)) != 0);
                    }
                    else
                    {
                        check = false;
                        break;
                    }
                }
                n++;
            }

            int coordX = 0;
            int coordY = 0;
            int rowNumber = 1;

            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.FillRectangle(Brushes.White, new Rectangle(0, 0, 250, 250));
                for (int i = 0; i < 15; i++)
                {
                    if( (i % 5) != 0 || i == 0 )
                    {
                        if (FillOrNot[i])
                        {
                            gr.FillRectangle(Brush, new Rectangle(coordX, coordY + ((i % 5) * 50), 50, 50));
                            gr.FillRectangle(Brush, new Rectangle((250-(50*rowNumber)), coordY + ((i % 5) * 50), 50, 50));
                        }
                    }
                    else
                    {
                        coordX += 50;
                        rowNumber++;
                        coordY = 0;

                        if (FillOrNot[i])
                        {
                            gr.FillRectangle(Brush, new Rectangle(coordX, coordY + ((i % 5) * 50), 50, 50));
                            gr.FillRectangle(Brush, new Rectangle((250 - (50 * rowNumber)), coordY + ((i % 5) * 50), 50, 50));
                        }                       
                    }
                }
            }


            bmp.Save($"D://Avatar{name}.png");
            Console.WriteLine("Ваш аватар сгенерирован и сохранен!");
            Console.ReadKey();
        }
    }
}
