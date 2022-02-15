using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Void : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneManager.GetActiveScene().name == "LevelOne")
        {
            SceneManager.LoadScene("LevelOne");

        }
        if (SceneManager.GetActiveScene().name == "LevelTwo")
        {
            SceneManager.LoadScene("LevelTwo");
        }
        if (SceneManager.GetActiveScene().name == "LevelThree")
        {
            SceneManager.LoadScene("LevelThree");
        }
    }
}
