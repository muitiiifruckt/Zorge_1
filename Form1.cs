using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zorge_1
{
    public partial class Form1 : Form
    {
        string S = "";
        string alf = "abcdefghijklmnopqrstuvwxyz";
        string numbers = "0123456789";
        string alf_numbers = "abcdefghijklmnopqrstuvwxyz0123456789"; 
        string text = "";
        string text_1 = "";
        string key = "";
        string anagramma = "";
        char[,] tabl_1 = new char[5, 6];
        char[,] tabl_3 = new char[3, 10];
        public Form1()
        {
            InitializeComponent();
        }
        private void encrypt(string key, string anagramma)
        {
            text_1 = richTextBox1.Text.ToLower();//
            foreach( char p in text_1)//очитска от лишних символов
            {
                if (string_in(p, alf_numbers))
                    text += p;
            }

            // проверки на правильность 
            try
            {
                if (key.Length != 6 && anagramma.Length != 8) throw new Exception("Длина ключа должна быть 6, длина анаграммы должна быть 8 и должна включать самые популярные буквы алфавита \"asintoer\"");
                

                //
                for (int i = 0; i < key.Length; i++)
                    if (!string_in(key[i], alf)) throw new Exception("В ключе присутствует лишние символы ( другой язык, пробелы, знаки препинания, верхний регистр и т.д.)");
                //
                for (int i = 0; i < anagramma.Length; i++)
                    if (!string_in(anagramma[i], alf)) throw new Exception("В анаграмме присутствует лишние символы ( другой язык, пробелы, знаки препинания, верхний регистр и т.д.)");


                //
                for (int i =0;i<key.Length; i++)
                    for(int j = 1;j>i && j <key.Length;j++)
                        if (key[i] == key[j])
                            throw new Exception("В ключе все буквы должны быть разные");


                //
                for (int i = 0; i < anagramma.Length; i++)
                    for (int j = 1; j > i && j < key.Length; j++)
                        if (anagramma[i] == anagramma[j])
                            throw new Exception("В анаграмме все буквы должны быть разные");

                //
            }
            catch (Exception e)
            { MessageBox.Show($"Ошибка: {e.Message}"); }

            //
            create_table();
            //
            bool is_first_number = true;
            bool Isnumber = false;
            //
            foreach (char k in text)
            {

                if (string_in(k, numbers)) // определяем цифра  ли это
                  Isnumber = true; 
                else 
                    Isnumber = false;

                /////
                ///


                if(Isnumber ) // если это число
                {
                    if(is_first_number)
                    {
                        S += "94" + k + k;
                        is_first_number = false;
                    }
                    else
                    {
                        S +=  k ;
                        S += k ;
                    }
                }
                else// если это  НЕ число
                {
                    if (!(S[S.Length-2]=='9' && S[S.Length - 1] == '4') && !is_first_number)
                    {
                        S += "94";
                    }
                    is_first_number = true;
                    int i = 0;
                    int j = 0;

                    for(;i<3;i++)// поиск индекосв в конечной таблице 
                    {
                        j = 0;
                        for (; j < 10; j++)
                        {
                            if (k == tabl_3[i,j])
                                goto found;
                        }
                    }
                found:
                    if (i == 0)
                        S += j;
                    else
                    {
                        S += (i+7) ;
                        S +=  j;
                    }

                }
            }
            if (string_in(text[text.Length-1],numbers)) // если последний символ был цифрой 
                S += "94";

            richTextBox2.Text= S;
        }
        
        private void create_table()
        {
            // заполнение первой таблимцы 
            for (int i = 0; i < 6; i++) // первая строка просто ключевое слово
            {
                tabl_1[0, i] = key[i];
            }
            ////
            ///

            int k = 0;
            for (int i = 1; i < 5; i++) // заполнение 
            {
                for (int j = 0; j < 6 && i * 6 + j < alf.Length; j++)
                {
                    for (; k < alf.Length; k++)
                    {

                        if (!string_in(alf[k], key))
                        { break; }

                    }
                    tabl_1[i, j] = alf[k];
                    k++;
                }
            }

            ////
            ///

            tabl_1[4, 2] = '.';
            tabl_1[4, 3] = '/';
            // заполнение первой таблимцы 

            // заполнение 3 таблимцы 
            tabl_3[2, 0] = '.';
            tabl_3[2, 0] = '/';
            int p = 0; // счетчик для заполнения первой строки 
            int q = 0; //  счетчик для последовательного заполнения таблицы по строкам ( второй и третьей)

            ////
            ///

            for (int i = 0; i < 6; i++) // заполнение конечной таблицы
            {
                for (int j = 0; j < 5; j++)
                {
                    if (string_in(tabl_1[j, i], anagramma)) // заполнение первой строки буквами из анаграммы по очередности появления в в таблице 1 ( от столбцов к строкам) 
                    {
                        tabl_3[0, p] = tabl_1[j, i];
                        p++;
                    }
                    else if (q == 20)
                        break;
                    else if (q < 10) // заполнение второй строки буквами из не анаграммы 
                    {
                        tabl_3[1, q] = tabl_1[j, i];
                        q++;
                    }
                    else if (q >= 10 && !(tabl_3[1, q - 10] == '.' || tabl_3[1, q - 10] == '/')) // заполнение третьей строки буквами из не анаграммы
                    {
                        if (i == 4 && j == 4) // в таблице пусто и нужно просто перейти на след значение
                            continue;
                        else
                        {
                            tabl_3[2, q - 10] = tabl_1[j, i];
                            q++;
                        }
                    }
                    else // перевод хода
                    {
                        q++;
                    }
                }
            }
            // заполнение 3 таблимцы 
        }

        public bool string_in(char a, string s) // функция, которая проверяет наличие того или иного элемента в алфавите 
        {
            for (int i = 0; i < s.Length; i++)
                if (a == s[i])
                    return true;
            return false;
        }


        private void encrypt_btn_Click(object sender, EventArgs e)
        {
            key = richTextBox3.Text;
            anagramma = richTextBox4.Text;
            encrypt(key, anagramma); // запуск
        }
    }
}
