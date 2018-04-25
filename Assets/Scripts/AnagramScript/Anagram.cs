using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Levels
{
    public static int JLevel = 1;
    public static string[] kamus;
    public static string[] kategori;
}


public class Anagram : MonoBehaviour
{
    static System.Random rand = new System.Random();


    public int JumlahMaksimalHuruf = 4;
    public int JumlahLevel = 1;
    private Connection conn;
    [HideInInspector]public GameObject Connection;

    private string soal;
    private string tipeSoal;
    private string choosenSoal;

    private int LifePoint = 5;
    private int LifeRemember = 5;
    private int score;
    private int[] randomArray = new int[12];
    private int desiredLength;


    private bool isPlay = false;
    private bool isRemember = false;
    private bool isDone = false;
    private bool sudahlah = true;
    private bool isPaused = false;




    private float timer = 0;
    private float TimerCount = 25;


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



    private List<char> kar = new List<char>(); //list character pada soal yg terpilih
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


    IEnumerator Start()
    {

        if (Levels.JLevel == 1)
        {
            WWW items = new WWW(conn.getUrlKataDasar());
            WWW itemsTheme = new WWW(conn.getUrlKategori());

            StartCoroutine(ShowProgress(items));

            yield return items;
            yield return itemsTheme;



            string itemsString = items.text;
            string itemsThemeString = itemsTheme.text;

            Levels.kamus = itemsString.Split('|');
            Levels.kategori = itemsThemeString.Split('|');
            print(Levels.kamus.Length);
        }

     
        CloseButton();
        GetSoal(Levels.kamus, Levels.kategori);
        StartCoroutine("TextinObject");


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

        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Time.timeScale = 1;
            canvasError.SetActive(false);
            isPaused = false;
        }

        Timer();

        Round.text = "Round : " + Levels.JLevel;

        Hp.text = "Kesempatan menyusun : " + LifePoint.ToString();
        lifecount.text = "Chance : " + LifeRemember.ToString();
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
            string[] dummy = new string[choosen.Count];

            for (int d = 0; d < choosen.Count; d++)
            {
                dummy[d] = choosen[d];
            }

            canvas1.SetActive(false);
            canvas2.SetActive(true);

            choosenSoal = string.Join(",", dummy);
            ChoosenString.text = "Susunlah huruf " + choosenSoal + " menjadi sebuah kata !";
            Clue.text = tipeSoal.ToString();
        }

        if (LifeRemember == 0 || LifePoint == 0)
        {

            conn.InsertScoreAna(Akun.username, score);
            Score.totalScore = score;
            SceneManager.LoadScene("Over");
        }

    }



    void CloseButton()
    {
        for (int b = 0; b < BlockButton.Length; b++)
        {
            BlockButton[b].interactable = false;
        }
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


    private string GetSoal(string[] soalRand, string[] tipe)
    {
        int count = soalRand.Length;
        int random = Random.Range(0, count);

        string Soalrandom = soalRand[random];
        string Tipe = tipe[random];


        while (Soalrandom.Length > JumlahMaksimalHuruf + 1)
        {

            random = Random.Range(0, count);
            Soalrandom = soalRand[random];
            Tipe = Levels.kategori[random];
        }

        soal = (string)Soalrandom;
        tipeSoal = (string)Tipe;

        soal = soal.ToUpper();

        print(soal);

        return soal;
    }



    private void Split()// split string soal into char, and store it to list
    {

        foreach (char c in soal)
        {
            kar.Add(c);
        }
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

        RandomList(randomArray);
        RandomList(StockRand);

        for (int i = 0; i < txtObj.Length; i++)
        {
            txtObj[randomArray[i]].text = Stock[StockRand[i]].ToString();
            yield return new WaitForSeconds(2);
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

            if (TimerCount == 0)
            {
                isRemember = true;
                CloseText();
                TimerCount = -1;

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
                score = score + (1000 * LifePoint);
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
        if (kalimat.Contains(soal))
        {

            if (Levels.JLevel == JumlahLevel)
            {
                score = score + 2000;
                conn.InsertKataAna(Akun.username, soal, kalimat);
                conn.InsertScoreAna(Akun.username, score);
                Score.totalScore = score;
                SceneManager.LoadScene("Over");
                print("Posted");
            }

            else
            {
                score = score + 2000;
                conn.InsertKataAna(Akun.username, soal, kalimat);
                conn.InsertScoreAna(Akun.username, score);
                Levels.JLevel++;
                TimerCount = 25;
                SceneManager.LoadScene("Anagram");
            }


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
            score = score + 100;
            BlockButton[0].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[1].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[2].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[3].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[4].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[5].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[6].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[7].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[8].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[9].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[10].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
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
            score = score + 100;
            BlockButton[11].interactable = false;
        }

        else
        {
            LifeRemember = LifeRemember - 1;
            StartCoroutine("Delays");

        }

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
}
