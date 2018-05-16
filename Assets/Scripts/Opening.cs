using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Titles
{
    public static string title;
    public static bool isFlexible = false;
    public static bool isAnagram = false;
    public static bool isPattern = false;
}


public class Opening : MonoBehaviour
{

    // Use this for initialization
    // erase hide inspector, if u wanna to see the object on inspector
    [HideInInspector] public Text error;
    [HideInInspector] public Text Progress;
    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public GameObject canvasError;
    [HideInInspector] public Text Title;
    [HideInInspector] public Text user;
    [HideInInspector] public Text pass;
    [HideInInspector] public Text mark;
    [HideInInspector] public GameObject conn;
    public InputField inputpass;
    string passw;

    private Connection con;
    string url;

    void Awake()
    {
        Screen.SetResolution(600, 1024, true);
        print(Title.text);
        con = conn.GetComponent<Connection>();
        url = con.UrlLogin;
    }

    // Update is called once per frame
    void Update()
    {
        passw = inputpass.text;

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

    }

    public void Login()
    {

        StartCoroutine(Check_akun());

    }

    public void Regist()
    {
        if (Title.text == "Flexible N-Back")
            Titles.isFlexible = true;
        else if (Title.text == "Anagram")
            Titles.isAnagram = true;
        else if (Title.text == "Pattern Match")
            Titles.isPattern = true;

        print(Titles.isFlexible);
        print(Titles.isAnagram);

        SceneManager.LoadScene("Register");
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

    IEnumerator Check_akun()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", user.text);
        form.AddField("password", passw);
        WWW www = new WWW(url, form);
        yield return www;
        string data = www.text;
        print(data);

        if (data == "Selamat datang")
        {
            Akun.username = user.text;
            Akun.password = pass.text;
            if (Title.text == "Flexible N-Back")
                SceneManager.LoadScene("PreFlexible");
            else if (Title.text == "Anagram")
                SceneManager.LoadScene("Anagram");
            else if (Title.text == "Pattern Match")
                SceneManager.LoadScene("PatternMatch");
        }
        else
        {
            mark.text = data;
        }
    }

}
