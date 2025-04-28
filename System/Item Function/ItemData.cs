using UnityEngine;

[CreateAssetMenu(fileName = "new ItemData",menuName = "Time the Vice/GameObject/Data/new Item")]
public class ItemData : ScriptableObject
{
    public string master;       //증거 주인
    public string id;           //증거 아이디
    public new string name;     //증거 이름
    public string itemEx;       //증거 설명

    public Sprite sprite;
}
