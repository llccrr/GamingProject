using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Opponent : Killable {

    public enum State {Idle, Chasing, Attacking};
    State currentState;

	NavMeshAgent navSys;
	Transform destination;
    Material playerSkin;

    Color originalOpponentColour;

    float attackDistance = .5f;
    float attackRate = 1;

    float timeUntilNextAttack;

    float playerCollisionRadius;
    float opponentCollisionRadius;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		navSys = GetComponent<NavMeshAgent> ();
        playerSkin = GetComponent<Renderer>().material;
        originalOpponentColour = playerSkin.color;

        currentState = State.Chasing;
        destination = GameObject.FindGameObjectWithTag("Player").transform;

        playerCollisionRadius = destination.GetComponent<CapsuleCollider>().radius;
        opponentCollisionRadius = GetComponent<CapsuleCollider>().radius; 
        StartCoroutine(UpdatePathfinding());


    }
	
	// Update is called once per frame
	void Update () {
        if(Time.time > timeUntilNextAttack)
        {
            //Dont use vector3.Distance cause using too much ressource for thing we dont necerraly need
            float sqrDistance = (destination.position - transform.position).sqrMagnitude;
            if (sqrDistance < Mathf.Pow(attackDistance + playerCollisionRadius + opponentCollisionRadius, 2))
            {
                timeUntilNextAttack = Time.time + attackRate;
                StartCoroutine(Attack());
            }
        }
    
	}
    IEnumerator Attack()
    {
        currentState = State.Attacking;
        navSys.enabled = false;
        
        Vector3 originalPosition = transform.position;
        Vector3 rayToMove = (destination.position - transform.position).normalized;
        Vector3 attackPosition = destination.position - rayToMove * (playerCollisionRadius);

        


        float attackSpeed = 3; 
        float LoadingPercentage = 0;

        playerSkin.color = Color.red;
        while(LoadingPercentage <= 1)
        {
            LoadingPercentage += Time.deltaTime * attackSpeed;
            float parabolic = (-LoadingPercentage * LoadingPercentage + LoadingPercentage) * 4;
           
            //Make the player move from the original position and then once the percentage reached 100% going back to the original position
            transform.position = Vector3.Lerp(originalPosition, attackPosition, parabolic);

            yield return null;
        }
        playerSkin.color = originalOpponentColour;
        currentState = State.Chasing;
        navSys.enabled = true;
    }
        IEnumerator UpdatePathfinding() {
		float timingRefresh = 0.3f;

		while (destination != null)
        {
            if(currentState == State.Chasing)
            {
                Vector3 rayToMove = (destination.position - transform.position).normalized;
                Vector3 destinationPosition = destination.position - rayToMove * (playerCollisionRadius + opponentCollisionRadius + attackDistance/2);
                if (!killed)
                {
                    navSys.SetDestination(destinationPosition);
                }           
            }
            yield return new WaitForSeconds(timingRefresh);
        }
	}
}
