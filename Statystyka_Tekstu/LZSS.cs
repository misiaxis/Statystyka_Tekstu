using System;

namespace Statystyka_Tekstu
{
    static class LZSS
    {
        static char symbol='$';
        static public string CompressLZSS(string input)
        {
            string output = "";
            string word;

            int longestword = 0;

            output += input[0];

            int window = 1;

            for (int i = 1; i < input.Length; i += window)
            {
                window = 0;
                again:
                if (i + window < input.Length)
                {
                    window++;
                    word = input.Substring(i, window);
                    if (input.Substring(0, i).Contains(word))
                    {
                        goto again;
                    }
                    else
                    {
                        if (window > 1) window--;
                    }
                }

                word = input.Substring(i, window);

                string shortcut = ""+symbol;
                shortcut += FindInText(word, input.Substring(0, i));
                shortcut += symbol;
                shortcut += word.Length;
                shortcut += symbol;

                bool Bshortcut = false;

                if (word.Length > shortcut.Length)
                {
                    //place shortcut
                    Bshortcut = true;
                    output += shortcut;
                    if (longestword < word.Length) longestword = word.Length;
                }
                else
                {
                    output += word;
                }
            }
            return output;
        }

        static public string DecompressLZSS(string input)
        {
            string output = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == symbol)
                {
                    int startpoz = i;
                    int first, second;
                    first = 0;
                    second = 0;
                    i++;
                    try
                    {
                        do
                        {
                            first *= 10;
                            first += Int32.Parse(input[i].ToString());
                            i++;
                        } while (input[i] != symbol);

                        i++;
                        do
                        {
                            second *= 10;
                            second += Int32.Parse(input[i].ToString());
                            i++;
                        } while (input[i] != symbol);
                        output += output.Substring(first, second);
                    }
                    catch
                    {
                        i = startpoz;
                        output += input[i];
                    }
                }
                else
                {
                    output += input[i];
                }
            }

            return output;
        }

        static int FindInText(string word, string text)
        {
            return text.IndexOf(word);
        }
    }
}
