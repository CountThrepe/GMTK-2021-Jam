using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_Speed = 5f;
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
    private bool canJump = true;
	private bool frozen = false;
    private int cantJumpCounter = 0;

	public Animator animator;
	private SpriteRenderer spr;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		spr = GetComponent<SpriteRenderer>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
				{
					OnLandEvent.Invoke();
				}
			}
		}

        if(!canJump) {
            cantJumpCounter++;
            if(cantJumpCounter > 10 || !m_Grounded) canJump = true;
        }
	}


	public void Move(float move, bool jump)
	{
		if(frozen) return;
		
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * m_Speed, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump && canJump)
		{ 
			// Add a vertical force to the player.
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            canJump = false;
            cantJumpCounter = 0;
		}

		if(!m_Grounded)
		{
			animator.SetBool("notGrounded", true);
		}
		//Tell the animator if we're moving horizontally
		animator.SetFloat("Speed", Mathf.Abs(move));
		//Tell the animator our vertical speed
		animator.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
	}

	public void Celebrate() {
		frozen = true;
		m_Rigidbody2D.velocity = Vector2.zero;
		m_Rigidbody2D.isKinematic = true;
		// Play celebration animation
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void onLanding()
	{
		animator.SetBool("notGrounded", false);
	}

	public void frontToBack()
	{
		this.spr.sortingLayerName = "Back";
	}

	public void backToFront()
	{
		this.spr.sortingLayerName = "Front";
	}
}
