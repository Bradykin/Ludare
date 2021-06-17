using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLevelUpdate : MonoBehaviour
{
    [SerializeField] private Text currentScore;

    // Update is called once per frame
    void Update()
    {
        currentScore.text = LevelController.Instance.GetCurrentScore().ToString();
    }
}
