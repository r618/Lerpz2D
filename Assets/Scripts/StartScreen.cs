using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200), "Welcome to the Unity Lerpz Level!!");
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 15, 200, 30), "Play"))
        {
            SceneManager.LoadScene("Level_1");
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 65, 200, 30), "Exit"))
        {
            Application.Quit();
        }
    }
}
