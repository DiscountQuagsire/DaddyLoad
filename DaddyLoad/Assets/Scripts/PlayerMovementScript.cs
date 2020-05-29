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


        if (Input.GetKey("w"))
        {
            transform.position += new Vector3(0, flightSpeed, 0);
            isJumping = true;
        }

        if (!Input.GetKey("w"))
        {
            isJumping = false;
        }
        if (Input.GetKey("s")) transform.position += new Vector3(0, -0.1f, 0);

    }

    private void Flip()
    {
   
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
