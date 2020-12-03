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
    public int itemId;

    public string itemName;

    public double hpPower;

    [SerializeField]
    private int num;
}