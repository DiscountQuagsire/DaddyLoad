using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovementScript : MonoBehaviourPunCallbacks
{
    public Animator animator;

    public float runSpeed;
    public float maxFallSpeed;
    public float maxRiseSpeed;
    public float thrustForce;
    public float gravity;
    private float horizontalMove;
    public float drillDamage;

    public Transform bottom;
    public Transform front;

    private bool isJumping = false;
    private bool isTurnedRight = false;
    private bool isDrilling;

    public LayerMask isGround;

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

        if (!isDrilling) horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (isDrilling) horizontalMove = 0;
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
        if (!isDrilling)
        {
            transform.position += new Vector3(horizontalMove, 0, 0);
            if ((Input.GetKey("a") ^ Input.GetKey("d")) && !isJumping) DrillSide();

            if (Input.GetKey("w")) rb.AddForce(new Vector3(0, thrustForce * rb.mass * 20, 0));
            
        }
      

        if (Input.GetKey("s")) DrillDown();
        if (!Input.GetKey("s"))
        {
            animator.SetBool("IsDrilling", false);
        }

        isJumping = Input.GetKey("w");
        
        rb.AddForce(new Vector3(0, -gravity * rb.mass * 20, 0));
        

        if (rb.velocity.y < -maxFallSpeed) rb.velocity = new Vector3(rb.velocity.x, -maxFallSpeed, 0);
        if (rb.velocity.y > maxRiseSpeed) rb.velocity = new Vector3(rb.velocity.x, maxRiseSpeed, 0);
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    private void DrillDown()
    {
        RaycastHit2D hit = Physics2D.Raycast(bottom.position, new Vector2(0, -1), 0.1f, isGround);

        if (hit.collider != null)
        {
            GameObject block = hit.collider.gameObject;
            animator.SetBool("IsDrilling", true);
            isJumping = false;
            hit.collider.gameObject.GetComponent<Block>().takeDamage(drillDamage, gameObject);
        }
        else
        {
            return;
        }
    }

    private void DrillSide()
    {
        Vector2 direction = front.right;
        if (!isTurnedRight) direction = -direction;
        RaycastHit2D hit = Physics2D.Raycast(front.position, direction, 0.4f, isGround);
        Debug.DrawRay(front.position, direction);
        if (hit.collider != null)
        {
            GameObject block = hit.collider.gameObject;
            hit.collider.gameObject.GetComponent<Block>().takeDamage(drillDamage, gameObject);
        }
        else
        {
            return;
        }
    }

    [PunRPC]
    public void destroyBlock(string playerID, int x, int y)
    {
        GameObject.Find("CommManager").GetComponent<CommunicationScript>()
            .receiveMessage("blockdestroy/" + playerID + "/" + x + "/" + y);
    }
}
