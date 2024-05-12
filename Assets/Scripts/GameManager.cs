using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]  public List<Teemo> teemos;

    [SerializeField]  public GameObject playButton;
     [SerializeField]  public GameObject gameUi;
      [SerializeField]  public GameObject timeOut;

       [SerializeField]  private TMPro.TextMeshProUGUI timeText;
       [SerializeField]  private TMPro.TextMeshProUGUI scoreText;
         [SerializeField]  private TMPro.TextMeshProUGUI hongoText;

    public float startingTime = 30f;

     public float timeRemaining;
     public HashSet<Teemo> currentTeemos = new HashSet<Teemo>();
     public int score;
    public bool playing = false;

    public void StartGame()
    {
        playButton.SetActive(false);
        timeOut.SetActive(false);
        gameUi.SetActive(true);
        for (int i = 0; i < teemos.Count; i++)
        {
            teemos[i].Hide();
            teemos[i].SetIndex(i);
        }

        currentTeemos.Clear();
        timeRemaining = startingTime;
        score = 0;
        scoreText.text = "0";
        playing = true;
    }

    public void GameOver(int type)
    {
        if (type == 0)
        {
            timeOut.SetActive(true);
        }

        else

        {
         hongoText.SetActive(true);
        }
        playing = false;
        playButton.SetActive(true);

          foreach (Teemo teemo in teemos)
         {
             teemo.StopGame();
         }

        playing = false;
        playButton.SetActive(true);
    }

     public void AddScore(int teemoIndex)
    {
            score += 1;
            scoreText.text = $"{score}";
            timeRemaining += 1;
            currentTeemos.Remove(teemos[teemoIndex]);
    }

     public void Missed(int teemoIndex, bool isTeemo)
     {       
        if (isTeemo)
            {
                timeRemaining -= 2;
            }

            currentTeemos.Remove(teemos[teemoIndex]);
     }

     public void Update()
    {
        if(playing)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    GameOver(0);
                }
        timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:02}";
        if (currentTeemos.Count <= (score / 10))
                {
                    int index = Random.Range(0, teemos.Count);
                    if (!currentTeemos.Contains(teemos[index]))
                    {
                        currentTeemos.Add(teemos[index]);
                        teemos[index].Activate(score / 10);
                    }
                }
        }
    }

}
