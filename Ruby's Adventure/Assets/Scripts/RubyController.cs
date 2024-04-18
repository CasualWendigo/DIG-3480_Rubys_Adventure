using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

﻿public class RubyController : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject gameOverScreen;
    public GameObject youWinScreen;

    public float speed = 3.0f;
    public int maxHealth = 5;
    
    public GameObject projectilePrefab;

    public ParticleSystem hurtEffect;
    public ParticleSystem healEffect;
    
    AudioSource audioSource;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip lossSound;
    
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    private bool gameOver;
    public bool fixRobotQuestDone = false;
    public int score = 0;
    public int maxScore = 4;
    public bool holdingQuestItem = false;
    public bool fetchSparePartsDone = false;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        scoreText.SetText("x 0");
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize(); }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible) {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0) {
                isInvincible = false;
                hurtEffect.Stop(); } }
        
        // Shoot
        if(Input.GetKeyDown(KeyCode.C))
            Launch();
        
        // Interact
        if (Input.GetKeyDown(KeyCode.X)) {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null) {
                NonPlayerCharacter jambi = hit.collider.GetComponent<NonPlayerCharacter>();
                QuestGiverQuackers quackers = hit.collider.GetComponent<QuestGiverQuackers>();
                if (jambi != null) 
                    jambi.DisplayDialog();
                if (quackers != null)
                    quackers.DisplayDialog();
                
                if (fixRobotQuestDone && fetchSparePartsDone)
                    youWinScreen.SetActive(true); } }

        // Restart
        if (Input.GetKey(KeyCode.R)) {
            if (gameOver == true) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); } }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0) { // If losing health...
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetBool("Hit", true);
            hurtEffect.Play();
            
            PlaySound(hitSound); }
        
        if (amount > 0)
            healEffect.Play();

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth <= 0) {
            gameOver = true;
            gameOverScreen.SetActive(true);
            PlaySound(lossSound);
            speed = 0;
        }
    }

    public void ChangeScore(int scoreAmount)
    {
        score = score + scoreAmount;
        scoreText.text = "x " + score.ToString();
    }
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        
        PlaySound(throwSound);
    } 
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}