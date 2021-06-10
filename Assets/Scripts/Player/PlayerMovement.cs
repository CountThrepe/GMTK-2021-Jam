using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;

    private Vector2 move;
    private Rigidbody2D rb;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = move;
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>() * speed;
    }
}
