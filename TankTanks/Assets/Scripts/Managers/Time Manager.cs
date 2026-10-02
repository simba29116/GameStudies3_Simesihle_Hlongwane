using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("UI Refrences")]
    [SerializeField] private TextMeshProUGUI currentLapTimerText;
    //[SerializeField] private TextMeshProUGUI overallRaceTimerText;
   // [SerializeField] private TextMeshProUGUI bestLapTimeText;
    //[SerializeField] private TextMeshProUGUI lapText;



    [Header("Race Settings")]
    [SerializeField] private CheckPoint[] checkPoints;
    [SerializeField] private int lastCheckPointIndex = -1;
    [SerializeField] private bool isCircuit = false;
    [SerializeField] private int totalLaps = 1;
    [SerializeField] private int currentLap = 0;

    private bool raceFinished = false;
    private bool raceStarted = false;

    [Header("Lap Timer")]
    private float currentLapTime = 0f;
    // private float overallRaceTime = 0f;
   // private float bestLapTime = Mathf.Infinity;



   // private const string BestLapPrefKey = "BestLapTime";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

       /* if (PlayerPrefs.HasKey(BestLapPrefKey))
        {
           bestLapTime = PlayerPrefs.GetFloat(BestLapPrefKey);
       }

        PlayerPrefs.DeleteKey("BestLapTime");*/
    }




    private void Update()
    {
        if (raceStarted && !raceFinished)
        {
            UpdateTimers();
        }
        UpdateUI();
    }
    public void CheckPointReached(int checkPointIndex)
    {
        if ((!raceStarted && checkPointIndex != 0) || raceFinished)
        {
            return;
        }

        if (checkPointIndex == lastCheckPointIndex + 1)
        {
            //Update Checkpoint
            UpdateCheckpoint(checkPointIndex);


        }

    }

    private void UpdateCheckpoint(int checkPointIndex)
    {



        if (checkPointIndex == 0)
        {
            if (!raceStarted)
            {
                StartRace();
            }
            else if (isCircuit && lastCheckPointIndex == checkPoints.Length - 1)
            {
                OnLapFinished();
            }
        }
        else if (!isCircuit && checkPointIndex == checkPoints.Length - 1)
        {
            OnLapFinished();
        }

        lastCheckPointIndex = checkPointIndex;
    }










    private void OnLapFinished()
    {
        currentLap++;


        /*if (currentLapTime < bestLapTime)
        {
            bestLapTime = currentLapTime;

        }*/


        if (currentLap >= totalLaps)
        {
            EndRace();
        }
        else
        {
            //Reset for next lap 
            currentLapTime = 0f;
            lastCheckPointIndex = isCircuit ? 0 : -1;
        }


    }



    private void StartRace()
    {
        raceStarted = true;
        raceFinished = false;

    }


    private void EndRace()
    {
        raceFinished = true;
        raceStarted = false;
    }


    private void UpdateTimers()
    {
        currentLapTime += Time.deltaTime;
        //overallRaceTime += Time.deltaTime;
    }

    private void UpdateUI()
    {
        currentLapTimerText.text = FormatTime(currentLapTime);
        //overallRaceTimerText.text = FormatTime(overallRaceTime);
       /*lapText.text = "LAP:" + currentLap + "/" + totalLaps;
        bestLapTimeText.text = FormatTime(bestLapTime);*/

    }



    private string FormatTime(float time)
    {
        if (float.IsInfinity(time) || time < 0f)
        {
            return "--:--:---";
        }

        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int milliseconds = (int)((time - (minutes * 60) - seconds) * 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
