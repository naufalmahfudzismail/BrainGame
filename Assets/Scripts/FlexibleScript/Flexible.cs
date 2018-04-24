using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Collection
{
    public static bool IsGame1 = true;
    public static bool IsGame2 = false;
    public static bool IsDone = false;
    public static bool counter = false;
    public static float Skor;
    public static int _counting = 0;
    public static int _count = 0;
    public static string[] kamus; // array kamus data
}

public class Flexible : MonoBehaviour
{
    private Connection conn;
    public GameObject Connection;

    static System.Random rand = new System.Random();

    public int JumlahCharacter = 40;
    public float WaktuPerStep = 2f;
    public int perulangan = 2;

    [HideInInspector] public Button CharButton;
    [HideInInspector] public Button PostButton;
    [HideInInspector] public Button SoundButton;
    [HideInInspector] public GameObject canvasError;
    [HideInInspector] public GameObject canvasLoading;
    [HideInInspector] public GameObject canvasPlay;
    [HideInInspector] public GameObject canvasField;
    [HideInInspector] public GameObject Kalimat;
    [HideInInspector] public GameObject InsertButton;
    [HideInInspector] public GameObject Soaltxt;
    [HideInInspector] public GameObject[] inputKata = new GameObject[3];
    [HideInInspector] public Slider slide;
    [HideInInspector] public AudioClip[] Audio = new AudioClip[7];
    [HideInInspector] public AudioSource[] source = new AudioSource[7];
    [HideInInspector] public InputField[] InputKata = new InputField[3];
    [HideInInspector] public InputField kalimat;
    [HideInInspector] public Text[] floor = new Text[9];
    [HideInInspector] public Text[] result;
    [HideInInspector] public Text soal;
    [HideInInspector] public Text charScore;
    [HideInInspector] public Text postScore;
    [HideInInspector] public Text soundScore;
    [HideInInspector] public Text Scores;
    [HideInInspector] public Text ResultCek;
    [HideInInspector] public Text Akurasi;
    [HideInInspector] public Text error;
    [HideInInspector] public Text Progress;
    [HideInInspector] public Text Round;
    [HideInInspector] public Text Kategori;
    [HideInInspector] public Text hint;
    [HideInInspector] public Text header;
    [HideInInspector] public Text Wrong;

    private double _wrong;
    private string FieldSoal;

    private bool CharisMatched = false;
    private bool FloorisMatched = false;
    private bool SoundisMatched = false;
    private bool isPaused = false;

    private int totalC;
    private int JumlahSoal;
    private float SlideValue;

    private string[] kategori = new string[3]
    {
       "HEWAN", "BUAH", "SAYURAN"
    };

    private List<char> wasDoubled = new List<char>(); //list double value char dari finalchar 
    private List<char> distinctDouble = new List<char>(); //list wasdouble yang telah di distinct
    private double tGetChar;
    private double tGetFloor;
    private double tGetSound;
    private double tSameChar;
    private double tSameFloor;
    private double tSameSound;

    int index;

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private List<char> alphabets = new List<char>(); //list alphabet sri string alphabet



    List<AudioSource> SuaraSource = new List<AudioSource>();// list source suara
    List<char> randomedChar = new List<char>(); //list char random
    List<char> sameChar = new List<char>(); //list randomed  yang akan di samakan
    List<char> sameClone = new List<char>(); //list samechar yang sudah di gandakan
    List<char> difChar = new List<char>(); //list randomechar yang berbeda
    List<char> finalChar = new List<char>(); //list dari gabungan anatar random list sameclone dan difchar
    List<char> _finalChar = new List<char>(); //disticnt finalchar
    List<Text> tFl = new List<Text>(); //list gameobject text

    private void Awake()
    {
        Screen.SetResolution(600, 1024, true);
        conn = Connection.GetComponent<Connection>();


        ObjectActive();

        if (JumlahCharacter % 2 != 0)
        {
            print("JumlahCharacter Karater tidak bisa ganjil, set Default = 40");
            JumlahCharacter = 40;
        }



    }

