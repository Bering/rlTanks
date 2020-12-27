using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    int offsetX = 300;
    int offsetY = 300;

    public void SetXY(int x, int y)
    {
        offsetX = x;
        offsetY = y;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(offsetX + 10, offsetY + 152, 215, 25), "Back to OS")) {
            #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
