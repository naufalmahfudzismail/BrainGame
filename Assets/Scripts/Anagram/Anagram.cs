using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Anagram : MonoBehaviour {

	static System.Random rand =  new System.Random();
	public string soal;
	public int desiredLength;

	public Text[] txtObj;

	private List<char> alphabet		   =  new List
	private List<char> kar			   =  new List<char> ();
	private List<char> distract        =  new List<char> (){'!', '@', '#', '$', '%', '&', '?', '<', '>', '{', '}', ']', '[', '|', '/', '~', '+', '^'};
	private List<char> desiredDistract =  new List<char> ();
	private List<char> Generate        =  new List<char> ();
	private List<char> FinalGenerate   =  new List<char> ();



	// Use this for initialization
	void Start () {
		ListMixer ();
		StartCoroutine ("PlayGame");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	static void RandomList(int [] array) // random prevent repeating index
	{
		int arraySize = array.Length;
		int random;
		int temp;

		for (int i = 0; i < arraySize; i++) {

			random = i + (int)(rand.NextDouble () * (arraySize - i));
			temp = array [random];
			array [random] = array [i];
			array [i] = temp;
		}
	}

	private void split()// split string soal into char, and store it to list
	{
		foreach (char c in soal) {
			kar.Add (c);
		}
	}

	private void Distraction() // add desired distraction
	{
		split ();
		for (int r = 0; r < desiredLength; r++) {
			desiredDistract.Add (distract [(int)Random.Range (0, distract.Count)]);
		}
	}

	private void  Store() // store char soal and distract into list
	{
		Distraction ();
		int sumChar = (int)kar.Count;
		int sumDist = (int)desiredDistract.Count;
		int GenerLength = sumChar + sumDist;

		for (int i = 0; i < sumChar; i++) {
			Generate.Add (kar [i]);
		}

		for (int j = 0; j < sumDist; j++) {
			Generate.Add (desiredDistract [j]);
		}
	}

	private void ListMixer() //mix the added list into random output
	{
		Store ();
		int GenerLength = Generate.Count;
		int[] randomArray = new int[GenerLength];

		for (int i = 0; i < GenerLength; i++) {
			randomArray [i] = i;
		}

		RandomList (randomArray);

		for (int r = 0; r < GenerLength; r++) {

			FinalGenerate.Add(Generate [randomArray[r]]);
			Debug.Log (FinalGenerate[r]);
		}
	
	}

	IEnumerator PlayGame()
	{
		foreach (char r in FinalGenerate) {
			
			int s = (int)Random.Range (0, txtObj.Length);

			txtObj [s].text = r.ToString ();

			yield return new WaitForSeconds (3f);

			txtObj [s].text = "";

		}
	}
}
