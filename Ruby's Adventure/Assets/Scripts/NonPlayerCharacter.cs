using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject giveOutQuest;
    public GameObject completeQuest;
    float timerDisplay;
    private bool acknowledgedQuestDone = false;
    public AudioClip questCompleted;

    private RubyController controller;
    
    void Start()
    {
        timerDisplay = -1.0f;
    }
    
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                giveOutQuest.SetActive(false);
                completeQuest.SetActive(false);
            }
        }
    }
    
    public void DisplayDialog()
    {
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController");
        controller = rubyControllerObject.GetComponent<RubyController>();

        if (controller.score >= controller.maxScore) {
            completeQuest.SetActive(true);
            if (acknowledgedQuestDone == false) {
                acknowledgedQuestDone = true;
                controller.fixRobotQuestDone = true;
                controller.PlaySound(questCompleted); } }
        if (controller.score < controller.maxScore)
            giveOutQuest.SetActive(true);

        timerDisplay = displayTime;
    }
}
