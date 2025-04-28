using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUI : MonoBehaviour
{
    public ItemData ItemData;

    public void ClickData()
    {
        UISysetm.Instance.GetNotepad(ItemData);
    }
}
