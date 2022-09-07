using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour

{
    public void OnClickRestart()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene("MainGame");
    }
}
