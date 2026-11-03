using UnityEngine;
using UnityEngine.UI;
public class UniversalPowerupBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject barParent;

    private float maxTime;
    private float currentTime;
    private bool isActive;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!isActive)
            return;

        currentTime -= Time.deltaTime;

        fillImage.fillAmount = currentTime / maxTime;

        if (currentTime <= 0)
        {
            isActive = false;
            
        }
    }

    public void StartTimer(float duration, Color barColor)
    {
        maxTime = duration;
        currentTime = duration;

        fillImage.color = barColor;
        fillImage.fillAmount = 1f;

        isActive = true;
       
    }
}
