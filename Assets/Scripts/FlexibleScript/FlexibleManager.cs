﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FlexibleManager : MonoBehaviour {

	public int maxSoal;
	public Text[] huruf;


	public List<GameObject> floor = new List<GameObject> ();



	List<char> alphabet = new List<char>(){'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','Y','Z'};
	List<char> hurufMuncul = new List<char> ();
	char[] tampungan = new char[20];


	// Use this for initialization
	void Start () {

		if (maxSoal > 20) {
			maxSoal = 20;
		}

		setSoal ();

		Debug.Log ("========================================");

		for (int i = 0; i < hurufMuncul.Count; i++) {
			Debug.Log (hurufMuncul [i]);
		}



		
	}
	
	// Update is called once per frame
	void Update () {

			

		}
		


	private void setSoal(){
		for (int i = 0; i < maxSoal; i++) {
			tampungan [i] = alphabet [Random.Range (0, alphabet.Count)];
			Debug.Log (tampungan [i]);
		}
			
		char[] tampung = tampungan.Distinct ().ToArray();

		Debug.Log (tampung.Length);

		Debug.Log ("========================================");

		for (int j = 0; j < tampung.Length; j++) {

			hurufMuncul.Add (tampung [j]);
			Debug.Log (hurufMuncul [j]);
		}


		//hurufMuncul = hurufMuncul.Distinct();
			
	}
		


	IEnumerator wordAntre()
	{
		yield return new WaitForSeconds (3f);

		while (true) {
			int r = (int)Random.Range (0, huruf.Length);
			for (int hm = 0; hm < hurufMuncul.Count; hm++) {
				
				huruf [r].text = hurufMuncul [hm].ToString ();
				Debug.Log ("Text : " + hurufMuncul [hm] + " Berada di " + huruf [r]);

			}

			huruf [r].text = "";
		}
	}

	/*private void randomBlock()
	{
		for (int r = 0; r < huruf.Length - 1; r++) {

			Text temp = null;
			temp = huruf [r];
			huruf [r] = huruf [Random.Range (0, huruf.Length - 1)];

		}
	}*/
		
		
}
