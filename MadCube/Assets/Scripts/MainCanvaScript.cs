using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanvaScript : MonoBehaviour
{
    public static MainCanvaScript Instance;
    [SerializeField] private GameObject GameOverPan;
    [SerializeField] private GameObject BestRecordPan;
    [SerializeField] private GameObject SceneInterpolatePan;
    [SerializeField] private GameObject GameCompletedPan;

    [SerializeField] private GameObject PausePan;
    [SerializeField] private Slider mainMusicSlider;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void Start()
    {
        MainEvents.Instance.OnGameOver += SetGameOverPan;
        MainEvents.Instance.OnNewRecord += SetBestRecordPan;
        MainEvents.Instance.OnGameCompleted += SetGameCompletedPan;
        MainEvents.Instance.OnGamePaused += SetPausePan;
        TryGetBestRecord();
        MainMusicSetup();

    }
    void MainMusicSetup()
    {
        // Sahne tekrar baþlarsa
        if (PlayerPrefs.HasKey("Main_Music_Volume"))
        {
            mainMusicSlider.value = PlayerPrefs.GetFloat("Main_Music_Volume",1f);
        }
    }
    private void OnDisable()
    {
        MainEvents.Instance.OnGameOver -= SetGameOverPan;
        MainEvents.Instance.OnNewRecord -= SetBestRecordPan;
        MainEvents.Instance.OnGameCompleted -= SetGameCompletedPan;
        MainEvents.Instance.OnGamePaused -= SetPausePan;
    }
    private void SetGameCompletedPan()
    {
        if (!GameCompletedPan.activeInHierarchy)
        {
            GameCompletedPan.SetActive(true);
        }
        float newBestRecord = PlayerPrefs.GetFloat("Time");
        int minutes = (int)(newBestRecord / 60);
        int seconds = (int)(newBestRecord % 60);
        GameCompletedPan.GetComponentInChildren<Image>().color = Color.black * 0f;
        GameCompletedPan.GetComponentInChildren<Image>().DOColor(Color.black, 1f);
        GameCompletedPan.transform.Find("RecordText").GetComponent<TextMeshProUGUI>().text = $"Best Record: {minutes:00}:{seconds:00}";
    }

    private void TryGetBestRecord()
    {
        float record = PlayerPrefs.GetFloat("Time", 0);
        if (record > 0)
        {
            int minutes = (int)(record / 60);
            int seconds = (int)(record % 60);
            BestRecordPan.GetComponentInChildren<TextMeshProUGUI>().text = $"Best Record: {minutes:00}:{seconds:00}";
        }
    }

    private void SetBestRecordPan()
    {
        float newBestRecord = PlayerPrefs.GetFloat("Time");
        int minutes = (int)(newBestRecord / 60);
        int seconds = (int)(newBestRecord % 60);
        BestRecordPan.GetComponentInChildren<TextMeshProUGUI>().text = $"Best Record: {newBestRecord / 60:00} : {newBestRecord % 60:00}";
        GameCompletedPan.transform.Find("RecordText").GetComponent<TextMeshProUGUI>().text = $"New Record: {minutes:00}:{seconds:00}";
    }

    private void SetGameOverPan()
    {
        if (!GameOverPan.activeInHierarchy)
        {
            GameOverPan.SetActive(true);
        }
        GameOverPan.transform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GameOverPan.transform.DOScale(Vector3.one, 0.5f));
        sequence.Join(GameOverPan.transform.DOShakeRotation(1f, 10f, 50, 90, true).OnComplete(() =>
        {
            GameOverPan.transform.DORotate(Vector3.zero, 0.5f);
        }));
    }

    public void SetSceneInterpolatePan()
    {
        if (!SceneInterpolatePan.activeInHierarchy)
        {
            SceneInterpolatePan.SetActive(true);
        }
        SceneInterpolatePan.GetComponentInChildren<Image>().color = new Color(0, 0, 0, 0);
        SceneInterpolatePan.GetComponentInChildren<Image>().DOColor(new Color(0, 0, 0, 1), 1f).OnComplete(() =>
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });

    }

    public void SetPausePan(bool isOpening)
    {
        if (!isOpening)
        {
            PausePan.SetActive(false);
            return;

        }
        if (!PausePan.activeInHierarchy) PausePan.SetActive(true);
    }

    public void OnMainMusicChanged()
    {
        PlayerPrefs.SetFloat("Main_Music_Volume", mainMusicSlider.value);
        AudioManager.instance.SetVolume();


    }
}
