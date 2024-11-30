using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStuaBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image MPImage;
    public Image Weapon_Normal;
    public Image Weapon_BigArea;
    public Image Skill_Stop;
    private void Awake()
    {
        healthDelayImage.fillAmount = healthImage.fillAmount;
    }
    private void Update()
    {
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime;
        }
    }
    public void OnStatusChage(float persentageHP,float persentageMP)
    {
        healthImage.fillAmount = persentageHP;
        MPImage.fillAmount = persentageMP;
    }
}
