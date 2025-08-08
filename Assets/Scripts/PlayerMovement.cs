using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerspeed = 10;
    public float horizontalspeed = 10;
    public float rightlimit = 5.5f;
    public float leftlimit = -5.5f;

    public float speedIncreaseRate = 0.01f; // units per second

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * playerspeed, Space.World);
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (this.gameObject.transform.position.x > leftlimit)
            {
                transform.Translate(Vector3.left * Time.deltaTime * horizontalspeed);
            }

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (this.gameObject.transform.position.x < rightlimit)
            {
                transform.Translate(Vector3.left * Time.deltaTime * horizontalspeed * -1);
            }

        }
        // Clamp x-position to stay within limits
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, leftlimit, rightlimit);
        transform.position = clampedPos;
        playerspeed += speedIncreaseRate * Time.deltaTime;
        horizontalspeed += speedIncreaseRate * Time.deltaTime;
    }
}