    private IEnumerator ShowProgress(WWW www)
    {
        canvasError.SetActive(true);
        if (!string.IsNullOrEmpty(www.error) || Application.internetReachability == NetworkReachability.NotReachable)
        {
            error.text = "Network Connection Unavailable";
            Progress.text = "";
            Time.timeScale = 0;
            isPaused = true;
        }

        else
        {
            error.text = "Waiting...";
            while (!www.isDone)
            {

                Progress.text = string.Format("Downloaded {0:P1}", www.progress);
                yield return new WaitForSeconds(.1f);

            }
            canvasError.SetActive(false);
            isPaused = false;

            Debug.Log("Done");
            canvasPlay.SetActive(true);
        }
    }

    // Use this for initialization
    IEnumerator Start()
    {
        if (Collection._counting == 0)
        {
            WWW items = new WWW(conn.getUrlKataDasar());
            StartCoroutine(ShowProgress(items));
            yield return items;
            string itemsString = items.text;

            Collection.kamus = itemsString.Split('|');
            for (int i = 0; i < Collection.kamus.Length; i++)
            {
                Collection.kamus[i] = Collection.kamus[i].ToUpper();
            }

            for (int i = 0; i < Collection.kamus.Length; i++)
            {
                print(Collection.kamus[i]);
            }
        }

        index = Random.Range(0, kategori.Length - 1);
        Kategori.text = kategori[index];

        if (!isPaused)
        {
            if (Collection.IsGame2 && !Collection.IsGame1)
            {
                StartCoroutine("Loading");
                canvasLoading.SetActive(false);
                Round.text = "Round 2";
                hint.text = "Pada Round 2 : Ingat lah huruf huruf yang muncul ketika huruf itu muncul sama dengan permunculan pada 2 urutan sebelum nya";
                header.text = "Pada Round 2 : Klik Character / Position/ Sound , jika huruf / posisi / suara yang muncul sama seperti 2 urutan sebelum nya";
            }

            if (Collection.IsGame1 && !Collection.IsGame2)
            {
                hint.text = "Pada Round 1 : Ingat lah huruf huruf yang muncul ketika huruf itu muncul sama dengan permunculan pada 1 urutan sebelum nya";
                header.text = "Pada Round 1 : Klik Character / Position/ Sound , jika huruf / posisi / suara yang muncul sama seperti 1 urutan sebelum nya";
                Round.text = "Round 1";
            }

            StartCoroutine("SetTask");
            CheckDouble();
        }
    }

    private void ObjectActive()
    {
        canvasLoading.SetActive(false);
        canvasPlay.SetActive(true);
        canvasField.SetActive(false);
        Kalimat.SetActive(false);
        InsertButton.SetActive(false);
        Soaltxt.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            inputKata[i].SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            canvasError.SetActive(true);
            error.text = "Network Connection Unavailable";
            Progress.text = "";
            Time.timeScale = 0;
            isPaused = true;
        }


        charScore.text = tGetChar.ToString();
        postScore.text = tGetFloor.ToString();
        soundScore.text = tGetSound.ToString();

