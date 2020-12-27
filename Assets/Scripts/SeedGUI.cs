using UnityEngine;

public class SeedGUI : MonoBehaviour
{
    private Settings settings = null;
    
    void Start()
    {
        settings = GameObject.Find("GameManager").GetComponent<Settings>();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 20), "Seed: " + settings.seed.ToString());
    }
}
