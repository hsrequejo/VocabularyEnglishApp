using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextAsset word_en_full;
    public TextAsset word_pt_full;
    public string[] words_en;
    public string[] words_pt;
    public AudioClip[] audios;
    public string[] alternatives;
    public int index;
    public string word;
    public string translation;
    public AudioClip audioClip;
    public Button audioButton;
    public TextMeshProUGUI TMP_word;
    public AudioSource wordAudioSource;

    public TextMeshProUGUI a1;
    public TextMeshProUGUI a2;
    public TextMeshProUGUI a3;
    public TextMeshProUGUI a4;

    public Button b1;
    public Button b2;
    public Button b3;
    public Button b4;

    public TextMeshProUGUI correctWarning;
    public TextMeshProUGUI wrongWarning;

    public Canvas gameCanvas;

    public GameObject countdownPanel;
    public int countdownTimer;
    public TextMeshProUGUI countdownDisplay;

    public bool gameStarted = false;
    public TextMeshProUGUI timerDisplay;
    public float timeRemaining;

    public int score;
    public TextMeshProUGUI scoreDisplay;

    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI highScore;

    public GameObject timeIsUpPanel;

    public int maxTime;

    public bool timerOn;

    int correctAnswerIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = 30f;
        score = 0;
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted && timerOn)
        {
            //start the timer
            if (timeRemaining >= 0)
            {
                timerDisplay.SetText("Time Left: {0:0}s", timeRemaining);
                timeRemaining -= Time.deltaTime ;
            }
            else if (timeRemaining <= 0)
            {
                GameOver();
            }
        }
    }

    public void FillAlternatives()
    {
        int rnd;
        int correct_index;
        correct_index = Random.Range(0, 4);
        
        for (int i = 0; i < 4; i++)
        {
            rnd = Random.Range(0, 1000);
            alternatives[i] = words_pt[rnd];
        }
        alternatives[correct_index] = translation;
    }

    public void FillButtonsAnswers()
    {
        a1.text = alternatives[0];
        a2.text = alternatives[1];
        a3.text = alternatives[2];
        a4.text = alternatives[3];
    }

    public void ResetBoard()
    {
        alternatives = new string[4];
        words_en = word_en_full.text.Split('\n');
        words_pt = word_pt_full.text.Split('\n');
        audios = Resources.LoadAll<AudioClip>("mp3");
        index = Random.Range(0, 1000);
        word = words_en[index];
        translation = words_pt[index];
        audioClip = audios[index];
        TMP_word.text = word;
        wordAudioSource = audioButton.GetComponent<AudioSource>();
        wordAudioSource.clip = audioClip;
        wordAudioSource.PlayOneShot(audioClip);
        FillAlternatives();
        FillButtonsAnswers();
        b1.GetComponent<Image>().color = Color.white;
        b2.GetComponent<Image>().color = Color.white;
        b3.GetComponent<Image>().color = Color.white;
        b4.GetComponent<Image>().color = Color.white;
    }

    public void CheckAnswer(TextMeshProUGUI buttonText)
    {
        TextMeshProUGUI[] allAnswers = new TextMeshProUGUI[4];
        allAnswers[0] = a1;
        allAnswers[1] = a2;
        allAnswers[2] = a3;
        allAnswers[3] = a4;
        for (int i = 0; i < 4; i++)
        {
            if (allAnswers[i].text == translation)
            {
                allAnswers[i].GetComponentInParent<Image>().color = Color.green;
                break;
            }
        }
        if (buttonText.text == translation)
        {
            DisableInteraction();
            correctWarning.gameObject.SetActive(true);
            score ++;
            scoreDisplay.SetText("Score: {0:0}", score);
            StartCoroutine(DelayDisableWarning(correctWarning));
        }
        else
        {
            DisableInteraction();
            wrongWarning.gameObject.SetActive(true);
            buttonText.GetComponentInParent<Image>().color = Color.red;
            StartCoroutine(DelayDisableWarning(wrongWarning));
        }
    }

    IEnumerator DelayDisableWarning(TextMeshProUGUI warning)
    {

        yield return new WaitForSeconds(1f);

        warning.gameObject.SetActive(false);
        ResetBoard();
        EnableInteraction();
    }

    public void buttonPressed1()
    {
        CheckAnswer(a1);
    }
    public void buttonPressed2()
    {
        CheckAnswer(a2);
    }
    public void buttonPressed3()
    {
        CheckAnswer(a3);
    }
    public void buttonPressed4()
    {
        CheckAnswer(a4);
    }

    public void DisableInteraction()
    {
        b1.interactable = false;
        b2.interactable = false;
        b3.interactable = false;
        b4.interactable = false;
    }

    public void EnableInteraction()
    {
        b1.interactable = true;
        b2.interactable = true;
        b3.interactable = true;
        b4.interactable = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartCountDown()
    {
        timeRemaining = maxTime;
        gameOverPanel.SetActive(false);
        countdownPanel.gameObject.SetActive(true);
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
        countdownPanel.gameObject.SetActive(false);
        ResetBoard();
        gameStarted = true;
    }

    public void GameOver()
    {
        gameStarted = false;
        timeIsUpPanel.SetActive(true);
        StartCoroutine(TimeIsUp());
        gameOverPanel.gameObject.SetActive(true);
        finalScore.SetText("Final Score: {0:0}", score);
        highScore.SetText("High Score: ***");
    }
    IEnumerator TimeIsUp()
    {
        
        yield return new WaitForSeconds(1f);
        timeIsUpPanel.SetActive(false);
    }

    public void disableTimer()
    {
        timerOn = false;
        timerDisplay.gameObject.SetActive(false);
    }

    public void enableTimer()
    {
        timerOn = true;
        timerDisplay.gameObject.SetActive(true);
    }
}
