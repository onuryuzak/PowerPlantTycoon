using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;
using TMPro;
using DG.Tweening;

public class TutorialSystem : BaseSingleton<TutorialSystem>
{
    public TextMeshProUGUI[] tutorialText;

    public List<GameObject> targets;
    public int textIndex = 0;
    public int targetIndex = 0;
    public bool tutorial;
    public int targetCount;
    public int textCount;

    private void Start()
    {
        tutorial = InventoryManager.instance._inventorySO.tutorialIsDone;

        if (!tutorial)
        {
            tutorialText[0].gameObject.SetActive(true);
            targets[0].AddComponent<Target>();
            for (int i = 1; i < tutorialText.Length; i++)
            {
                tutorialText[i].gameObject.SetActive(false);

            }
            DOVirtual.DelayedCall(0.2f, () =>
            {
                for (int i = 1; i < targets.Count; i++)
                {
                    targets[i].AddComponent<Target>();
                    targets[i].GetComponent<Target>().enabled = false;
                }
                targetCount = targets.Count;
            });
        }
        textCount = tutorialText.Length;


    }
    public void UpgradeTargetIndex()
    {
        if (targetIndex < targets.Count)
        {
            targetIndex++;
            ChangeTarget();
        }

        else
        {
            InventoryManager.instance._inventorySO.tutorialIsDone = true;
            Database.instance.saveGame();
            targets[targets.Count - 1].GetComponent<Target>().enabled = false;
        }
    }
    public void UpgradeTextIndex()
    {
        if (!InventoryManager.instance._inventorySO.tutorialIsDone)
        {
            if (textIndex < tutorialText.Length)
            {
                textIndex++;
                ChangeText();
            }
        }
        else
        {
            InventoryManager.instance._inventorySO.tutorialIsDone = true;
            Database.instance.saveGame();
            tutorialText[tutorialText.Length-1].gameObject.SetActive(false);
        }
    }

    private void ChangeText()
    {
        tutorialText[textIndex - 1].gameObject.SetActive(false);
        if (textIndex < tutorialText.Length)
            tutorialText[textIndex].gameObject.SetActive(true);
        else
        {
            UpgradeTextIndex();
        }
    }
    private void ChangeTarget()
    {
        targets[targetIndex - 1].GetComponent<Target>().enabled = false;
        if (targetIndex < targets.Count)
            targets[targetIndex].GetComponent<Target>().enabled = true;
        else
        {
            UpgradeTargetIndex();
        }
    }

}
