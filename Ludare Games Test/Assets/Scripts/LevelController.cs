using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    private int highestScore = 0;
    private int currentScore = 0;

    private List<IReset> levelResettables = new List<IReset>();

    public void SubscribeResettable(IReset resettable)
    {
        levelResettables.Add(resettable);
    }

    public void UnsubscribeResettable(IReset resettable)
    {
        levelResettables.Remove(resettable);
    }

    public void ResetAll()
    {
        for (int i = 0; i < levelResettables.Count; i++)
        {
            levelResettables[i].Reset();
        }
    }

    public int GetHighestScore()
    {
        return highestScore;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void GainScore(int scoreGained)
    {
        currentScore += scoreGained;
    }

    public void OnRunLost()
    {
        currentScore = 0;
    }

    public void OnRunEnd()
    {
        if (currentScore > highestScore)
        {
            highestScore = currentScore;
        }
        currentScore = 0;
    }
}