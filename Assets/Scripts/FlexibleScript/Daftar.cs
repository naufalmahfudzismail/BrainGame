using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Daftar : MonoBehaviour
{

    public Text error;
    public Text Progress;
    public bool isPaused = false;
    public GameObject canvasError;
    public GameObject conn;
    public Text user;
    public Text pass;
    public Text nama;
    public Text kerja;
    public Text umur;
    public Text mark;
    private Connection con;
    string url;

    void Start()
    {
        print(Titles.title);
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

            yield return new WaitForSeconds(5f);

            if (Titles.title == "Flexible N-back")
                SceneManager.LoadScene("Login");
            if (Titles.title == "Anagram")
                SceneManager.LoadScene("Login_Anagram");
        }
    }


}
