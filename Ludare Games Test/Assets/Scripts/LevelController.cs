using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
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
}