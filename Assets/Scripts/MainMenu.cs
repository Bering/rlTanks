using UnityEngine;
using Mirror;

public class MainMenu : MonoBehaviour
{
    int offsetX;
    int offsetY;
    NetworkManagerHUD hud = null;
    OptionsMenu optionsMenu = null;
    ExitMenu exitMenu = null;

    void Start()
    {
        hud = GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>();
        optionsMenu = GameObject.Find("MainMenu").GetComponent<OptionsMenu>();
        exitMenu = GameObject.Find("MainMenu").GetComponent<ExitMenu>();
    }

    void Update()
    {
        offsetX = Screen.width / 2 - 130;
        offsetY = Screen.height / 2 - 130;
        hud.offsetX = offsetX;
        hud.offsetY = offsetY;
        optionsMenu.SetXY(offsetX, offsetY);
        exitMenu.SetXY(offsetX, offsetY);
    }
}
