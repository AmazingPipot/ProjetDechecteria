using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScript : MonoBehaviour
{

    public Animator Animator;

	public void OnButtonClick()
    {
        Animator.SetTrigger("Close");
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1.1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Introduction");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
