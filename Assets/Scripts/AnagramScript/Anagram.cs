using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Anagram : MonoBehaviour
{


    static System.Random rand = new System.Random();

    private string soal;
    private string tipeSoal;

    private int LifePoint = 5;


    private bool isPlay = false;
    private bool isDone = false;

    public int JumlahString;
    public int desiredLength;

    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject ReplayButton;
    public GameObject TxtSentence;
    public GameObject SendButton;


    public Text[] txtObj;
    public Text Hp;
    public Text Result;
    public Text Clue;

    public InputField txtField;
    public InputField txtSentences;

    private string[] kamus;
    private string[] kategori;

    private List<char> kar = new List<char>(); //list character pada soal yg terpilih
    private List<char> distract = new List<char>() { '!', '@', '#', '$', '%', '&', '?', '<', '>', '{', '}', ']', '[', '|', '/', '~', '+', '^' };
    private List<char> desiredDistract = new List<char>(); //list distracte yang terpilih
    private List<char> Generate = new List<char>(); //list gabungan antara distraksi dan soal
    private List<char> FinalGenerate = new List<char>(); //list generate yang telah di random



    // Use this for initialization

    IEnumerator Start()
    {

        WWW items = new WWW("http://localhost/BrainGameDB/items.php");
        WWW itemsTheme = new WWW("http://localhost/BrainGameDB/itemsTheme.php");

        yield return items;
        yield return itemsTheme;

        string itemsString = items.text;
        string itemsThemeString = itemsTheme.text;

        kamus = itemsString.Split('|');
        kategori = itemsThemeString.Split('|');

        print(kamus.Length);

        GetSoal(kamus);

        ListMixer();

        StartCoroutine("PlayGame");



    }


    // Update is called once per frame
    void Update()
    {


        Hp.text = LifePoint.ToString();


        if (isDone && !isPlay)
        {

            canvas1.SetActive(false);
            canvas2.SetActive(true);
            Clue.text = tipeSoal.ToString();
        }

        if (!isDone && isPlay)
        {

            isDone = false;
            isPlay = false;
            SceneManager.LoadScene("Anagram");
        }

    }


    private string GetSoal(string[] soalRand)
    {
        int count = soalRand.Length;
        int random = Random.Range(0, count);

        string Soalrandom = soalRand[random];
        string Kategori = kategori[random];


        while (Soalrandom.Length != JumlahString)
        {

            random = Random.Range(0, count);
            Soalrandom = soalRand[random];
            Kategori = kategori[random];
        }

        soal = (string)Soalrandom;
        tipeSoal = (string)Kategori;

        soal = soal.ToUpper();

        print(soal);

        return soal;
    }

    static void RandomList(int[] array) // random prevent repeating index
    {
        int arraySize = array.Length;
        int random;
        int temp;

        for (int i = 0; i < arraySize; i++)
        {

            random = i + (int)(rand.NextDouble() * (arraySize - i));
            temp = array[random];
            array[random] = array[i];
            array[i] = temp;
        }
    }

    private void split()// split string soal into char, and store it to list
    {

        foreach (char c in soal)
        {
            kar.Add(c);
        }
    }

    private void Distraction() // add desired distraction
    {
        split();
        for (int r = 0; r < desiredLength; r++)
        {
            desiredDistract.Add(distract[(int)Random.Range(0, distract.Count)]);
        }
    }

    private void Store() // store char soal and distract into list
    {
        Distraction();
        int sumChar = (int)kar.Count;
        int sumDist = (int)desiredDistract.Count;

        for (int i = 0; i < sumChar; i++)
        {
            Generate.Add(kar[i]);
        }

        for (int j = 0; j < sumDist; j++)
        {
            Generate.Add(desiredDistract[j]);
        }
    }

    private void ListMixer() //mix the added list into random output
    {
        Store();
        int GenerLength = Generate.Count;
        int[] randomArray = new int[GenerLength];

        for (int i = 0; i < GenerLength; i++)
        {
            randomArray[i] = i;
        }

        RandomList(randomArray);

        for (int r = 0; r < GenerLength; r++)
        {

            FinalGenerate.Add(Generate[randomArray[r]]);
            Debug.Log(FinalGenerate[r]);
        }

    }

    IEnumerator PlayGame()
    {
        foreach (char r in FinalGenerate)
        {

            int s = (int)Random.Range(0, txtObj.Length);

            txtObj[s].text = r.ToString();

            yield return new WaitForSeconds(2f);

            txtObj[s].text = "";

        }
        isDone = true;
    }


    public void CheckJawaban()
    {
        string Jawaban = txtField.text;
        string Soal = soal;

        if (Jawaban == Soal)
        {
            Result.text = "Selamat, Jawaban anda benar!";
            Score.totalScore = 1000 * LifePoint;
            TxtSentence.SetActive(true);
            SendButton.SetActive(true);
        }
        else
        {
            Result.text = "Maaf, Jawaban anda Salah!";
            LifePoint--;
        }

        if (LifePoint == 0)
        {

            Result.text = "Maaf, Anda Gagal!";
            ReplayButton.SetActive(true);
        }
    }

    public void ReplayCLick()
    {
        isDone = false;
        isPlay = true;
    }

    public void SendDataClick()
    {
        string kalimat = txtSentences.text;
        if (kalimat.Contains(soal))
        {
            InsertAnagram.Insert(kalimat, soal);
            InsertScore.Insert("Anagram", Score.totalScore);

            print("Posted");
        }

        else
        {
            StartCoroutine("Btnply");
        }
    }

    IEnumerator Btnply()
    {
        SendButton.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        SendButton.SetActive(true);
    }



    /*
	 * WHAT TO DO NOW:
	 * BUILD REPLAY BUTTON (done)
	 * BUILD TIMER (done)
	 * BUILD TEXT FIELD POST TO DATABASe (done)
	 * TURN ON / OFF CANVAS (done)
	 * DETERMINE HP TEXT (done)
	 * DETERMINE SCORE TEXT INTO SCORE STATIS (done)
	*/
}
