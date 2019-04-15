using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {

    public Transform[] patrolPoints;
    public float speed;
    public int health;
    Transform currentPatrolPoint;
    int currentPatrolIndex;

    public Transform target;
    public float chaseRange;

    public float attackRange;
    public float damage;
    private float lastAttackTime;
    public float attackDelay;

    public GameObject projectile;
    public float webForce;

    //Jaap's edits
    private Animator animator;
    private GameObject player;
    //end


    // Use this for initialization
    void Start () {
        //More of my edits
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player");
        //end edits
        currentPatrolIndex = 0;
        currentPatrolPoint = patrolPoints[currentPatrolIndex];
		
	}
	
	// Update is called once per frame
	void Update () {

        if (health <= 0) {
            animator.SetTrigger("BossDead");
            Destroy(gameObject, 1);

        }
        transform.Translate(Vector3.down * Time.deltaTime * speed);
        if (Vector3.Distance (transform.position, currentPatrolPoint.position) < .1f) {
            if (currentPatrolIndex + 1 < patrolPoints.Length) {
                currentPatrolIndex++;
            } else {
                currentPatrolIndex = 0;
            }
            currentPatrolPoint = patrolPoints[currentPatrolIndex];
        }
        // Turn to face the current patrol point
        // Finding the direction Vector that points to the patrolpoint
        Vector3 patrolPointDir = currentPatrolPoint.position - transform.position;
        //Figure out the rotation in degrees we need to turn towards
        float angle = Mathf.Atan2(patrolPointDir.y, patrolPointDir.x) * Mathf.Rad2Deg - 90f;
        // Made the rotation that we need to face
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        // Apply the rotation to our transform
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 180f);

        // Chasing Player AI
        // Get the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget < chaseRange) {
            // Start chasing the target
            Vector3 targetDir = target.position - transform.position;
            float angle1 = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
            Quaternion q1 = Quaternion.AngleAxis(angle1, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q1, 180);

            transform.Translate(Vector3.up * Time.deltaTime * speed);

            //Jaap Edit
            //no longer needed animator.SetTrigger("OutofRange");
            //end edit

        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer < attackRange)
        {
            if (Time.time > lastAttackTime + attackDelay)
            {
                target.SendMessage("takeDamage", damage);
                // Record the time we attacked
                lastAttackTime = Time.time;

                //Jaap's edits
                animator.SetTrigger("enemyDetected");
                health -= (int) (player.GetComponent<Stats>().damage);
                animator.SetTrigger("damage");
                // no longer needed player.GetComponent<Stats>().takeDamage(gameObject.GetComponent<Stats>().damage);
                //end
            }
        }
        // check to see if the player is within our attack range
        float distanceToPlayer1 = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer1 < attackRange) {
            // Turn towards the target
            Vector3 targetDir2 = target.position - transform.position;
            float angle2 = Mathf.Atan2(targetDir2.y, targetDir2.x)
                * Mathf.Rad2Deg - 90f;
            Quaternion q2 = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q2, 90 * Time.deltaTime);

            // Check to see if its time to attack
            if (Time.time > lastAttackTime * attackDelay) {
                // Raycast to see if we have the line of sight to the player
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, attackRange);
                // check to see if we hit anything and what it was
                if (hit.transform == target) {
                    // Hit the player - fire the projectile
                    GameObject newWeb = Instantiate(projectile, transform.position, transform.rotation);
                    newWeb.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, webForce));
                    lastAttackTime = Time.time;
                    //Jaap's edit
                    //no longer needed animator.SetTrigger("OutofRange");
                    //end edits. 

                }
            }
        }
	}
}
