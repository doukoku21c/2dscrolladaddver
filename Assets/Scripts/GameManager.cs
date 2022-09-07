using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartBtn;

    // �ڷ� �ι� ���� ��ư��
    int ClickCount = 0;
    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();

        {
     // �ڷει� ����
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClickCount++;
                if (!IsInvoking("DoubleClick"))
                    Invoke("DoubleClick", 1.0f);

            }
            else if (ClickCount == 2)
            {
                CancelInvoke("DoubleClick");
                Application.Quit();
            }

        }

    }

    
    private InterstitialAd interstitial;
    public Canvas myCanvas;
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
                    string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
                    string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }




    public void NextStage()
    {
        // stage change
        if(stageIndex < Stages.Length-1){
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            player.maxSpeed = 0; 
            PlayerReposition();

            RequestInterstitial();
            //When you want call Interstitial show
            StartCoroutine(showInterstitial());

            IEnumerator showInterstitial()
            {
                while (!this.interstitial.IsLoaded())
                {
                    yield return new WaitForSeconds(0.2f);
                }
                this.interstitial.Show();
                myCanvas.sortingOrder = -1;
            }

        //else
        //{
        //    Time.timeScale = 0;
        //    Debug.Log("���� Ŭ����");
        //    Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
        //    btnText.text = "Game Clear!";
        //    UIRestartBtn.SetActive(true);
        }

        
    }

    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        // Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
        PlayerReposition();
        UIStage.text = "STAGE " + (stageIndex + 1);
        player.VelocityZero();
        player.maxSpeed = 4;
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            // Player Die Effect
            player.OnDie();
            //Result UI
            Debug.Log("�׾����ϴ�");
            UIRestartBtn.SetActive(true);
            //Retry Button UI 

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") { 
        {
            HealthDown(); // hp down -1
            if (health > 1)
                      PlayerReposition();
            }
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-19.14f, 4.1f, -1); // ���� 3.57f����
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainGame");
    }
}
