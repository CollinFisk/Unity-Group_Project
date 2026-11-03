using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 WorldPos = Input.mousePosition;
        transform.LookAt(WorldPos);
    }

    
}
