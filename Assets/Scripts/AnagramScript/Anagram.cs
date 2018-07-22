using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Levels
{
    public static int JLevel = 1;
    public static string[] kamus;

    public static string[] kategori;

    public static string[] SoalKata;

    public static void RandomList(int[] array) // random prevent repeating index
    {
        int arraySize = array.Length;
        int random;
        int temp;

        for (int i = 0; i < arraySize; i++)
        {
            random = Random.Range(0, arraySize - 1);
            temp = array[random];
            array[random] = array[i];
            array[i] = temp;
        }
    }
}


public class Anagram : MonoBehaviour
{
    static System.Random rand = new System.Random();
    public string[] SoalKataDasar;
    public bool InputManualSoal = true;
    public int JumlahMaksimalHuruf = 4;
    public int JumlahLevel = 1;
    public int KesempatanMengingatHuruf = 5;
    public float WaktuMembuatKalimat = 60;
    public int ScorePerHuruf = 100;
    public int ScorePerSatuKata = 1000;
    public int ScoreMembuatKalimat = 2000;
    private Connection conn;
    [HideInInspector] public GameObject Connection;

    private string soal;
    private string tipeSoal;
    private string choosenSoal;
    private const string KATADASAR = "Assets/Database/katadasar.txt";
    private const string TEMA = "Assets/Database/tema.txt";
    private int score;
    private int[] randomArray = new int[12];
    private string[] chsoal;
    private int desiredLength;
    private bool isPlay = false;
    private bool isRemember = false;
    private bool isDone = false;
    private bool sudahlah = true;
    private bool isPaused = false;
    private float timer = 0;
    private float TimerCount = 25;
    public float WaktuIngatPerChar;

    [HideInInspector] public GameObject canvasError;
    [HideInInspector] public GameObject canvas1;
    [HideInInspector] public GameObject canvas2;
    [HideInInspector] public GameObject ReplayButton;
    [HideInInspector] public GameObject TxtSentence;
    [HideInInspector] public GameObject SendButton;
    [HideInInspector] public GameObject[] TextObjects; //Tempat menampung text
    [HideInInspector] public Button[] BlockButton;
    [HideInInspector] public Text[] txtObj;   // Objek text nya
    [HideInInspector] public Text Hp;
    [HideInInspector] public Text Result;
    [HideInInspector] public Text Clue;
    [HideInInspector] public Text countDown;
    [HideInInspector] public Text lifecount;
    [HideInInspector] public Text status;
    [HideInInspector] public Text ChoosenString;
    [HideInInspector] public Text ScoreWhile;
    [HideInInspector] public Text Round;
    [HideInInspector] public Text error;
    [HideInInspector] public Text Progress;
    [HideInInspector] public InputField txtField;
    [HideInInspector] public InputField txtSentences;
    [HideInInspector] public Text TxtBtnError;



    private List<char> kar = new List<char>(); //list character pada soal yg terpilih
    private List<string> kars = new List<string>();// list string soal
    private List<char> distract = new List<char>()
    {
        '!', '@', '#', '$', '%', '&', '?', '<', '>', '{', '}', ']', '[', '|', '/', '~', '+', '^', '1', '2', '3', '4', '5', '6', '7', '8', '9'
    };
    private List<char> desiredDistract = new List<char>(); //list distracte yang terpilih
    private List<string> choosen = new List<string>();

    // Use this for initialization

    private void Awake()
    {
        Screen.SetResolution(600, 1024, true);
        conn = Connection.GetComponent<Connection>();
        canvasError.SetActive(false);
    }

    private string[] getDatafromFile(string path)
    {
        StreamReader reader = new StreamReader(path);
        string data = reader.ReadLine();
        string[] datas = data.Split('|');
        return datas;
    }

    private IEnumerator ShowProgress(WWW www)
    {
        canvasError.SetActive(true);
        if (!string.IsNullOrEmpty(www.error) || Application.internetReachability == NetworkReachability.NotReachable)
        {

            error.text = www.error;
            Time.timeScale = 0;
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


            Debug.Log("Done");
            canvas1.SetActive(true);
        }
    }


