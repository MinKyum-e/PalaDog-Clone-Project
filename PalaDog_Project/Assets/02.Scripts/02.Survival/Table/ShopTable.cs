namespace ShopEnums {
    public enum ListType
    {
        Enforce,
        Unlock,
        Spawn,
    }

    public enum GoodsType
    {
        Gold,
        Food,
    }

    public enum UnLockType
    {
        InGameUnit,
        Evolution
    }
}



public struct ShopTable 
{
    public int index; //상점 고유 인덱스
    public string name; //
    public int group;
    public ShopEnums.ListType grade;
    public int prelist;
    public ShopEnums.GoodsType goodsType;
    public int goodsValue;
    public int value;
}
