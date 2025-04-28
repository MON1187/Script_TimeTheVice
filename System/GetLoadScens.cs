using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class GetLoadScens : MonoBehaviour
{
    [SerializeField] private GetLoadScens nextLoadScens;
    public int myScensNumber;

    [SerializeField] private int encyclopediaValue;

    public EncyclopediaScens encyclopediaScens;

    public void PlayScens()
    {
        LoadScens(myScensNumber);
    }

    public void LoadScens(int scensIndex)
    {
        if (encyclopediaScens.isSelecScens)
        {
            encyclopediaScens.A();
            return;
        }

        GameManager.Instance.GetNextScenes(nextLoadScens);
        DialogManager.Instance.StartLoadDialog(DialogManager.Instance.dialogLists[scensIndex].dialog);
    }
}
[Serializable]
public class EncyclopediaScens
{
    public bool isSelecScens;                                   //다른 분기점으로 나뉠 때 사용
    public bool isGetEvidence;                                  //증거 확보 하는지 확인 ( Ture = 한다 )
    public GetLoadScens[] selectloadScens;                      //다른 루트로 넘어갈수 있는 씬

    public void A()
    {
        Debug.Log("Select Choose");
    }
}

/*
[CustomEditor(typeof(GetLoadScens))]
public class OnSelectLoadScens : Editor
{
    GetLoadScens getLoadScens;

    void OnEnable()
    {
        getLoadScens = (GetLoadScens)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (getLoadScens.encyclopediaScens.isSelecScens)
        {
            //getLoadScens.encyclopediaScens.selectloadScens = EditorGUILayout.ObjectField("On select", getLoadScens.encyclopediaScens.selectloadScens, typeof(GetLoadScens), true) as GetLoadScens;
        }
    }
}*/