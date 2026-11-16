using UnityEngine;

public class TestObject : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log("Feeling Very Disabled Today :(");
    }
    private void OnEnable()
    {
        Debug.Log("Feeling Very Enabled Today :)");

    }
}
