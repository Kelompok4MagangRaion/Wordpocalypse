using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DurabilityWeapon : MonoBehaviour
{
    public FloatValue durabilitySword;
    public FloatValue bullet;

    public Slider durSword;
    public Text bullText;

    void Start()
    {
        durSword.maxValue = durabilitySword.InitialValue;
        durSword.value = durabilitySword.RuntimeValue;
    }

    void Update()
    {
        durSword.value = durabilitySword.RuntimeValue;
        bullText.text = "Bullet : " + bullet.RuntimeValue;
    }
}
