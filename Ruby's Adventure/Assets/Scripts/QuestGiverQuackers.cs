using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiverQuackers : MonoBehaviour
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

        if (controller.holdingQuestItem == true) {
            completeQuest.SetActive(true);
            if (acknowledgedQuestDone == false) {
                acknowledgedQuestDone = true;
                controller.fetchSparePartsDone = true;
                controller.PlaySound(questCompleted); } }
        if (controller.holdingQuestItem == false)
            giveOutQuest.SetActive(true);

        timerDisplay = displayTime;
    }
}
