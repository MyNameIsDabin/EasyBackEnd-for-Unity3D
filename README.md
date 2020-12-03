# EasyBackEnd-for-Unity3D
유니티에서 뒤끝을 좀 더 쉽게 사용하려고 만든 유틸 스크립트 입니다.

## 차트 매니저
Json 시리얼라이즈/디시리얼라이즈 기능을 이용해서 편하게 클래스만 설계하면 바로 차트의 데이터를 가져올 수 있습니다.
```
//차트 가져오기
ChartManager.Instance.LoadChartDatas<ItemData>("chartExample");

//불러온 차트의 데이터 리스트 가져오기
List<ItemData> itemDatas = ChartManager.Instance.GetDataList<ItemData>();

//가져온 차트의 데이터 사용하기
ItemData item = ChartManager.Instance.GetDataById<ItemData>(2);
Debug.Log(item.hpPower);
```

차트 매니저를 이용해서 클래스에 데이터를 담기 위해 다음 규칙을 지켜야 합니다.
```
/*
[Serializable]
public class ItemData
{
    public int itemId;

    public string itemName;

    public double hpPower;

    [SerializeField]
    private int num;
}
```
- 반드시 뒤끝 차트의 칼럼 명과 클래스 멤버 변수의 이름이 정확히 일치해야 합니다.
- 클래스 상단에 [Serializable] 가 반드시 필요합니다.
- 클래스 멤버 변수는 반드시 [SerializeField] 로 선언하거나 혹은 public 으로 선언해야 합니다.
