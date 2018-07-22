using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Versi : MonoBehaviour
{

    public Text ulang;
    public GameObject input;
    public Text round;
    public Text mark;


    // Use this for initialization
    private void Awake()
    {

        Screen.SetResolution(600, 1024, true);
    }

    void Start()
    {
        round.text = "Round : " + Collection._count;
    }

    // Update is called once per frame
    void Update()
    {

        if (Collection._count > 1)
        {
            input.SetActive(false);
        }

    }

    public void Flx1() //method button flx1
    {
        Collection.IsGame2 = false;
        Collection.IsGame1 = true;

        if (Collection._count == 1)
        {
            try
            {
                int u = System.Int32.Parse(ulang.text);
                if (u < 0)
                    u = u * -1;

                Collection.ulang = u;
                SceneManager.LoadScene("Flexible");

            }
            catch (System.Exception Ex)
            {

                mark.text = Ex.Message;

            }
        }

        else if (Collection._count > 1)
        {
            SceneManager.LoadScene("Flexible");
        }
    }

    public void Flx2() //method button flx2
    {
        Collection.IsGame1 = false;
        Collection.IsGame2 = true;
        if (Collection._count == 1)
        {
            try
            {
                int u = System.Int32.Parse(ulang.text);
                if (u < 0)
                    u = u * -1;

                Collection.ulang = u;
                SceneManager.LoadScene("Flexible");
            }
            catch (System.Exception Ex)
            {

                mark.text = Ex.Message;

            }
        }

        else if (Collection._count > 1)
        {
            SceneManager.LoadScene("Flexible");
        }
    }
}
