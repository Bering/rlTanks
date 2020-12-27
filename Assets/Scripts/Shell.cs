using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] float impulse = 4000;

    public void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * impulse);
        Destroy(gameObject, 15);
    }
}
