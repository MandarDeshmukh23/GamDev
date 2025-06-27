using UnityEngine;
using UnityEngine.UIElements;

public class MainBallScirpt : MonoBehaviour
{
    public GameObject stone;
    public float offset;
    bool allballgenerated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && !allballgenerated)
        {
            for (int i = 0; i < 7; i++)
            {
                Instantiate(stone, new Vector2(transform.position.x + i * offset, transform.position.y + i * offset), transform.rotation);
            }
            allballgenerated = true;
        }
    }
}