    void Start()
    {

        if (Levels.JLevel == 1)
        {
            if (!InputManualSoal)
            {
                Levels.kamus = getDatafromFile(KATADASAR);
                print(Levels.kamus.Length);
            }

            else
            {
                List<string> strlist = new List<string>();
                int[] IndexKata = new int[SoalKataDasar.Length];
                for (int i = 0; i < SoalKataDasar.Length; i++)
                {
                    IndexKata[i] = i;
                }
                Levels.RandomList(IndexKata);
                for (int i = 0; i < SoalKataDasar.Length; i++)
                {
                    print(IndexKata[i]);
                    strlist.Add(SoalKataDasar[IndexKata[i]]);
                }

                Levels.kamus = strlist.ToArray();
    
                foreach (string c in Levels.kamus)
                {
                    print(c);
                }
            }

        }
        CloseButton();
        GetSoal(Levels.kamus);

        StartCoroutine("TextinObject");

    }

    void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else if (!isPaused)
        {
            Time.timeScale = 1f;
        }

        /*if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            canvasError.SetActive(true);
            error.text = "Network Connection Unavailable";
            Progress.text = "";
            TxtBtnError.text = "Retry";
            isPaused = true;
        }*/

        Timer();
        Round.text = "Round : " + Levels.JLevel;
        lifecount.text = "Chance : " + KesempatanMengingatHuruf.ToString();
        ScoreWhile.text = "Score sementara :" + score;

        if (isRemember)
        {
            OpenButton();

            sudahlah = false;

            if (kar.Count == choosen.Count)
            {
                StartCoroutine("Delays");
                isRemember = false;
                isDone = true;
            }

        }


        if (isDone && !isPlay && !isRemember)
        {

            Timer2();

            canvas1.SetActive(false);
            canvas2.SetActive(true);

            choosenSoal = string.Join(",", chsoal);
            ChoosenString.text = "Susunlah huruf " + choosenSoal + " menjadi sebuah kata !";
            Clue.text = "Good Luck";
        }

