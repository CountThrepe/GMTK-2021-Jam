using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float targetXOffset = 4;
    public float xFactor = 0.1f;

    public float yUpperBound = 0f;
    public float yLowerBound = -2f;
    public float yAbsoluteBound = 3.5f;
    public float yFactor = 0.04f;

    public float minX = -18;

    // Update is called once per frame
    void FixedUpdate()
    {
        float targetX = player.position.x + (targetXOffset * player.localScale.x);
        float xOffset = (targetX - transform.position.x) * xFactor;
        Vector3 pos = transform.position + new Vector3(xOffset, GetOffset(transform.position.y, player.position.y, yLowerBound, yUpperBound, yAbsoluteBound, yFactor), 0);
        if(pos.x < minX) pos.x = minX;
        transform.position = pos;
    }

    private float GetOffset(float pos, float targetPos, float lowerBound, float upperBound, float absBound, float factor)
    {
        float dist = targetPos - pos;
        float offset = 0;
        if(dist > absBound) offset = dist - absBound;
        else if(dist < -absBound) offset = dist + absBound;
        else if(dist > upperBound) offset = (dist - upperBound) * factor;
        else if(dist < lowerBound) offset = (dist + lowerBound) * factor;

        return offset;
    }
}
