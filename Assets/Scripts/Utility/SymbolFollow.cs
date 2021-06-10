using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolFollow : MonoBehaviour
{
    public Transform player, finalSpot;
    public float factor = 0.01f;
    public Vector2 offset;
    private int follow;
    void OnEnable()
    {
        follow = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(follow == 1)
        {
            Vector2 targetPos = player.position;
            targetPos.y += offset.y;
            targetPos.x += offset.x * player.localScale.x;

            Vector2 diff = targetPos - (Vector2) transform.position;
            transform.position = (Vector2) transform.position + (diff * factor);
        }
        else if(follow == 2)
        {
            Vector2 diff = finalSpot.position - transform.position;
            transform.position = (Vector2) transform.position + (diff * factor);
        }
    }

    public void StartFollow()
    {
        follow = 1;
    }

    public void FinalSpot()
    {
        follow = 2;
    }
}
