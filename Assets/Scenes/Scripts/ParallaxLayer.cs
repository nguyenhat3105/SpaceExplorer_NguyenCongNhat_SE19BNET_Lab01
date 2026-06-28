using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float speed = 1f;

    private float height;
    private Vector3 startPosition;
    private GameObject cloneLayer;

    void Start()
    {
        startPosition = transform.position;
        height = GetComponent<SpriteRenderer>().bounds.size.y;

        cloneLayer = Instantiate(gameObject, transform.parent);
        Destroy(cloneLayer.GetComponent<ParallaxLayer>());
        cloneLayer.transform.position = new Vector3(startPosition.x, startPosition.y + height, startPosition.z);
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        cloneLayer.transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        if (transform.position.y < startPosition.y - height)
        {
            transform.position = new Vector3(startPosition.x, cloneLayer.transform.position.y + height, startPosition.z);
        }

        if (cloneLayer.transform.position.y < startPosition.y - height)
        {
            cloneLayer.transform.position = new Vector3(startPosition.x, transform.position.y + height, startPosition.z);
        }
    }
}
