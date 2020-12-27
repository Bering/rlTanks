using UnityEngine;
using Mirror;

public class MenuToggler : MonoBehaviour
{
    public bool visible = true;

    NetworkManagerHUD hud = null;
    GameObject mainMenu = null;

    void Start()
    {
        hud = GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>();
        mainMenu = GameObject.Find("MainMenu");
        ShowHide(visible);
    }

    void Update()
    {
        if (Input.GetKeyUp("escape")) {
            Toggle();
        }
    }

    public void Toggle()
    {
      ShowHide(!visible);
    }

    public void ShowHide(bool v)
    {
        visible = v;
        hud.showGUI = visible;
        mainMenu.SetActive(visible);
    }
}
