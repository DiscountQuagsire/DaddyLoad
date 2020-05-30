using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviourPunCallbacks
{
    public Animator animator;

    public float flightSpeed;
    public float runSpeed;
    public float maxFallSpeed;
    public float maxRiseSpeed;
    public float thrustForce;
    public float gravity;
    private float horizontalMove;

    private bool isJumping = false;
    private bool isTurnedRight = false;

    private Rigidbody2D rb;

    private void Start()
    {
        if (!photonView.IsMine) return;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKeyDown("g")) GetComponent<BoxCollider2D>().enabled = !GetComponent<BoxCollider2D>().enabled;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetBool("IsJumping", isJumping);

        if (horizontalMove > 0 && !isTurnedRight)
        {
            isTurnedRight = !isTurnedRight;
            Flip();
        }

        if (horizontalMove<0 && isTurnedRight)
        {
            isTurnedRight = !isTurnedRight;
            Flip();
        }

    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        transform.position += new Vector3(horizontalMove, 0, 0);

        isJumping = Input.GetKey("w");

        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
        if (Input.GetKey("w")) rb.AddForce(new Vector3(0, thrustForce * rb.mass, 0));

        if (rb.velocity.y < -maxFallSpeed) rb.velocity = new Vector3(rb.velocity.x, -maxFallSpeed, 0);
        if (rb.velocity.y > maxRiseSpeed) rb.velocity = new Vector3(rb.velocity.x, maxRiseSpeed, 0);

        if (Input.GetKey("w")) rb.AddForce(new Vector3(0, thrustForce * rb.mass, 0));


    }

    private void Flip()
    {
   
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
