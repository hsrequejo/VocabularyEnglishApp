using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContDownToStart : MonoBehaviour
{
    public int countdownTimer;
    public TextMeshProUGUI countdownDisplay;
    public GameObject CountdownPanel;


    public void StartCountDown()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        countdownTimer = 3;
        while (countdownTimer > 0)
        {
            countdownDisplay.text = countdownTimer.ToString();
            yield return new WaitForSeconds(0.5f);
            countdownTimer--;
        }

        countdownDisplay.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        CountdownPanel.gameObject.SetActive(false);

    }
}
