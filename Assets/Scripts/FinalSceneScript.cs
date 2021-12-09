using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FinalSceneScript : MonoBehaviour
{
   public void _ResetGame()
    {
        PlayerPrefs.SetInt("LEVELS", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
