using UnityEngine;

[CreateAssetMenu(fileName = "new ItemData",menuName = "Time the Vice/GameObject/Data/new Item")]
public class ItemData : ScriptableObject
{
    public string master;       //���� ����
    public string id;           //���� ���̵�
    public new string name;     //���� �̸�
    public string itemEx;       //���� ����

    public Sprite sprite;
}
