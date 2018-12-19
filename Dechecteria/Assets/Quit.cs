using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{

	public void OnQuitButtonClick()
    {
        Debug.Log("Button Quit clicked.");
        Application.Quit();
    }
}