        if (Collection.IsDone)
        {
            float accurate = (float)(((tGetChar + tGetFloor + tGetSound) - _wrong) / (tSameChar + tSameFloor + tSameSound)) * 100;
            int Total = (int)(accurate * 1000) - (int)(_wrong * 100);

            if (Total < 0)
                Total = 0;

            Scores.text = "Score Game : " + (Total);
            Akurasi.text = "Ketepatan : " + accurate + " %";
            Wrong.text = "Kesalahan : " + _wrong;
            Collection.Skor = Collection.Skor + Total;

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

    IEnumerator Loading()
    {
        canvasLoading.SetActive(true);
        yield return new WaitForSeconds(5f);

    }
    private void RandomingChar()
    {
        foreach (char c in alphabet)
        {
            alphabets.Add(c);
        }

        if (JumlahCharacter > alphabets.Count)
        {
            int k = JumlahCharacter - alphabets.Count;
            for (int i = 0; i < k; i++)
            {
                int rand = (int)Random.Range(0, alphabets.Count);
                alphabets.Add(alphabets[rand]);
            }
        }

        int jAlpha = alphabets.Count;

        for (int i = 0; i < JumlahCharacter; i++)
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
        RandomingChar();

        totalC = (sameClone.Count + difChar.Count);


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
        int tfC = JumlahCharacter + (JumlahCharacter / 2);
        Text[] sameFloor = new Text[JumlahCharacter];
        Text[] difFloor = new Text[JumlahCharacter / 2];


        for (int i = 0; i < fc; i++)
        {

            int random = i + (int)(rand.NextDouble() * (fc - i));

            temp = floor[random];
            floor[random] = floor[i];
            floor[i] = temp;

        } // shuffle floor

        for (int i = 0; i < JumlahCharacter; i++)
        {

            int random = (int)Random.Range(0, fc);

            sameFloor[i] = floor[random];
            sameFloor[i + 1] = floor[random];

            i++;
        }

        for (int i = 0; i < JumlahCharacter / 2; i++)
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

    private void RandomingSound()
    {
        int d = Audio.Length;
        int total = JumlahCharacter + JumlahCharacter / 2;

        for (int i = 0; i < source.Length; i++)
        {
            source[i].clip = Audio[i];
        }

        for (int i = 0; i < total; i++)
        {
            int r = Random.Range(0, d);
            SuaraSource.Add(source[r]);
        }

    }


    IEnumerator SetTask()
    {

        GatherChar();
        RandomingPosition();
        RandomingSound();
        int counter = 1;

        if (Collection.IsGame1)
            counter = 1;
        if (Collection.IsGame2)
            counter = 2;


        int JtS = JumlahCharacter + (int)(JumlahCharacter / 2);
        slide.maxValue = JtS;
        SlideValue = JtS;

        for (int i = 0; i < JtS; i++)
        {
            tFl[i].text = finalChar[i].ToString();
            SuaraSource[i].Play();

            slide.value = SlideValue;

            if (i >= counter)
            {
                if (finalChar[i] == finalChar[i - counter])
                {

                    CharisMatched = true;


                }
                if (tFl[i] == tFl[i - counter])
                {

                    FloorisMatched = true;

                }

                if (SuaraSource[i].ToString() == SuaraSource[i - counter].ToString())
                {
                    SoundisMatched = true;
                }
            }

            yield return new WaitForSeconds(WaktuPerStep);

            CharisMatched = false;
            FloorisMatched = false;
            SoundisMatched = false;

            tFl[i].text = "";
            SuaraSource[i].Stop();
            SlideValue = SlideValue - 1;

            CharButton.interactable = true;
            PostButton.interactable = true;
            SoundButton.interactable = true;

        }

        Collection.IsDone = true;
    }

    private void CheckDouble()
    {
        int TotalChar = finalChar.Count;
        int TotalFloor = tFl.Count;
        int TotalSound = SuaraSource.Count;
        int counter = 1;

        if (Collection.IsGame1)
            counter = 1;
        if (Collection.IsGame2)
            counter = 2;


        for (int i = 0; i < TotalChar - counter; i++)
        {

            if (finalChar[i] == finalChar[i + counter])
            {

                tSameChar = tSameChar + 1;
                wasDoubled.Add(finalChar[i]);

            }

        }

        Debug.Log(tSameChar);

        for (int i = 0; i < TotalFloor - counter; i++)
        {

            if (tFl[i] == tFl[i + counter])
            {

                tSameFloor = tSameFloor + 1;
            }
        }

        Debug.Log(tSameFloor);

        for (int i = 0; i < TotalSound - counter; i++)
        {

            if (SuaraSource[i] == SuaraSource[i + counter])
            {

                tSameSound = tSameSound + 1;
            }
        }

        Debug.Log(tSameSound);

        print("====================");

        IEnumerable<char> distinctSame = finalChar.Distinct();

        foreach (char c in distinctSame)
        {
            Debug.Log(c);
            _finalChar.Add(c);
        }

    }

    public void ClickHuruf()
    {
        if (CharisMatched)
        {

            tGetChar = tGetChar + 1;
        }

        else
        {
            _wrong = _wrong + 1;
        }

        CharButton.interactable = false;

    }
    public void ClickPosisi()
    {
        if (FloorisMatched)
        {

            tGetFloor = tGetFloor + 1;
        }

        else
        {
            _wrong = _wrong + 1;
        }

        PostButton.interactable = false;

    }

    public void ClickSound()
    {
        if (SoundisMatched)
        {

            tGetSound = tGetSound + 1;
        }

        else
        {
            _wrong = _wrong + 1;
        }

        SoundButton.interactable = false;

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
        InputKata[0].text = InputKata[0].text.ToUpper();
        bool isMatched = CheckKamus(InputKata[0].text);

        if (isMatched && InputKata[1].text != InputKata[0].text && InputKata[2].text != InputKata[0].text)
        {
            result[0].text = "Benar";
            Collection.Skor = Collection.Skor + 100;
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
        InputKata[1].text = InputKata[1].text.ToUpper();
        bool isMatched = CheckKamus(InputKata[1].text);

        if (isMatched && InputKata[0].text != InputKata[1].text && InputKata[2].text != InputKata[1].text)
        {
            result[1].text = "Benar";
            Collection.Skor = Collection.Skor + 100;
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
        InputKata[2].text = InputKata[2].text.ToUpper();
        bool isMatched = CheckKamus(InputKata[2].text);

        if (isMatched && InputKata[0].text != InputKata[2].text && InputKata[1].text != InputKata[2].text)
        {
            result[2].text = "Benar";
            Collection.Skor = Collection.Skor + 100;
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
        int i = Collection.kamus.Length;
        int j = _finalChar.Count;
        bool isThrough = false;
        bool isMatched = false;

        print(j);
        print(i);
        print(kata);


        if (kata != null || kata != "")
        {
            for (int y = 0; y < j; y++)
            {
               
                    if (c[0] == _finalChar[y])
                    {
                        isThrough = true;
                        y = j;
                    }
                
            }
            if (isThrough)
            {
                for (int x = 0; x < i; x++)
                {
                    if (kata == Collection.kamus[x])
                    {
                        isMatched = true;
                        x = i;
                    }
                }

                if (isMatched)
                {
                    int k = System.Array.IndexOf<string>(Collection.kamus, kata);
                    print(k);
                    print(k + 1);
                    string d = Collection.kamus[k + 1];

                    if (d == kategori[index])
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
        if (kalimat != null)
        {
            kalimat.text = kalimat.text.ToUpper();
            if (kalimat.text.Contains(InputKata[0].text) && kalimat.text.Contains(InputKata[1].text) && kalimat.text.Contains(InputKata[2].text))
            {
                Collection.Skor = Collection.Skor + 500;
                ResultCek.text = "Benar!";
                InsertButton.SetActive(true);
                kalimat.readOnly = true;
            }

            else
            {
                ResultCek.text = "Kalimat belum sesuai";
                kalimat.text = "";

                StartCoroutine("Delay");
                ResultCek.text = "Tap here";

            }
        }
    }

    public void InsertData()
    {
        Collection._counting = Collection._counting + 1;

        Score.totalScore = Score.totalScore + (int)Collection.Skor;

        conn.InsertKalimat("Flexible", kalimat.text, FieldSoal);

        conn.InsertScore("Flexible N-Back", Score.totalScore);

        if (Collection._count >= perulangan)
            SceneManager.LoadScene("Over");
        else
        {
            if (Collection.IsGame1 && !Collection.IsGame2)
            {
                Collection.IsGame1 = false;
                Collection.IsGame2 = true;
                Collection.IsDone = false;
             
                print(Collection.IsGame1);
                print(Collection.IsGame2);
                print(Collection.IsDone);
            }

            else if (Collection.IsGame2 && !Collection.IsGame1)
            {
                Collection._count = Collection._count + 1;
                Collection.IsGame1 = true;
                Collection.IsGame2 = false;
                Collection.IsDone = false;

                print(Collection.IsGame1);
                print(Collection.IsGame2);
                print(Collection.IsDone);
            }

            SceneManager.LoadScene("Flexible");
        }


        print("Posted");
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
    }

    public void Test()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Time.timeScale = 1;
            canvasError.SetActive(false);
            isPaused = false;
        }
    }




    /*
	 * WHAT TO DO NOW:
	 * REPLAY THE SCENE ABOUT 5 TIMES
	 * TAMPUNG SCORE TIAP REPLAY DALAM BENTUK STATIS
	 * BUILD 2 SCENE GAMEPLAY, SCOREBOARD
	*/


}
