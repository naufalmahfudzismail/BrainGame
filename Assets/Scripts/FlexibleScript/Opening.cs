using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Opening : MonoBehaviour
{

    // Use this for initialization

    public Text error;
    public Text Progress;
    [HideInInspector] public bool isPaused = false;
    public GameObject canvasError;
    public Text Title;

    void Start()
    {

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

    public void Login()
    {
        if (Title.text == "Flexible N-Back")
            SceneManager.LoadScene("Flexible");
        if (Title.text == "Anagram")
            SceneManager.LoadScene("Anagram");
    }

    public void Regist()
    {
        SceneManager.LoadScene("Register");
    }

    public void Daftar()
    {

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
