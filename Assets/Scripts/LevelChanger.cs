using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    public Animator anim;
    

    public void PlayGame()
    {
        StartCoroutine(LoadGame());
        
    }

    IEnumerator LoadGame()
    {
        anim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
        anim.SetTrigger("FadeIn");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
