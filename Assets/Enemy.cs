using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;

    public LayerMask esTierra, esJugador;

    public int hp;


    //Animaciones
    public Animation animation;

    //Patrullar 
    public Vector3 walkpoint;
    public Vector3 offset;
    bool walkPointSet;
    public float walkPointRange;


    //atacar
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;


    //States
    public float sightRange, attackRange;
    public bool playerEnRango, playerEnRangoAtaque;

    [SerializeField]
    float fuerzaDisparo;

    private void Awake()
    {

        player = GameObject.Find("Bobby").transform;
        agent = GetComponent<NavMeshAgent>();
        animation = GetComponent<Animation>();

    }



    private void Update()
    {
        playerEnRango = Physics.CheckSphere(transform.position, sightRange, esJugador);
        playerEnRangoAtaque = Physics.CheckSphere(transform.position, attackRange, esJugador);

        if (!playerEnRango && !playerEnRangoAtaque) Patrullar();
        if (playerEnRango && !playerEnRangoAtaque) PerseguirJugador();
        if (playerEnRango && playerEnRangoAtaque) AtacarJugador();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
            Debug.Log("Recibio daño");
        }
    }


    private void Patrullar()
    {
        if(hp>0)
        animation.Play("run");

        if (!walkPointSet&&hp>0) BuscarWalkPoint();


        if (walkPointSet&&hp>0)
            agent.SetDestination(walkpoint);

        Vector3 distanciaParaPunto = transform.position - walkpoint;

        if (distanciaParaPunto.magnitude < 1f)
            walkPointSet = false;
    }

    private void BuscarWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //animation.Play("run");

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if (Physics.Raycast(walkpoint, -transform.up, 2f, esTierra))

            walkPointSet = true;


    }


    private void PerseguirJugador()
    {
        agent.SetDestination(player.position);
    }

    private void AtacarJugador()
    {


        agent.SetDestination(transform.position);

        transform.LookAt(player);

        //Ejecutamos animacion de ataque
        animation.Play("attack3");


        if (!alreadyAttacked)
        {


            Rigidbody rb = Instantiate(projectile, transform.position + offset, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * fuerzaDisparo, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
       
        if (hp <= 0&&animation.IsPlaying("run"))
        {
            animation.Stop("run");
            animation.Play("death");
            Invoke(nameof(DestroyEnemy), 3.0f);
        }
    }


    private void DestroyEnemy()
    {
        
        Destroy(gameObject);

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }


}
