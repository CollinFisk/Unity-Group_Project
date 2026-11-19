using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    private void Update()
    {
        transform.position = GameObject.FindWithTag("Player").transform.position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.SetActive(true);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        other.gameObject.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        other.gameObject.SetActive(false);
    }
}
