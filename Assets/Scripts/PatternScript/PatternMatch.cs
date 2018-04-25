using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score
{
    public static int totalScore = 0;
}

public class Tile
{
    public GameObject tileObject;
    public string Type;
    public static int Round = 1;

    public Tile(GameObject tileObject, string Type)
    {
        this.tileObject = tileObject;
        this.Type = Type;
    }

    public string getType()
    {
        return this.Type;
    }

    public GameObject getTileObject()
    {
        return this.tileObject;
    }
}



public class PatternMatch : MonoBehaviour
{
    private Connection conn;
    private bool isPaused = false;
    public GameObject Connection;

    GameObject tile1 = null;
    GameObject tile2 = null;

    Ball ball1;
    Ball ball2;
    Ball ball3;


    public int TargetJumlahBola;
    public float WaktuBatas;
    public int BatasSkorPerLevel = 0;
    private int TValue = 0;
    private int skor;
    private float timer = 0;
    private float TimerCount = 15;

    public List<GameObject> tile;
    public Text score;
    public Text TotalScore;
    public Text countDown;
    public Text round;
    public Text Target;

    List<GameObject> tileBank = new List<GameObject>();

    static int baris = 8;
    static int kolom = 6;

    Tile[,] tiles = new Tile[kolom, baris];

    private bool renewBoard = false;
    private bool acak = false;

    public Text error;
    public Text Progress;
    public GameObject canvasError;


    private void Awake()
    {
        Screen.SetResolution(600, 1024, true);
        conn = Connection.GetComponent<Connection>();
    }

    void Start()
    {

        TimerCount = WaktuBatas;


        int jumlah = (baris * kolom) / 3;

        for (int i = 0; i < jumlah; i++)
        {

            for (int j = 0; j < tile.Count; j++)
            {

                GameObject obj = (GameObject)Instantiate(tile[j], new Vector3(-10, -10, 0), tile[j].transform.rotation);
                obj.SetActive(false);
                tileBank.Add(obj);

            }

            Shuffle();

        }


        for (int b = 0; b < baris; b++)
        {

            for (int k = 0; k < kolom; k++)
            {
                Vector3 tilePos = new Vector3(k, b, 0);

                for (int n = 0; n < tileBank.Count; n++)
                {

                    GameObject obj = tileBank[n];

                    if (!obj.activeSelf)
                    {
                        obj.transform.position = new Vector3(tilePos.x, tilePos.y, tilePos.z);
                        obj.SetActive(true);
                        tiles[k, b] = new Tile(obj, obj.name);
                        n = tileBank.Count + 1;

                    }

                }
            }
        }

    }

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

