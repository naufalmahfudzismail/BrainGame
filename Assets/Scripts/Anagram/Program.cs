using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /*class Shuffle
    {
        static System.Random rnd = new System.Random(); //add random number

        static void Fisher_Yates(int[] array)
        {
            int arraysize = array.Length;
            int random;
            int temp;

            for (int i = 0; i < arraysize; i++)
            {
                random = i + (int)(rnd.NextDouble() * (arraysize - i));

                temp = array[random];
                array[random] = array[i];
                array[i] = temp;
            }
        }

        public static string StringMixer(string s) //shuffling
        {
            string output = "";
            int arraysize = s.Length;
            int[] randomArray = new int[arraysize];

            for (int i = 0; i < arraysize; i++)
            {
                randomArray[i] = i;
            }

            Fisher_Yates(randomArray);

            for (int i = 0; i < arraysize; i++)
            {
                output += s[randomArray[i]];
            }

            return output;
        }
    }

    class addRandom
    {
        public static char GetRandomCharacter(string text, Random rng)
        {
            int index = rng.Next(text.Length);
            return text[index];
        }
    }

    class addDistraction //adding the right amount of distractions char
    {
        public static String add(String text, String distract, int endLength)
        {
            int panjang = text.Length;
            char[] distractingChar = new char[distract.Length];
            string distraction;
            distractingChar = distract.ToCharArray;

            if (panjang < endlength)
            {
                int desired = endlength - panjang;
                char[] numDistract = new char[desired];
                for (int i = 0; i <= desired; i++)
                {
                    numDistract[i] = distractingChar[i];
                }
                distraction = distractingChar.ToString;
            }
            
            return distraction;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string original = "ShiddiqAsySyuhada"; //original string
            string distractions = "!@#$%&?<>{}][|/~"; //distractions

            int desiredLength = 20; //desired length for string + distraction

            distractions = Shuffle.StringMixer(distractions); //adding distraction based on desiredLength

            original = String.Join(original, distractions); //joining original text and distraction

            string mixedOriginal = Shuffle.StringMixer(original); //shuffling text

            System.Console.WriteLine("The original string: {0}", original);
            System.Console.WriteLine("A mix of characters from the original string: {0}", mixedOriginal);

            System.Console.ReadKey();
        }
    }*/
}
