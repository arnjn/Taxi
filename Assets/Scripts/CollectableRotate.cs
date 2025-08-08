using UnityEngine;

public class CollectableRotate : MonoBehaviour
{
    [SerializeField] int rotatespeed = 1;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotatespeed, 0, Space.World);
    }
}
