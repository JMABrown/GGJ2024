using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button btnStart;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPress(string buttonValue)
    {
        switch(buttonValue)
        {
            case "Start":
                {
                    //load scene
                    SceneManager.LoadScene("Merged Scene");
                    break;
                }
            default:
                {
                    Debug.Log("button error");
                    break;
                }
        }
    }
}
