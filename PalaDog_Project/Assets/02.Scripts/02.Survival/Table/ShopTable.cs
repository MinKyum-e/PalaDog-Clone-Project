public enum ListType
{
    Enforce
}

public enum GoodsType
{

}

public class ShopTable 
{
    int index; //상점 고유 인덱스
    string GameName; //
    int group;
    ListType grade;
    int prelist;
    GoodsType goodsType;
    int goodsValue;
    int value;
}
