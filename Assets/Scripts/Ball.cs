using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {


	private int value;
	public GameObject nomor;


	// Use this for initialization
	void Start () {

		value = Random.Range (0, 10);
		TextMesh txtNomor = nomor.GetComponent<TextMesh> ();
		txtNomor.text = value.ToString ();
		Debug.Log (value);

	}
	
	// Update is called once per frame
	void Update () {


		
	}

	public int getValue()
	{
		return this.value;
	}
		
		
}
