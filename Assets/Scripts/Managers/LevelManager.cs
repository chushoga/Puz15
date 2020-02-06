using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void testMe(string name)
    {
       
        LoadScene(name);
    }
}
