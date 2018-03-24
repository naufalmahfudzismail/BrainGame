using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Flexible : MonoBehaviour
{

    static System.Random rand = new System.Random();

    public int Jsoal;

    public GameObject CharButton;
    public GameObject PostButton;
    public GameObject canvasPlay;
    public GameObject canvasField;
    public GameObject Kalimat;
    public GameObject InsertButton;
    public GameObject Soaltxt;

    private int tSameChar;
    private int tSameFloor;
    private int tGetChar;
    private int tGetFloor;
    private string FieldSoal;


    //private double CharPercentage = (tGetChar / tSameChar) * 100;
    //private double FloorPercentage = (tGetFloor / tSameChar) * 100;

    private bool CharisMatched = false;
    private bool FloorisMatched = false;
    private bool isDone = false;

    public Text[] floor = new Text[9];
    public Text[] result;
    public Text soal;
    public InputField kalimat;
    public GameObject[] inputKata = new GameObject[3];
    public InputField[] InputKata = new InputField[3];
    public Text charScore;
    public Text postScore;

    private int totalC;

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private List<char> alphabets = new List<char>(); //list alphabet sri string alphabet

    private string[] kamus; // array kamus data


    List<char> randomedChar = new List<char>(); //list char random
    List<char> sameChar = new List<char>(); //list randomed  yang akan di samakan
    List<char> sameClone = new List<char>(); //list samechar yang sudah di gandakan
    List<char> difChar = new List<char>(); //list randomechar yang berbeda
    List<char> wasDoubled = new List<char>(); //list double value char dari finalchar 
    List<char> distinctDouble = new List<char>(); //list wasdouble yang telah di distinct
    List<char> finalChar = new List<char>(); //list dari gabungan anatar random list sameclone dan difchar
    List<Text> tFl = new List<Text>(); //list gameobject text


    // Use this for initialization
    IEnumerator Start()
    {

        WWW items = new WWW("http://localhost/BrainGameDB/items.php");

        yield return items;

        string itemsString = items.text;

        kamus = itemsString.Split('|');

        for (int i = 0; i < kamus.Length; i++)
        {
            kamus[i] = kamus[i].ToUpper();
        }

        GatherChar();
        RandomingPosition();
        StartCoroutine("SetTask");
        CheckDouble();


    }

    // Update is called once per frame
    void Update()
    {

        charScore.text = tGetChar.ToString();
        postScore.text = tGetFloor.ToString();

        if (isDone)
        {
            canvasPlay.SetActive(false);
            canvasField.SetActive(true);

            IdentifyChar();

            if (result[0].text == "Benar" && result[1].text == "Benar" && result[2].text == "Benar")
            {
                string[] kata = new string[3];

                for (int i = 0; i < kata.Length; i++)
                {
                    kata[i] = InputKata[i].text;
                }

                FieldSoal = string.Join(",", kata);

                Soaltxt.SetActive(true);

                soal.text = "Buatlah kalimat tersebut dengan kata " + FieldSoal;

                Kalimat.SetActive(true);

            }

        }

    }
    private void randomingChar()
    {
        foreach (char c in alphabet)
        {
            alphabets.Add(c);
        }

        int jAlpha = alphabets.Count;

        for (int i = 0; i < Jsoal; i++)
        {

            int Random = i + (int)(rand.NextDouble() * (jAlpha - i));
            randomedChar.Add(alphabets[Random]);

        }

        int dCount = randomedChar.Count;

        for (int i = 0; i < dCount; i++)
        {

            sameChar.Add(randomedChar[i]);
            difChar.Add(randomedChar[i + 1]);

            i++;
        }


        int SCount = sameChar.Count;

        for (int i = 0; i < SCount; i++)
        {

            sameClone.Add(sameChar[i]);
            sameClone.Add(sameChar[i]);
        }

        for (int i = 0; i < sameClone.Count; i++)
        {
            Debug.Log(sameClone[i]);
        }


    }

    private void GatherChar()
    {
        randomingChar();

        totalC = (sameClone.Count + difChar.Count);
        //int sparTC = totalC / 3;

        for (int i = 0; i < totalC; i++)
        {

            int select = (int)Random.Range(0, 2);
            int rand1 = (int)Random.Range(0, sameClone.Count);
            int rand2 = (int)Random.Range(0, difChar.Count);

            if (select == 0)
            {


                if (rand1 == 0)
                {

                    finalChar.Add(sameClone[rand1]);
                }
                else
                {
                    finalChar.Add(sameClone[rand1]);
                    finalChar.Add(sameClone[rand1 - 1]);

                    i++;
                }

                Debug.Log("Add same");
            }

            if (select == 1)
            {

                finalChar.Add(difChar[rand2]);
                Debug.Log("Add dif");
            }
        }

        Debug.Log("========================");

        for (int i = 0; i < finalChar.Count; i++)
        {
            Debug.Log(finalChar[i]);
        }

    }


    private void RandomingPosition()
    {
        Text temp;

        int fc = floor.Length;
        int tfC = Jsoal + (Jsoal / 2);
        Text[] sameFloor = new Text[Jsoal];
        Text[] difFloor = new Text[Jsoal / 2];


        for (int i = 0; i < fc; i++)
        {

            int random = i + (int)(rand.NextDouble() * (fc - i));

            temp = floor[random];
            floor[random] = floor[i];
            floor[i] = temp;

        } // shuffle floor

        for (int i = 0; i < Jsoal; i++)
        {

            int random = (int)Random.Range(0, fc);

            sameFloor[i] = floor[random];
            sameFloor[i + 1] = floor[random];

            i++;
        }

        for (int i = 0; i < Jsoal / 2; i++)
        {

            int random = (int)Random.Range(0, fc);
            difFloor[i] = floor[random];
        }

        for (int i = 0; i < tfC; i++)
        {

            int random = (int)Random.Range(0, 2);
            int rand1 = (int)Random.Range(0, sameFloor.Length);
            int rand2 = (int)Random.Range(0, difFloor.Length);

            if (random == 0)
            {

                tFl.Add(sameFloor[rand1]);
            }

            if (random == 1)
            {

                tFl.Add(difFloor[rand2]);

            }
        }

        for (int i = 0; i < tfC; i++)
        {

            Debug.Log(tFl.ToString());
        }

    }


    IEnumerator SetTask()
    {
        int JtS = Jsoal + (Jsoal / 2);

        for (int i = 0; i < JtS; i++)
        {

            tFl[i].text = finalChar[i].ToString();

            if (i > 0)
            {
                if (finalChar[i] == finalChar[i - 1])
                {

                    CharisMatched = true;


                }
                if (tFl[i] == tFl[i - 1])
                {

                    FloorisMatched = true;

                }
            }

            yield return new WaitForSeconds(2f);

            CharisMatched = false;
            FloorisMatched = false;

            tFl[i].text = "";

            CharButton.SetActive(true);
            PostButton.SetActive(true);

        }

        isDone = true;
    }

    private void CheckDouble()
    {
        int TotalChar = finalChar.Count;
        int TotalFloor = tFl.Count;

        for (int i = 0; i < TotalChar - 1; i++)
        {

            if (finalChar[i] == finalChar[i + 1])
            {

                tSameChar = tSameChar + 1;
                wasDoubled.Add(finalChar[i]);

            }

        }

        Debug.Log(tSameChar);

        for (int i = 0; i < TotalFloor - 1; i++)
        {

            if (tFl[i] == tFl[i + 1])
            {

                tSameFloor = tSameFloor + 1;
            }
        }

        Debug.Log(tSameFloor);

        print("====================");

        IEnumerable<char> distinctSame = wasDoubled.Distinct();

        foreach (char c in distinctSame)
        {
            Debug.Log(c);
            distinctDouble.Add(c);
        }

    }

    public void ClickHuruf()
    {
        if (CharisMatched)
        {

            tGetChar = tGetChar + 1;
        }

        CharButton.SetActive(false);

    }
    public void ClickPosisi()
    {
        if (FloorisMatched)
        {

            tGetFloor = tGetFloor + 1;
        }

        PostButton.SetActive(false);

    }

    private void IdentifyChar()
    {

        for (int i = 0; i < 3; i++)
        {
            inputKata[i].SetActive(true);
        }

    }


    public void Check()
    {
        bool isMatched = CheckKamus(InputKata[0].text);

        if (isMatched)
        {
            result[0].text = "Benar";
            InputKata[0].readOnly = true;
        }
        else
        {
            result[0].text = "Salah";
            InputKata[0].text = "";
        }

    }

    public void Check2()
    {
        bool isMatched = CheckKamus(InputKata[1].text);

        if (isMatched)
        {
            result[1].text = "Benar";
            InputKata[1].readOnly = true;
        }
        else
        {
            result[1].text = "Salah";
            InputKata[1].text = "";
        }
    }

    public void Check3()
    {
        bool isMatched = CheckKamus(InputKata[2].text);

        if (isMatched)
        {
            result[2].text = "Benar";
            InputKata[2].readOnly = true;
        }
        else
        {
            result[2].text = "Salah";
            InputKata[2].text = "";
        }

    }

    private bool CheckKamus(string kata) //check kata, apakah kata depan nya sesuai dan apakah kata tersebut sesuai kamus data
    {
        char[] c = kata.ToCharArray();
        int i = kamus.Length;
        int j = distinctDouble.Count;
        bool isThrough = false;
        bool isMatched = false;

        print(j);
        print(i);
        print(kata);



        for (int y = 0; y < j; y++)
        {
            if (c[0] == distinctDouble[y])
            {
                isThrough = true;
                y = j;
            }
        }


        if (isThrough)
        {

            for (int x = 0; x < i; x++)
            {
                if (kata == kamus[x])
                {
                    isMatched = true;
                    x = i;
                }
            }

            if (isMatched)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    public void CheckKalimat()
    {
        if (kalimat.text.Contains(InputKata[0].text) && kalimat.text.Contains(InputKata[1].text) && kalimat.text.Contains(InputKata[2].text))
        {
            InsertButton.SetActive(true);
        }

        else
        {
            kalimat.text = "";
        }
    }



    /*
	 * WHAT TO DO NOW:
	 * REPLAY THE SCENE ABOUT 5 TIMES
	 * TAMPUNG SCORE TIAP REPLAY DALAM BENTUK STATIS
	 * BUILD 2 SCENE GAMEPLAY, SCOREBOARD
	*/


}
