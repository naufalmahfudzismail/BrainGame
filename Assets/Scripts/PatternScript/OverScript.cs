using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverScript : MonoBehaviour {

    public Text score;
    public Button send;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        score.text = Score.totalScore.ToString();
		
	}

    public void Send()
    {

    }
}
