using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Daftar : MonoBehaviour
{

    [HideInInspector] public Text error;
    [HideInInspector] public Text Progress;
    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public GameObject canvasError;
    [HideInInspector] public GameObject conn;
    [HideInInspector] public Text user;
    [HideInInspector] public Text pass;
    [HideInInspector] public Text nama;
    [HideInInspector] public Text kerja;
    [HideInInspector] public Text umur;
    [HideInInspector] public Text mark;
    private Connection con;
    string url;

    void Start()
    {
       
        con = conn.GetComponent<Connection>();
        url = con.UrlRegist;
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

    }

    public void Regist()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", user.text);
        form.AddField("password", pass.text);
        form.AddField("nama", nama.text);
        form.AddField("kerja", kerja.text);
        form.AddField("umur", umur.text);
        WWW www = new WWW(url, form);
        yield return www;
        string data = www.text;
        print(data);

        if (data == "Username dan Password Kosong!" || data == "Username sudah dipakai!" || data == "Password Kosong!" || data == "Username Kosong!")
        {
            mark.text = data;
        }
        else
        {
            mark.text = "Sukses daftar !, silhkan login kembali";

            if (Titles.isFlexible)
                SceneManager.LoadScene("Login");
            else if (Titles.isAnagram)
                SceneManager.LoadScene("Login_Anagram");
        }
    }


}
