using Economy;
using GeneralsMiniGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public AnimalType type;
    public Animator anim;
    public GameObject floatingPoints;
    public AudioClip clip;
    public float velocity;
    public Rigidbody2D rb;

    private Vector2 direction;
    private bool dead;

    private void Start()
    {
        if (type == AnimalType.chicken || type == AnimalType.duck)
            StartCoroutine(ChickenMovement());
    }

    private void OnMouseDown()
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        if (type == AnimalType.chicken)
        {
            GetComponent<Collider2D>().enabled = false;
            dead = true;
            PointsManager.instance.AddPoints(1);
            AnimalSpawner.Instance.RemoveSpecificAnimal(gameObject);
            anim.SetTrigger("Dead");
            Instantiate(floatingPoints, transform.position, Quaternion.identity);
            Destroy(gameObject, 3);
        }
        else
        {
            AttemptsCounter.Instance.AddAttempt();
            AnimalSpawner.Instance.RemoveSpecificAnimal(gameObject);
            Destroy(gameObject);
        }
    }

    IEnumerator ChickenMovement()
    {
        direction = new Vector2((Random.Range(-1f, 1f)), (Random.Range(-1f, 1f)));
        while (!dead)
        {
            rb.velocity = direction * velocity * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = new Vector2((Random.Range(0, 2) * velocity * Time.deltaTime), (Random.Range(0, 2) * velocity * Time.deltaTime));
    }
}

public enum AnimalType { distracter, chicken, duck }