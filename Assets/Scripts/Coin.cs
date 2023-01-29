using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int score;

    [SerializeField] AudioClip coinAudio;


    void Update() {
            transform.Rotate(0, 80*Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            GlobalUI.instance.AddScore(score);
            GlobalUI.instance.PlaySound(coinAudio);
            Destroy(this.gameObject);
        }
    }
}
