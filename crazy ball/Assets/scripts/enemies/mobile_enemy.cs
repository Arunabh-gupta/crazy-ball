using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobile_enemy : MonoBehaviour
{
    [SerializeField]
    private float health = healths.enemyHealth;
    [SerializeField]
    private GameObject target_player;
    [SerializeField]
    private Transform shootpoint;
    [SerializeField]
    private float looking_Radius = 3f;
    [SerializeField]
    private GameObject enemy_bullet_prefab;
    [SerializeField]
    private float enemy_bullet_speed;
    [SerializeField]
    private float enemy_rotation_speed = 250f;
    [SerializeField]
    private float enemy_movement_speed = 20f;
    //for time delay between each shot of bullet
    private float timestamp = 0.0f;
    private float DelayPerShot;
    //for time delay between each shot of bullet
    Vector2 target_dir;

    private void Start()
    {   // Using Gun_container class to give the values of respective variables
        enemy_bullet_speed = Gun_Container.enemyGun.GetSpeed();
        DelayPerShot = Gun_Container.enemyGun.GetFireRate();
       // Using Gun_container class to give the values of respective variables
    }
    private void Update()
    {

        target_dir = target_player.transform.position - transform.position;
        if (target_dir.magnitude <= looking_Radius && Time.time > timestamp)
        {
            enemy_firing();

        }

        lookAtandMoveTowards(target_dir);


    }

    // to look at the player gameObject ans move towards it
    private void lookAtandMoveTowards(Vector2 target_dir)
    {
        float target_angle = Mathf.Atan2(target_dir.y, target_dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, target_angle), enemy_rotation_speed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, target_player.transform.position, enemy_movement_speed * Time.deltaTime);
        // Debug.Log(transform.position);

    }
    // to look at the player gameObject ans move towards it

    // enemy bullet firing
    private void enemy_firing()
    {
        timestamp = DelayPerShot + Time.time;
        GameObject canon_bullet = Instantiate(enemy_bullet_prefab, (Vector2)shootpoint.position, Quaternion.identity);
        canon_bullet.GetComponent<canon_bullet>().damage = Gun_Container.enemyGun.GetDamage();// Giving damage to the bullet from the Gun_container class
        Vector2 enemy_bullet_direction = target_dir;
        enemy_bullet_direction.Normalize();
        canon_bullet.GetComponent<Rigidbody2D>().velocity = enemy_bullet_direction * enemy_bullet_speed;
    }
    // enemy bullet firing

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "player bullet")
        {
            health -= other.gameObject.GetComponent<bullet>().damage;
            Debug.Log("Mobile enemy's health " + health);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