        CheckGrid();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 1000);

            if (hit)
            {
                Debug.Log("hit");
                tile1 = hit.collider.gameObject;

            }
        }
        else if (Input.GetMouseButtonUp(0) && tile1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 1000);

            if (hit)
            {
                tile2 = hit.collider.gameObject;
            }

            if (tile1 && tile2)
            {

                int horzDist = (int)Mathf.Abs(tile1.transform.position.x - tile2.transform.position.x);
                int vertDist = (int)Mathf.Abs(tile1.transform.position.y - tile2.transform.position.y);

                // condition limit swap vertical or horizontal 1 block
                if (horzDist == 1 ^ vertDist == 1)
                {

                    //swap object position and turn back if condition is false while di seret
                    Tile temp = tiles[(int)tile1.transform.position.x, (int)tile1.transform.position.y];
                    tiles[(int)tile1.transform.position.x, (int)tile1.transform.position.y] = tiles[(int)tile2.transform.position.x, (int)tile2.transform.position.y];
                    tiles[(int)tile2.transform.position.x, (int)tile2.transform.position.y] = temp;

                    Vector3 tempPos = tile1.transform.position;
                    tile1.transform.position = tile2.transform.position;
                    tile2.transform.position = tempPos;

                    //reset touched ball

                    tile1 = null;
                    tile2 = null;
                }
                else
                {

                    //terpilih = false;

                }



            }

        }

        score.text = skor.ToString();
        Target.text = "Target Minimal per level : " + TargetJumlahBola;
        TotalScore.text = Score.totalScore.ToString();
        round.text = "Round : " + Tile.Round;

        string minutes = Mathf.Floor(TimerCount / 60).ToString("00");
        string seconds = Mathf.Floor(TimerCount % 60).ToString("00");

        countDown.text = minutes + ":" + seconds;



        Timer();

        Over();

        if (TimerCount == 0 && skor >= BatasSkorPerLevel)
        {
            skor = 0;
        }



    }

    private void Shuffle()
    //random cloning object
    {
        System.Random rand = new System.Random();
        int r = tileBank.Count;
        while (r > 1)
        {

            r--;
            int n = rand.Next(r + 1);
            GameObject val = tileBank[n];
            tileBank[n] = tileBank[r];
            tileBank[r] = val;


        }

    }

    private void clearObject()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Ball") as GameObject[];
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].SetActive(false);
        }


    }

    private void reShuffle()
    {
        if (acak)
        {

            clearObject();
            // clone object in border
            int jumlah = (baris * kolom) / 3;

            for (int i = 0; i < jumlah; i++)
            {

                for (int j = 0; j < tile.Count; j++)
                {

                    GameObject obj = (GameObject)Instantiate(tile[j], new Vector3(-10, -10, 0), tile[j].transform.rotation);
                    obj.SetActive(false);
                    tileBank.Add(obj);

                }

                Shuffle();

            }

            for (int b = 0; b < baris; b++)
            {

                for (int k = 0; k < kolom; k++)
                {
                    Vector3 tilePos = new Vector3(k, b, 0);

                    for (int n = 0; n < tileBank.Count; n++)
                    {

                        GameObject obj = tileBank[n];

                        if (!obj.activeSelf)
                        {

                            obj.transform.position = new Vector3(tilePos.x, tilePos.y, tilePos.z);
                            obj.SetActive(true);
                            tiles[k, b] = new Tile(obj, obj.name);
                            n = tileBank.Count + 1;
                        }

                    }
                }
            }

            Debug.Log("ReShuffle!");
            acak = false;
            TimerCount = WaktuBatas;
            skor = 0;
            Tile.Round++;

        }
    }


    private void Timer()
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
                countDown.text = minutes + ":" + seconds;
            }

            else
            {

                acak = true;
                reShuffle();
            }

        }
    }


    // Update is called once per frame


    private void CheckGrid()
    {
        int counter = 1;
        //check in columns
        for (int b = 0; b < baris; b++)
        {

            counter = 1;

            for (int k = 1; k < kolom; k++)
            {
                if (tiles[k, b] != null && tiles[k - 1, b] != null)
                {
                    //if tiles exist
                    if (tiles[k, b].Type == tiles[k - 1, b].Type)
                    {

                        counter++;
                    }
                    else
                    {
                        //reset counter
                        counter = 1;
                    }
                    // if there are found, calculate the value
                    if (counter == 3 && Input.GetMouseButtonDown(1))
                    {

                        //tercapai = true;
                        if (tiles[k, b] != null)
                            ball1 = tiles[k, b].tileObject.GetComponent<Ball>();
                        if (tiles[k - 1, b] != null)
                            ball2 = tiles[k - 1, b].tileObject.GetComponent<Ball>();
                        if (tiles[k - 2, b] != null)
                            ball3 = tiles[k - 2, b].tileObject.GetComponent<Ball>();
                        Debug.Log("Tercapai");
                        TValue = ball1.getValue() + ball2.getValue() + ball3.getValue();

                        // if their total value is reached target, remove them
                        if (TargetJumlahBola <= TValue)
                        {

                            if (tiles[k, b] != null)
                                tiles[k, b].tileObject.SetActive(false);
                            if (tiles[k - 1, b] != null)
                                tiles[k - 1, b].tileObject.SetActive(false);
                            if (tiles[k - 2, b] != null)
                                tiles[k - 2, b].tileObject.SetActive(false);
                            tiles[k, b] = null;
                            tiles[k - 1, b] = null;
                            tiles[k - 2, b] = null;
                            skor += 1;
                            Score.totalScore += 50;
                            renewBoard = true;
                        }
                    }

                }

            }
        }

        //check in rows
        for (int k = 0; k < kolom; k++)
        {

            counter = 1;

            for (int b = 1; b < baris; b++)
            {

                if (tiles[k, b] != null && tiles[k, b - 1] != null)
                {
                    //if tiles exist

                    if (tiles[k, b].Type == tiles[k, b - 1].Type)
                    {

                        counter++;
                    }

                    else
                    {
                        counter = 1;
                    }
                    // if there are found, calculate the value
                    if (counter == 3 && Input.GetMouseButtonDown(1))
                    {
                        if (tiles[k, b] != null)
                            ball1 = tiles[k, b].tileObject.GetComponent<Ball>();
                        if (tiles[k, b - 1] != null)
                            ball2 = tiles[k, b - 1].tileObject.GetComponent<Ball>();
                        if (tiles[k, b - 2] != null)
                            ball3 = tiles[k, b - 2].tileObject.GetComponent<Ball>();
                        Debug.Log("Tercapai");
                        TValue = ball1.getValue() + ball2.getValue() + ball3.getValue();

                        // if their total value is reached target, remove them
                        if (TargetJumlahBola <= TValue)
                        {
                            if (tiles[k, b] != null)
                                tiles[k, b].tileObject.SetActive(false);
                            if (tiles[k, b - 1] != null)
                                tiles[k, b - 1].tileObject.SetActive(false);
                            if (tiles[k, b - 2] != null)
                                tiles[k, b - 2].tileObject.SetActive(false);
                            tiles[k, b] = null;
                            tiles[k, b - 1] = null;
                            tiles[k, b - 2] = null;
                            skor += 1;
                            Score.totalScore += 50;
                            renewBoard = true;
                        }
                    }
                }
            }
        }

        if (renewBoard)
        {

            newGrid();
            renewBoard = false;
        }
    }

    private void newGrid()
    {
        bool moved = false;
        Shuffle();


        for (int b = 1; b < baris; b++)
        {

            for (int k = 0; k < kolom; k++)
            {

                if (b == baris - 1 && tiles[k, b] == null)
                {

                    Vector3 tilePos = new Vector3(k, b, 0);

                    for (int i = 0; i < tileBank.Count; i++)
                    {

                        GameObject obj = tileBank[i];

                        if (!obj.activeSelf)
                        {

                            obj.transform.position = new Vector3(tilePos.x, tilePos.y, tilePos.z);
                            obj.SetActive(true);
                            tiles[k, b] = new Tile(obj, obj.name);
                            i = tileBank.Count + 1;
                        }
                    }
                }

                if (tiles[k, b] != null)
                {

                    //dropDown if below is empty

                    if (tiles[k, b - 1] == null)
                    {

                        tiles[k, b - 1] = tiles[k, b];
                        tiles[k, b - 1].tileObject.transform.position = new Vector3(k, b - 1, 0);
                        tiles[k, b] = null;
                        moved = true;
                    }
                }
            }

            if (moved)
            {

                Invoke("newGrid", 0.5f);
            }
        }
    }

    private void Over()
    {
        if (TimerCount == 0 && skor < BatasSkorPerLevel)
        {
            conn.InsertScore3Col(Akun.username, Score.totalScore);
            SceneManager.LoadScene("Over");
        }

    }



}

