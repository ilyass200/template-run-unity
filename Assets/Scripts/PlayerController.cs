using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    float speed = 5;
    public int life = 2; 
    [HideInInspector] public bool alive = true;
    [HideInInspector] public bool win;
    Rigidbody rb;
    float horizontalInput;
    [SerializeField] float jumpForce = 200f;
    [SerializeField] LayerMask groundMask;

    [Header("Collider")]
    [SerializeField] Collider normalCollider;
    [SerializeField] Collider dashCollider;

    [Header("Audio")]
    [SerializeField] AudioClip footstepAudio;
    [SerializeField] AudioClip jumpAudio;
    [SerializeField] AudioClip dashAudio;
    [SerializeField] AudioClip winAudio;
    [SerializeField] AudioClip gameOverAudio;

    Animator animator;
    Vector3 startPosition;

    public static PlayerController instance;
    AudioSource audioSource;
    int maxDistance;
    
    void Awake(){
        if(instance != null){ 
            Debug.LogWarning("There is other PlayerController instance in the scene !");
            Destroy(this.gameObject);
            return;
        } else instance = this;
    }
    
    bool isDashing;
    float defaultY;

    bool isGrounded;

    void Start() {
        startPosition = transform.position;
        ChangeDifficulty();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        defaultY = transform.position.y;
    }

    void PlayFooStep(){
        if (!alive || GlobalUI.instance.isPause || win) return;
        if(isDashing) audioSource.PlayOneShot(dashAudio);
        else audioSource.PlayOneShot(footstepAudio);
    }

    void FixedUpdate() {
        if (!alive || GlobalUI.instance.isPause || win) return;
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);
    
    }

    void Update () {
        if (!alive || GlobalUI.instance.isPause || win) return;
        int distance = (int)(Vector3.Distance(startPosition, transform.position));
        if(distance >= maxDistance && GlobalUI.instance.levelMode != 5) Win();
        string distanceString = GlobalUI.instance.levelMode == 5 ? "INFINITE" : maxDistance.ToString();
        GlobalUI.instance.distanceTMP.text = $"DISTANCE: {(distance).ToString()}/{distanceString}";

        Vector3 playerUpperPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        isGrounded = Physics.Raycast(playerUpperPosition, Vector3.down, 1.1f, groundMask);
        animator.SetBool("jumping", !isGrounded);
        animator.SetBool("dashing", isDashing);
        float posX = transform.position.x;
        if((Input.GetKeyDown(KeyCode.LeftArrow) && GlobalUI.instance.isArrowKeyBoard) || (Input.GetKeyDown(KeyCode.Q))&& !GlobalUI.instance.isArrowKeyBoard)
            posX -= 3;
        else if ((Input.GetKeyDown(KeyCode.RightArrow) && GlobalUI.instance.isArrowKeyBoard) || (Input.GetKeyDown(KeyCode.D))&& !GlobalUI.instance.isArrowKeyBoard)
            posX += 3;
        

        if(Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.UpArrow) && GlobalUI.instance.isArrowKeyBoard) || (Input.GetKeyDown(KeyCode.Z))&& !GlobalUI.instance.isArrowKeyBoard) Jump();
        if((Input.GetKeyDown(KeyCode.DownArrow) && GlobalUI.instance.isArrowKeyBoard) || (Input.GetKeyDown(KeyCode.S))&& !GlobalUI.instance.isArrowKeyBoard) DashUnDash();

        if(posX > 3) posX = 3;
        if(posX < -3) posX = -3;
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);


        if (transform.position.y < -5) {
            Die();
        }
	}

    public void ChangeDifficulty(){
        if(GlobalUI.instance.levelMode == 2) {
            speed = 10;
            maxDistance = 1500;
        }
        else if(GlobalUI.instance.levelMode == 3) {
            speed = 13.5f;
            maxDistance = 2500;
        }
        else if(GlobalUI.instance.levelMode == 4) {
            speed = 17.5f;
            maxDistance = 5000;
        }
        else if(GlobalUI.instance.levelMode == 5) {
            speed = 20;
            maxDistance = 1000000;
        }
        else {
            speed = 7.5f;
            maxDistance = 500;
        }
    }

    public void Hit(){
        life -= 1;
        if(life < 0) life = 0;
        GlobalUI.instance.lifeTMP.text = $"LIFE: {life}";
        if(life == 0) Die();
    }

    public void Win(){
        win = true;
        GlobalUI.instance.PlaySound(winAudio);
        animator.SetBool("win", win);
        GlobalUI.instance.winPanel.SetActive(true);
    }

    public void Die ()
    {
        alive = false;
        // transform.position = new Vector3(transform.position.x, defaultY, transform.position.z);
        GlobalUI.instance.PlaySound(gameOverAudio);
        animator.SetBool("dead", true);
        GameData gameData = SaveManager.LoadData();
        gameData.scores.Add(GlobalUI.instance.score);
        SaveManager.SaveData(gameData);
        GlobalUI.instance.gameOverPanel.SetActive(true);
    }

    void Jump(){
        if(!isGrounded) return;
        audioSource.PlayOneShot(jumpAudio);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        ToNormal(true);
    }

    void DashUnDash(){
        isDashing = !isDashing;
        StopAllCoroutines();
        if(isDashing) StartCoroutine(DashCoroutine());
        if(!isGrounded) ToNormal(false);
        else ToNormal(!isDashing);
    }

    IEnumerator DashCoroutine()
    {
        audioSource.PlayOneShot(dashAudio);
        yield return new WaitForSeconds(1.25f);
        isDashing = false;
        ToNormal(true);
    }

    void ToNormal(bool isNormal){
        // bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height/2) + 0.1f, groundMask);
        // if(!isGrounded) {
        //     rb.velocity = Vector3.zero;
        // }
        transform.position = new Vector3(transform.position.x, defaultY, transform.position.z);
        normalCollider.enabled = isNormal;
        dashCollider.enabled = !isNormal;
    }
}
