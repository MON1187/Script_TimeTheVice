using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;

public enum MoodType
{
    Default = 0,
    Fun,
    Dis,
    Ang,
    Sed
}

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public DialogList[] dialogLists;
    public string[] urls;

    [SerializeField] private GameObject dialogHidUI;
    [SerializeField] private TMP_Text uiName;
    [SerializeField] private TMP_Text uiText;

    public bool isWaiting = false;
    public bool hidDialogUi;

    //public bool currentPlayScens = false;  //현재 진행중인지 확인용
    [SerializeField] private bool isClickSkip = false;   //Input Key as Skip Text
    [SerializeField] public bool isPlayScnes = false;

    private void Awake()
    {
        Instance = this;

        urls = URL.URLs.ToArray();

        for (int i = 0; i < dialogLists.Length; i++)
        {
            dialogLists[i].senceNumber = i;
            StartLoadDialogData(urls[i], dialogLists[i].dialog);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            isClickSkip = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartLoadDialog(dialogLists[0].dialog);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartLoadDialog(dialogLists[1].dialog);
        } 
    }

    private void StartLoadDialogData(string URL, Dialog[] dialogs)
    {
        StartCoroutine(StartLoadDialogDataCoroutine(URL, dialogs));
    }

    IEnumerator StartLoadDialogDataCoroutine(string URL,Dialog[] dialog)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        print(data);

        LoadDialogData(data, dialog);
    }

    private void LoadDialogData(string sdv, Dialog[] dialog)
    {
        string[] L = sdv.Split('\n'); // 행 단위로 나눔
        //int index = L.Length;         // 행의 개수 // dialog 사이즈 지정용, 기능 안되서 주석처리

        for (int i = 0; i < dialog.Length; i++)
        {
            string row = L[i+1].Trim(); //처음 지정 단계는 스킵 및 각 행의 앞뒤 공백 제거
            // 탭으로 분리된 데이터 확인
            string[] s = row.Split('\t');

            try
            {
                dialog[i].Id = s[0];
                dialog[i].dialog = s[1];
                dialog[i].Time = float.Parse(s[2]);
                dialog[i].Face = (MoodType)Enum.Parse(typeof(MoodType), s[3], true);
                dialog[i].AutoPlay = string.Equals(s[4], "T", StringComparison.OrdinalIgnoreCase);
            }
            catch(Exception ex) {
                Debug.LogError(ex.ToString());
            }
        }
    }

    public void StartLoadDialog(Dialog[] dialogs)
    {
        StartCoroutine(LoadDialog(dialogs));
    }

    private IEnumerator LoadDialog(Dialog[] dialog)
    {
        if (!isPlayScnes) isPlayScnes = true;
        else yield break;

        Debug.Log("Start");
        for (int i = 0;i < dialog.Length;i++)
        {
            Debug.Log("Round : i");
            if (dialog[i].Id == ".") { uiName.text = ""; }
            else { uiName.text = dialog[i].Id; } 
            uiText.text = "";//Text Clear
            isClickSkip = false;
            foreach (char sentence in dialog[i].dialog)
            {
                yield return StartCoroutine(TypeSentence(sentence));  // 각 대사에 대해 타자 효과 실행
            }
            if (dialog[i].AutoPlay == false) { isWaiting = true;}
            
            if (!GameManager.Instance.autoPlay) 
            {
                while(true)
                {
                    if(Input.anyKeyDown)
                    {
                        break;
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
            else { yield return new WaitForSeconds(dialog[i].Time); }
        }

        isPlayScnes = false;

        if(GameManager.Instance.isPlatNextScens)
        {
            GameManager.Instance.PlayDialogScenes();
        }
    }

    private IEnumerator TypeSentence(char sentence)
    {

        while (isWaiting) { yield return null; }

        uiText.text += sentence;  // 텍스트 초기화

        if (isClickSkip) { yield return null; }
        else 
        { 
            if(!GameManager.Instance.autoPlay)
            {
                yield return new WaitForSeconds(GameManager.Instance.typingSpeed); // 타자 속도 딜레이 
            }
            else
            {
                yield return new WaitForSeconds(GameManager.Instance.autoTypeingSpeed);
            }
        }
    }
}
[System.Serializable]
public class DialogList
{
    public int senceNumber;
    public Dialog[] dialog;
}

//[CreateAssetMenu(fileName = "new Dialog",menuName = "Time the Vice/GameObject/Data/new dialog")]
[System.Serializable]
public class Dialog
{
    public string Id;
    [TextArea(3,5)] public string dialog;
    public float Time;
    public MoodType Face;
    public bool AutoPlay;
}

public static class URL
{
    public static readonly ReadOnlyCollection<string> URLs = 
    new ReadOnlyCollection<string>(new string[]
    {
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=0#gid=0",
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=1284277176",
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=1845778310",
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=1603882562",
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=836579479",//
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=1491224989",
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=633298070",
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=943692960",
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=1887267014",
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=104194588",
        "https://docs.google.com/spreadsheets/d/1YVvHpxEKLplgDdRFGN9QXPFDcRqQabF4ylAzmOjdelU/export?format=tsv&gid=316354781", 
    });
}
