using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//used for Main Menu, Win Screen, and Death Screen
public class StartScreen : MonoBehaviour
{

    private bool startInput;
    private bool restartInput;

    void Start()
    {
        
    }

    void Update()
    {
        GetInput();
        Play();

    }

    private void GetInput()
    {
        startInput = Input.GetKey(KeyCode.S);
        restartInput = Input.GetKey(KeyCode.R);
    }

    private void Play()
    {
        if (startInput)
        {
            SceneManager.LoadScene("Level 1");
        }

        if (restartInput)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
