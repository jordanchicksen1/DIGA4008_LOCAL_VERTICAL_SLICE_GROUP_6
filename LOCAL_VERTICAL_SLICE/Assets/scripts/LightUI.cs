using UnityEngine;
using UnityEngine.UI;

public class LightUI : MonoBehaviour
{
    public PlayerLight playerLight;
    public Image fillImage;

    void Update()
    {
        fillImage.fillAmount = playerLight.GetLightPercent();
    }
}