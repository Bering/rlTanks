using UnityEngine;

public class Follow : MonoBehaviour
{
    GameObject target = null;
    Vector3 offset;

    void Start()
    {
        offset = new Vector3(-62, -86, 62);
    }
    
    public void SetTarget(GameObject t)
    {
        target = t;
    }

    void Update()
    {
        if (target == null) return;

        transform.position = target.transform.position - offset;
    }
}
