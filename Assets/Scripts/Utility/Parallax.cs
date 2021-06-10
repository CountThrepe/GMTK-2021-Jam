using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private Vector2 startPos, camStartPos;
    public Transform cam;
    public float parallaxEffect;
    public bool repeat = true;

    void Start()
    {
        startPos = transform.position;
        camStartPos = cam.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 dist = new Vector2((cam.position.x - camStartPos.x) * parallaxEffect, (cam.position.y - camStartPos.y) * parallaxEffect);
        transform.position = startPos + dist;

        if(repeat)
        {
            float relDist = cam.position.x * (1 - parallaxEffect);
            if(relDist > startPos.x + length) startPos.x += length;
            else if(relDist < startPos.x - length) startPos.x -= length;
        }
    }
}
