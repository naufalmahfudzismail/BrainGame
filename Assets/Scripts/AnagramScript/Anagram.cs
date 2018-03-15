using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class StringExtensions
{
	public static bool ContainsWord(this string word, string otherword)
	{
		int currentIndex = 0;

		foreach(var character in otherword)
		{
			if ((currentIndex = word.IndexOf(character, currentIndex)) == -1)
				return false;
		}

		return true;
	}
}

public class Anagram : MonoBehaviour {


	static System.Random rand =  new System.Random();

	private string soal;

	private int LifePoint = 5;


	private bool isPlay = false;
	private bool isDone = false;
	private bool isMatch = false;

	public int JumlahString;
	public int desiredLength;

	public GameObject canvas1;
	public GameObject canvas2;
	public GameObject ReplayButton;
    public GameObject txtSentence;
    

	public Text[] txtObj;
	public Text Hp;
	public Text Result;
	public Text Clue;

	public InputField txtField;
    public InputField txtSentences;

	private string[] itemsData;

	private List<string> kamus 		   =  new List<string>();
	private List<char> kar			   =  new List<char> ();
	private List<char> distract        =  new List<char> (){'!', '@', '#', '$', '%', '&', '?', '<', '>', '{', '}', ']', '[', '|', '/', '~', '+', '^'};
	private List<char> desiredDistract =  new List<char> ();
	private List<char> Generate        =  new List<char> ();
	private List<char> FinalGenerate   =  new List<char> ();



	// Use this for initialization

	IEnumerator Start () {
		
		WWW items = new WWW("http://localhost/BrainGameDB/items.php");

		yield return items;

		string itemsString = items.text;

		itemsData = itemsString.Split('|');

		for (int i = 0; i < itemsData.Length; i++) {

			kamus.Add (itemsData [i]);
		}

		print (kamus.Count);

		GetSoal(kamus);

		ListMixer ();

		StartCoroutine ("PlayGame");



	}
		
	
	// Update is called once per frame
	void Update () {

		
        Hp.text = LifePoint.ToString();

		if (isDone && !isPlay) {

			canvas1.SetActive (false);
			canvas2.SetActive (true);
		}

		if (!isDone && isPlay) {

			isDone = false;
			isPlay = false;
			SceneManager.LoadScene ("Anagram");
		}
		
	}


	private string GetSoal(List<string> soalRand)
	{
		int count = soalRand.Count;
		int random = Random.Range (0, count);

		string Soalrandom = soalRand [random];

		while (Soalrandom.Length != JumlahString) {

			random = Random.Range (0, count);
			Soalrandom = soalRand [random];
		}

		soal = (string) Soalrandom;

		soal = soal.ToUpper ();

		print (soal);

		return soal;

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

			yield return new WaitForSeconds (2f);

			txtObj [s].text = "";

		}
		isDone = true;
	}
		

	public void CheckJawaban()
	{
		string Jawaban = txtField.text;
		string Soal = soal;

		if (Jawaban == Soal) 
		{
			Result.text = "Selamat, Jawaban anda benar!";
			//Score.text = GET.scoreAnagram.ToString();
			txtSentence.SetActive(true);
		}
		else 
		{
			Result.text = "Maaf, Jawaban anda Salah!";
			LifePoint --;
		}

		if (LifePoint == 0) {

			Result.text = "Maaf, Anda Gagal!";
			ReplayButton.SetActive (true);
		}

	
	}

	public void ReplayCLick()
	{
		isDone = false;
		isPlay = true;
	}



	/*
	 * WHAT TO DO NOW:
	 * BUILD REPLAY BUTTON (done)
	 * BUILD TIMER (done)
	 * BUILD TEXT FIELD POST TO DATABASE
	 * BUILD CHAR CONTAIN RESULT
	 * TURN ON / OFF CANVAS (done)
	 * DETERMINE HP TEXT (done)
	 * DETERMINE SCORE TEXT INTO SCORE STATIS (done)
	*/
}
