using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public GameObject options;
    public GameObject menu;

    private void Start()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }

    public void OptionsMenu()
    {
        options.SetActive(true);
        menu.SetActive(false);
    }

    public void BackToMenu()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }

}
