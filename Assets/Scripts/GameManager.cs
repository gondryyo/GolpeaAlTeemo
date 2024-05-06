using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Teemo> teemos;

    [SerializeField] private GameObject playButton;

    private float startingTime = 30f;

    private float timeRemaining;
    private HashSet<Teemo> currentTeemos = new HashSet<Teemo>();
    private int score;
    private bool playing = false;

    public void StartGame()
    {
        playButton.SetActive(false);
        for (int i = 0; i < teemos.Count; i++)
        {
            teemos[i].Hide();
        }

        currentTeemos.Clear();
        timeRemaining = startingTime;
        score = 0;
        playing = true;
    }

    private void Update()
    {
        
    }

}
