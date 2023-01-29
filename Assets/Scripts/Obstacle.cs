using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] ParticleSystem [] explosions;
    [SerializeField] Renderer [] renders;
    [SerializeField] AudioClip explosionAudio;

    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            PlayerController.instance.Hit();
            if(PlayerController.instance.alive) {
                GetComponent<Collider>().enabled = false;
                foreach(Renderer renderer in renders) renderer.enabled = false;
                foreach (ParticleSystem explosion in explosions) explosion.Play();
                audioSource.PlayOneShot(explosionAudio);
                if(PlayerController.instance.alive) Destroy(this.gameObject, 1f);
            }
        }
    }
}
