using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class GlobalUI : MonoBehaviour
{
    public static GlobalUI instance;
    [SerializeField] TextMeshProUGUI scoreTMP;
    [SerializeField] TextMeshProUGUI infoMenuTMP;
    public TextMeshProUGUI distanceTMP;
    [SerializeField] GameObject mainMenuPanel;

    [SerializeField] Transform bestScoreGrid; 
    [SerializeField] GameObject bestScoreCard; 

    AudioSource audioSource;
    public bool isArrowKeyBoard = true;
    

    public TextMeshProUGUI lifeTMP;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public int levelMode = 1;
    
    [HideInInspector] public int score;
    [HideInInspector] public bool isPause = true;
    
    void Awake(){
        if(instance != null){ 
            Debug.LogWarning("There is other GlobalUI instance in the scene !");
            Destroy(this.gameObject);
            return;
        } else instance = this;
    }

    void Start(){
        GlobalUI.instance.lifeTMP.text = $"LIFE: {PlayerController.instance.life}";
        audioSource = GetComponent<AudioSource>();
        BestScoresUpdate();
    }

    public void PlaySound(AudioClip audioClip) { audioSource.PlayOneShot(audioClip); }

    public void StartGame(){
        mainMenuPanel.SetActive(false);
        isPause = false;
    }

    public void ArrowShortcuts(bool isToggle){
        if(isToggle){
            isArrowKeyBoard = true;
            infoMenuTMP.text = "Arrow keyboard Mode !";
        }
    }

    public void qsdzShortcuts(bool isToggle){
        if(isToggle){
            isArrowKeyBoard = false;
            infoMenuTMP.text = "QSDZ keyboard mode !";
        }
    }


    public void ResetScores(){
        SaveManager.SaveData(new GameData());
        BestScoresUpdate();
        infoMenuTMP.text = "the score has been successfully reset !";

    }

    public void AddScore(int scoreToAdd){
        score += scoreToAdd;
        scoreTMP.text = $"SCORE: {score}";
    }

    public void EasyMode(bool isToggle){ 
        if(isToggle) levelMode = 1; 
        infoMenuTMP.text = "change to Easy mode !";
        PlayerController.instance.ChangeDifficulty();
    }
    
    public void MediumMode(bool isToggle){ 
        if(isToggle) levelMode = 2; 
        infoMenuTMP.text = "change to Medium mode !";
        PlayerController.instance.ChangeDifficulty();
    }
    
    public void HardMode(bool isToggle){ 
        if(isToggle) levelMode = 3; 
        infoMenuTMP.text = "change to Hard mode !";
        PlayerController.instance.ChangeDifficulty();
    }
    
    public void HardeCoreMode(bool isToggle){ 
        if(isToggle) levelMode = 4; 
        infoMenuTMP.text = "change to HardeCore mode !";
        PlayerController.instance.ChangeDifficulty();
    }

    public void InfiniteMode(bool isToggle){ 
        if(isToggle) levelMode = 5; 
        infoMenuTMP.text = "change to Infinite mode !";
        PlayerController.instance.ChangeDifficulty();
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BestScoresUpdate(){
        foreach(Transform childGrid in bestScoreGrid) Destroy(childGrid.transform.gameObject);
        GameData gameData = SaveManager.LoadData();

        int rank = 1;

        var scores = gameData.scores.OrderByDescending(x => x).ToList();


        foreach (int score in scores){
            GameObject bestScoreCardTmp =  GameObject.Instantiate(bestScoreCard, bestScoreGrid.transform, true);
            Vector3 tmpPos = bestScoreCardTmp.GetComponent<RectTransform>().localPosition;
            bestScoreCardTmp.GetComponent<RectTransform>().localPosition = new Vector3(tmpPos.x, tmpPos.y, 0);
            bestScoreCardTmp.GetComponent<RectTransform>().localScale = Vector3.one;
            bestScoreCardTmp.transform.GetComponent<TextMeshProUGUI>().text = $"Top {rank}: {score}";
            rank += 1;
        }
    }
}
