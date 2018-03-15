using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Flexible : MonoBehaviour {

	static System.Random rand  = new System.Random();

	public int Jsoal;

    public GameObject CharButton;
    public GameObject PostButton;

	private int tSameChar;
	private int tSameFloor;
    private int tGetChar;
    private int tGetFloor;

	private int Repetition = 5;


	//private double CharPercentage = (tGetChar / tSameChar) * 100;
	//private double FloorPercentage = (tGetFloor / tSameChar) * 100;

	private bool CharisMatched = false;
	private bool FloorisMatched = false;

	public Text[] floor = new Text[9];
    public Text charScore;
    public Text postScore;

	private int totalC;

	private string alphabet = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
	private List <char> alphabets = new List <char> ();


	List <char> randomedChar  =  new List <char> ();
	List <char> sameChar      =  new List <char> ();
	List <char> sameClone 	  =  new List <char> ();
	List <char> difChar       =  new List <char> ();

	List <char> finalChar     =  new List <char> ();
	List <Text> tFl  		  =  new List <Text> ();


	// Use this for initialization
	void Start () {

		//randomingChar ();

		GatherChar ();
		RandomingPosition ();
		StartCoroutine ("SetTask");
		CheckDouble ();
	}
	
	// Update is called once per frame
	void Update () {

        charScore.text = tGetChar.ToString();
        postScore.text = tGetFloor.ToString();
		
	}

	private void randomingChar()
	{
		foreach (char c in  alphabet) {
			alphabets.Add (c);
		}

		int jAlpha = alphabets.Count;

		for (int i = 0; i < Jsoal; i++) {

			int Random = i + (int)(rand.NextDouble () * ( jAlpha - i));
			randomedChar.Add (alphabets [Random]);
			//Debug.Log (randomedChar [i]);
		}

		int dCount = randomedChar.Count;

		for (int i = 0; i < dCount; i++) {

			sameChar.Add (randomedChar [i]);
			difChar.Add (randomedChar [i + 1]);

			i++;
		}
			

		int SCount = sameChar.Count;

		for (int i = 0; i < SCount ; i++) {
			
			sameClone.Add (sameChar [i]);
			sameClone.Add (sameChar [i]);
		}

		for (int i = 0; i < sameClone.Count; i++) {
			Debug.Log (sameClone[i]);
		}


	}
		
	private void GatherChar()
	{
		randomingChar ();

		totalC = (sameClone.Count + difChar.Count);
		//int sparTC = totalC / 3;

		for (int i = 0; i < totalC ; i++) {

			int select = (int)Random.Range (0, 2);
			int rand1 = (int)Random.Range (0, sameClone.Count);
			int rand2 = (int)Random.Range (0, difChar.Count);

			if (select == 0) {


				if (rand1 == 0 ) 
				{

					finalChar.Add (sameClone [rand1]);
				} 
				else
				{
					finalChar.Add (sameClone [rand1]);
					finalChar.Add (sameClone [rand1 - 1]);

					i++;
				}

				Debug.Log ("Add same");
			}

			if (select == 1) {

				finalChar.Add (difChar [rand2]);
				Debug.Log ("Add dif");
			}
		}

		Debug.Log ("========================");

		for (int i = 0; i < finalChar.Count; i++) {
			Debug.Log (finalChar[i]);
		}
			
	}


	private void RandomingPosition()
	{
		Text temp;

		int fc = floor.Length;
		int tfC = Jsoal + (Jsoal / 2);
		Text [] sameFloor = new Text[Jsoal];
		Text [] difFloor  = new Text[Jsoal / 2];


		for (int i = 0; i < fc; i++) {

			int random = i + (int)(rand.NextDouble() * (fc - i));

			temp = floor [random];
			floor [random] = floor [i];
			floor [i] = temp;

		} // shuffle floor

		for (int i = 0; i < Jsoal; i++) {

			int random = (int)Random.Range (0, fc);

			sameFloor [i] = floor [random];
			sameFloor [i + 1] = floor [random];

			i++;
		} 

		for (int i = 0; i < Jsoal / 2; i++) {

			int random = (int)Random.Range (0, fc);
			difFloor [i] = floor [random];
		}

		for (int i = 0; i < tfC; i++) {

			int random = (int)Random.Range (0, 2);
			int rand1 = (int)Random.Range (0, sameFloor.Length);
			int rand2 = (int)Random.Range (0, difFloor.Length);

			if (random == 0) {

				tFl.Add (sameFloor [rand1]);
			}

			if (random == 1) {

				tFl.Add (difFloor [rand2]);

			}
		}

		for (int i = 0; i < tfC; i++) {

			Debug.Log (tFl.ToString ());
		}
			
	}


	IEnumerator SetTask()
	{
		int JtS = Jsoal + (Jsoal / 2);
		
		for (int i = 0; i < JtS; i++) {

			tFl [i].text = finalChar [i].ToString ();

			if (i > 0) {
				if (finalChar [i] == finalChar [i - 1]) {

					CharisMatched = true;

				}
				if (tFl [i] == tFl [i - 1]) {

					FloorisMatched = true;

				} 
			}

			yield return new WaitForSeconds (2f);

			CharisMatched  = false;
			FloorisMatched = false;

			tFl [i].text = "";

            CharButton.SetActive(true);
            PostButton.SetActive(true);

		}
	}

	private void CheckDouble()
	{
		int TotalChar = finalChar.Count;
		int TotalFloor = tFl.Count;

		if (TotalChar == TotalFloor) {

			for (int i = 0; i < TotalChar - 1; i++) {
				
				if (finalChar [i] == finalChar [i + 1]) {

					tSameChar = tSameChar + 1;
				}

			}
				
			Debug.Log (tSameChar);

			for (int i = 0; i < TotalFloor - 1; i++) {

				if (tFl [i] == tFl [i + 1]) {

					tSameFloor = tSameFloor + 1;
				}
			}

			Debug.Log (tSameFloor);
				
		}
			
	}

	public void ClickHuruf()
	{
		if (CharisMatched) {

			tGetChar = tGetChar + 1;
		}

        CharButton.SetActive(false);

	}
	public void ClickPosisi()
	{
		if (FloorisMatched) {

			tGetFloor = tGetFloor + 1;
		}

        PostButton.SetActive(false);

	}

	/*
	 * WHAT TO DO NOW:
	 * REPLAY THE SCENE ABOUT 5 TIMES
	 * TAMPUNG SCORE TIAP REPLAY DALAM BENTUK STATIS
	 * BUILD 2 SCENE GAMEPLAY, SCOREBOARD
	*/
		

}
