using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;

    private float length;
    private float startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);
    }
}
