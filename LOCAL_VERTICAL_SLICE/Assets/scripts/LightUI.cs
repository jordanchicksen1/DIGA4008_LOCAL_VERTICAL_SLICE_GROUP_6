using UnityEngine;
using UnityEngine.UI;

public class LightUI : MonoBehaviour
{
    public PlayerLight playerLight;
    public Image fillImage;

    void Update()
    {
        //update image fill amount to the light percentage
        fillImage.fillAmount = playerLight.GetLightPercent();
    }
}