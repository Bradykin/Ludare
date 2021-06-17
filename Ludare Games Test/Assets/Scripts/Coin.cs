using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IReset
{
    // Start is called before the first frame update
    void Start()
    {
        LevelController.Instance.SubscribeResettable(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        LevelController.Instance.UnsubscribeResettable(this);
    }

    public void Collect()
    {
        Hide();
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
