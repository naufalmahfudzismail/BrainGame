﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverScript : MonoBehaviour {

    public Text score;
    public Button send;

    // Use this for initialization
    private void Awake()
    {
        Screen.SetResolution(600, 1024, true);
    }

    void Start () {

        score.text = Score.totalScore.ToString();
    }
	
	// Update is called once per frame
	void Update () {

        
		
	}

    public void Send()
    {
        Application.Quit();
    }
}
