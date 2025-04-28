using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    public ItemData itemData;

    bool isCheckItme = false;
    private void OnMouseDown()
    {
        Debug.Log("get Click");

        if(TestPlayManager.Instance.isTextPlay) {  return; }

        if (isCheckItme) return;

        Debug.Log($"get Item {itemData.id}");
        UISysetm.Instance.GetItem(itemData);
        isCheckItme = !isCheckItme;
    }
}
