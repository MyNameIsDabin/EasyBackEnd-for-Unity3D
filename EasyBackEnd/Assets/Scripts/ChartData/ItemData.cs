using System;
using UnityEngine;

/*
 * 주의사항 
 * 클래스 상단에 [Serializable] 필요!
 * 멤버 변수는 [SerializeField] 혹은 public 으로 선언!
 */

[Serializable]
public class ItemData
{
    public string itemId;

    public string itemName;

    [SerializeField]
    private double hpPower;

    public int num;
}