using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEndExtends;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        EasyBackEnd.Instance.Init(() =>
        {
            //쉽게 게스트 로그인!
            EasyBackEnd.Instance.GuestLogin();

        }, () =>
        {
            Debug.Log("실패");
        });
    }

    public void OnClickLoadChartButton()
    {
        //쉽게 차트 가져오기!
        ChartManager.Instance.LoadChartDatas<ItemData>("chartExample");

        //가져온 차트 데이터 사용하기!
        ItemData item = ChartManager.Instance.GetDataById<ItemData>(0);
        Debug.Log(item.hpPower);
    }
}
