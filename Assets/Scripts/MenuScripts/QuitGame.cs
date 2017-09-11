using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        // If in unity
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }
}
