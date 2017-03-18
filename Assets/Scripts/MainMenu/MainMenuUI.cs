using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuUI : MonoBehaviour {

	public void StartGame(){
		SceneManager.LoadScene ("main");

	}
	public void ExitGame(){
		Application.Quit();	
	}


}