        if (KesempatanMengingatHuruf == 0)
        {
            /*conn.InsertAnagram(Akun.username, Levels.JLevel.ToString(), choosenSoal, score, "^Gagal dalam menebak huruf^", soal);
            Score.totalScore = score;
            SceneManager.LoadScene("Over");*/
            isDone = true;
            isPlay = false;
            isRemember = false;
        }

    }
    void CloseButton()
    {
        for (int b = 0; b < BlockButton.Length; b++)
        {
            BlockButton[b].interactable = false;
        }
    }

    private string[] shuffleChar(string[] array)
    {
        int Length = array.Length;
        int[] index = new int[Length];
        string[] shuffleChar = new string[Length];

        for (int i = 0; i < Length; i++)
        {
            index[i] = i;
        }
        Levels.RandomList(index);

        for (int i = 0; i < Length; i++)
        {
            shuffleChar[i] = array[index[i]];
        }

        return shuffleChar;
    }

    void OpenButton()
    {
        if (sudahlah)
        {
            for (int b = 0; b < BlockButton.Length; b++)
            {
                BlockButton[b].interactable = true;
            }
        }

    }

    private string GetSoal(string[] soalRand)
    {
        if (!InputManualSoal)
        {
            int count = soalRand.Length;
            int random = Random.Range(0, count);
            string Soalrandom = soalRand[random];

            while (Soalrandom.Length > JumlahMaksimalHuruf + 1)
            {

                random = Random.Range(0, count);
                Soalrandom = soalRand[random];
            }

            soal = (string)Soalrandom;

            soal = soal.ToUpper();

            print(soal);


        }

        else
        {
            int i = Levels.JLevel;
            soal = Levels.kamus[i -1];
            soal = soal.ToUpper();
            print(soal);

        }

        return soal;
    }

    private void Split()// split string soal into char, and store it to list
    {

        foreach (char c in soal)
        {
            kar.Add(c);
            kars.Add(c.ToString());
        }

        chsoal = shuffleChar(kars.ToArray());
    }

    private void Distraction() // add desired distraction
    {
        Split();

        desiredLength = 12 - kar.Count;
        for (int r = 0; r < desiredLength; r++)
        {
            desiredDistract.Add(distract[(int)Random.Range(0, distract.Count)]);
        }
    }


    IEnumerator TextinObject() //set text on board
    {

        Distraction();
        TimerCount = 12 * WaktuIngatPerChar;

        int limit = kar.Count;

        int[] StockRand = new int[12];

        List<char> Stock = new List<char>();

        for (int i = 0; i < 12; i++)
        {
            if (i >= limit)
            {
                Stock.Add(desiredDistract[i - limit]);
            }
            else
            {
                Stock.Add(kar[i]);
            }
        }

        for (int i = 0; i < txtObj.Length; i++)
        {
            randomArray[i] = i;
        }

        for (int i = 0; i < StockRand.Length; i++)
        {
            StockRand[i] = i;
        }

        Levels.RandomList(randomArray);
        Levels.RandomList(StockRand);

        for (int i = 0; i < txtObj.Length; i++)
        {
            txtObj[randomArray[i]].text = Stock[StockRand[i]].ToString();
            print(txtObj[randomArray[i]].text);
            yield return new WaitForSeconds(WaktuIngatPerChar);
            TextObjects[randomArray[i]].SetActive(false);
        }

    }

    private void CloseText() // close the text
    {
        for (int b = 0; b < TextObjects.Length; b++)
        {
            TextObjects[b].SetActive(false);
        }
    }


    private void Timer() //time to remember
    {

        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer = 0;

            if (TimerCount > 0)
            {
                TimerCount--;
                string minutes = Mathf.Floor(TimerCount / 60).ToString("00");
                string seconds = Mathf.Floor(TimerCount % 60).ToString("00");
                countDown.text = "Timer     " + minutes + ":" + seconds;
            }

            if (TimerCount == 0 && !isDone)
            {
                isRemember = true;
                CloseText();
                TimerCount = -1;

            }

        }
    }

    public void Timer2()
    {

        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer = 0;

            if (WaktuMembuatKalimat > 0)
            {
                WaktuMembuatKalimat--;
                string minutes = Mathf.Floor(WaktuMembuatKalimat / 60).ToString("00");
                string seconds = Mathf.Floor(WaktuMembuatKalimat % 60).ToString("00");
                Hp.text = "Timer     " + minutes + ":" + seconds;
            }

            if (WaktuMembuatKalimat == 0)
            {
                Score.totalScore = score;
                conn.InsertAnagram(Akun.username, Levels.JLevel.ToString(), choosenSoal, score, "Gagal dalam game ini", soal);
                SceneManager.LoadScene("Over");
            }
        }
    }

    public void CheckJawaban()  // check answer
    {
        if (txtField.text != null)
        {
            txtField.text = txtField.text.ToUpper();
            string Jawaban = txtField.text;
            string Soal = soal;

            if (Jawaban == Soal)
            {
                Result.text = "Selamat, Jawaban anda benar!";
                score = score + ScorePerSatuKata;
                TxtSentence.SetActive(true);
                SendButton.SetActive(true);
            }
            else
            {
                Result.text = "Maaf, Jawaban anda Salah!";

            }
        }
    }

    public void ReplayCLick()
    {
        isDone = false;
        isPlay = true;
    }

    public void SendDataClick()
    {   //This method actually working
        txtSentences.text = txtSentences.text.ToUpper();
        string kalimat = txtSentences.text;

        txtSentences.readOnly = true;

        if (Levels.JLevel == JumlahLevel)
        {
            score = score + ScoreMembuatKalimat;
            conn.InsertAnagram(Akun.username, Levels.JLevel.ToString(), choosenSoal, score, kalimat, soal);
            Score.totalScore = score;
            SceneManager.LoadScene("Over");
            print("Posted");
        }

        else
        {
            score = score + ScoreMembuatKalimat;
            conn.InsertAnagram(Akun.username, Levels.JLevel.ToString(), choosenSoal, score, kalimat, soal);
            Levels.JLevel++;
            TimerCount = 12 * WaktuIngatPerChar;
            SceneManager.LoadScene("Anagram");
        }

    }

    IEnumerator Btnply()
    {
        SendButton.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        SendButton.SetActive(true);
    }

    IEnumerator Delays()
    {
        if (kar.Count == choosen.Count)
        {
            status.text = "Nice Work!";

        }
        else
        {
            status.text = " Salah !";
            yield return new WaitForSeconds(2f);
            status.text = "";
        }

    }

    //button function 
    public void Btn1()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[0].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[0].SetActive(true);
            choosen.Add(txtObj[0].text);
            score = score + ScorePerHuruf;
            BlockButton[0].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");
        }


    }

    public void Btn2()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[1].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[1].SetActive(true);
            choosen.Add(txtObj[1].text);
            score = score + ScorePerHuruf;
            BlockButton[1].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");

        }

    }

    public void Btn3()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[2].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[2].SetActive(true);
            choosen.Add(txtObj[2].text);
            score = score + ScorePerHuruf;
            BlockButton[2].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");
        }

    }

    public void Btn4()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[3].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[3].SetActive(true);
            choosen.Add(txtObj[3].text);
            score = score + ScorePerHuruf;
            BlockButton[3].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");

        }

    }

    public void Btn5()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[4].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[4].SetActive(true);
            choosen.Add(txtObj[4].text);
            score = score + ScorePerHuruf;
            BlockButton[4].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");

        }

    }

    public void Btn6()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[5].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[5].SetActive(true);
            choosen.Add(txtObj[5].text);
            score = score + ScorePerHuruf;
            BlockButton[5].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");

        }

    }

    public void Btn7()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[6].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[6].SetActive(true);
            choosen.Add(txtObj[6].text);
            score = score + ScorePerHuruf;
            BlockButton[6].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");

        }

    }

    public void Btn8()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[7].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[7].SetActive(true);
            choosen.Add(txtObj[7].text);
            score = score + ScorePerHuruf;
            BlockButton[7].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");

        }

    }

    public void Btn9()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[8].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[8].SetActive(true);
            choosen.Add(txtObj[8].text);
            score = score + ScorePerHuruf;
            BlockButton[8].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");

        }

    }

    public void Btn10()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[9].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[9].SetActive(true);
            choosen.Add(txtObj[9].text);
            score = score + ScorePerHuruf;
            BlockButton[9].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");

        }

    }

    public void Btn11()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[10].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[10].SetActive(true);
            choosen.Add(txtObj[10].text);
            score = score + ScorePerHuruf;
            BlockButton[10].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");
        }

    }

    public void Btn12()
    {
        bool isThat = false;

        for (int x = 0; x < kar.Count; x++)
        {
            if (txtObj[11].text == kar[x].ToString())
            {
                isThat = true;
                x = kar.Count + 1;
            }
        }

        if (isThat)
        {
            TextObjects[11].SetActive(true);
            choosen.Add(txtObj[11].text);
            score = score + ScorePerHuruf;
            BlockButton[11].interactable = false;
        }

        else
        {
            KesempatanMengingatHuruf = KesempatanMengingatHuruf - 1;
            StartCoroutine("Delays");

        }

    }

    public void Test()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            canvasError.SetActive(false);
            isPaused = false;
        }
        else
        {
            Progress.text = "No Network Connection!";
        }
    }

    public void Paused()
    {
        canvasError.SetActive(true);
        error.text = "Game is Paused";
        Progress.text = "";
        TxtBtnError.text = "Resume";
        isPaused = true;
    }

}
