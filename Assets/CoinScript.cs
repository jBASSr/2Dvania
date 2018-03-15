using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinScript : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        ScoreTextScript.coinAmount += 1;
        Destroy(gameObject);

    }
}
