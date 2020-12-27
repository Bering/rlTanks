using UnityEngine;
using Mirror;

public class OptionsMenu : MonoBehaviour
{
    int offsetX;
    int offsetY;
    Settings settings = null;
    string[] controlsOrientations = new string[] { "Isometric", "Screen" };

    void Start()
    {
        settings = GameObject.Find("GameManager").GetComponent<Settings>();
    }

    public void SetXY(int x, int y)
    {
        offsetX = x;
        offsetY = y;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(offsetX + 15, offsetY + 122, 100, 20), "Controls:");
        settings.controlsOrientation = GUI.SelectionGrid(new Rect(offsetX + 75, offsetY + 120, 150, 25), settings.controlsOrientation, controlsOrientations, 2);
    }
}
