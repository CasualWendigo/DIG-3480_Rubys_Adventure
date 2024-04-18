using System.Collections;
using System.Collections.Generic;
using UnityEngine;

﻿public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float patrolTime = 3.0f;
    public int maxHitPoints = 3;
    private int currentHitPoints;

    public ParticleSystem smokeEffect;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;
    
    Animator animator;

    AudioSource audioSource;
    public AudioClip robotFixed;
    public AudioClip metalHit1;
    public AudioClip metalHit2;


    private RubyController rubyController;
    
    void Start()
    {
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby
        audioSource = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rubyControllerObject != null)
            rubyController = rubyControllerObject.GetComponent<RubyController>(); //and this line of code finds the rubyController and then stores it in a variable
        timer = patrolTime;
        currentHitPoints = maxHitPoints;
    }

    void Update()
    {
        if(!broken)
            return;
        
        timer -= Time.deltaTime;
        if (timer < 0) {
            direction = -direction;
            timer = patrolTime; }
    }
    
    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
            return;
        
        Vector2 position = rigidbody2D.position;
        
        if (vertical) {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction); }
        else {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0); }
        
        rigidbody2D.MovePosition(position);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
            player.ChangeHealth(-1);
    }
    
    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {   
        currentHitPoints -= 1;

        if (currentHitPoints <= 0) {
            audioSource.PlayOneShot(robotFixed);
            animator.SetBool("Broken", false);
            rigidbody2D.simulated = false;
            smokeEffect.Stop();
            broken = false;
            rubyController.ChangeScore(1);
            return;
        }

        if (Random.value >= 0.5)
            audioSource.PlayOneShot(metalHit1);
        else
            audioSource.PlayOneShot(metalHit2);


    }
}