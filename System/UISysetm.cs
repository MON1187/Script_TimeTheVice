using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

using static Soundnamespace;
public class UISysetm : MonoBehaviour
{
    #region 
    public static UISysetm Instance;
    private void Awake()
    {
        Instance = this;
        SettingSystemSetValue();
        SetBagSystemValue();
        DialogUISystemSetValue();
    }


    [SerializeField]List<ItemData> itemDialog = new List<ItemData>();

    public Transform[] contentPositions = new Transform[6];
    public GameObject saveItemDataBorder;

    public Image icon;
    public new TMP_Text name;
    public TMP_Text text;

    #endregion

    #region UI Funciton

    #region Setting OnOFF
    [Header("Setting Resource")]
    [SerializeField] private GameObject ui_SettingPanel;

    [SerializeField] private Button settingOnButton;
    [SerializeField] private Button settingOffButton;

    private void SettingSystemSetValue()
    {
        settingOnButton.onClick.AddListener(OnSettingPanel);
        settingOffButton.onClick.AddListener(OffSettingPanel);

        SetAudioValue();
    }

    public void OnSettingPanel()
    {
        ui_SettingPanel.SetActive(true);
    }
    public void OffSettingPanel()
    {
        ui_SettingPanel.SetActive(false);
    }

    #endregion

    #region Player Bag system..?

    [Header("Bag System UI Resource")]

    public Button ui_PlayerBagONButton;
    public Button ui_PlayerBagOFFButton;

    public GameObject ui_PlayerBagGameObject;
    public GameObject ui_Character;
    public GameObject ui_Map;

    private void SetBagSystemValue()
    {
        ui_PlayerBagONButton.onClick.AddListener(UI_OnBag);
        ui_PlayerBagOFFButton.onClick.AddListener(UI_OffBag);
    }

    public void UI_OnBag()
    {
        ui_PlayerBagGameObject?.SetActive(true);
    }
    public void UI_OffBag()
    {
        ui_PlayerBagGameObject?.SetActive(false);
    }

    public void UI_OnMap()
    {
        ui_Map?.SetActive(true);
        UI_UnCharacter();
    }
    public void UI_OnCharacter()
    {
        ui_Character?.SetActive(true);
        UI_UnMap();
    }
    private void UI_UnMap()
    {
        ui_Map?.SetActive(false);
    } 
    private void UI_UnCharacter()
    {
        ui_Character?.SetActive(false);
    }
    #endregion

    #region Dialog System Hid UI
    [Header("Dialog System UI")]
    [SerializeField] private Button autoButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button hidUIButton;
    [SerializeField] private GameObject dialogPanelUI;

    private void DialogUISystemSetValue()
    {
        autoButton.onClick.AddListener(UI_SetAutoPlay);
        skipButton.onClick.AddListener(UI_SetSkip);
        hidUIButton.onClick.AddListener(UI_OnHidUI);
    }

    public void UI_SetAutoPlay()
    {
        GameManager.Instance.autoPlay = !GameManager.Instance.autoPlay;
    }

    public void UI_SetSkip()
    {
        GameManager.Instance.skipPlay = !GameManager.Instance.skipPlay;
    }

    public void UI_OnHidUI()
    {
        GameManager.Instance.hidUI = !GameManager.Instance.hidUI;

        dialogPanelUI.SetActive(false);
    }
    public void UI_OffHidUI()
    {
        GameManager.Instance.hidUI = !GameManager.Instance.hidUI;

        dialogPanelUI.SetActive(true);
    }

    #region Dialog Setting
    [Header("Setting Resource UI")]
    [SerializeField] private AudioMixer m_AudioMixer;     //오디오 믹설 연결
    [SerializeField] private Slider m_MusicMasterSlider;  //마스터 슬라이더 연결
    [SerializeField] private Slider m_MusicBGMSlider;     //배경음 슬라이더 연결
    [SerializeField] private Slider m_MusicSFXSlider;     //효과음 슬라이더 연결

    [SerializeField] private Slider typingSpeedSlider;
    [SerializeField] private Slider autoTypingSpeedSlider;

    private void SetAudioValue()
    {
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);

        typingSpeedSlider.onValueChanged.AddListener(UI_SetTypingSpeed);
        autoTypingSpeedSlider.onValueChanged.AddListener (UI_SetAutoTypingSpeed);
    }

    #region Typing Speed
    public void UI_SetTypingSpeed(float value)
    {
        GameManager.Instance.typingSpeed = typingSpeedSlider.value;
    }
    public void UI_SetAutoTypingSpeed(float value)
    {
        GameManager.Instance.autoTypeingSpeed = autoTypingSpeedSlider.value;
    }
    #endregion

    #region Sound Function

    //메인 소리음을 조절할때 쓰는 함수입니다.
    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat(Main, Mathf.Log10(volume) * 20);
    }
    //배경음을 조절할때 쓰는 함수입니다.
    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat(BGM, Mathf.Log10(volume) * 20);
    }
    //효과음을 조절할때 쓰는 함수입니다.
    public void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat(SFX, Mathf.Log10(volume) * 20);
    }
    #endregion

    #endregion

    #endregion

    #endregion

    #region Item Function
    public void GetItem(ItemData itemData)
    {
        itemDialog.Add(itemData);

        for (int i = 0; i < itemDialog.Count; i++)
        {
            if (itemDialog[i].id == itemData.id)
            {
                icon.sprite = itemData.sprite;
                name.text = itemData.name;
                text.text = itemData.itemEx;

                GameObject a = Instantiate(saveItemDataBorder, contentPositions[OwnerOfEvidence(itemData.master)]);
                a.GetComponent<ClickUI>().ItemData = itemData;
                a.transform.Find("Icon").GetComponent<Image>().sprite = itemData.sprite;
                a.transform.Find("Name").GetComponent<TMP_Text>().text = itemData.name;
            }
        }
    }

    public void GetNotepad(ItemData itemData)
    {
        icon.sprite = itemData.sprite;
        name.text = itemData.name;
        text.text = itemData.itemEx;
    }
    #endregion

    #region extra
    private int OwnerOfEvidence(string name)
    {
        return name switch
        {
            CharacterName.Player                    => 0,
            CharacterName.Friend2                   => 0,
            CharacterName.Friend1                   => 0,
            CharacterName.PoliceChief               => 0,
            CharacterName.GirlFriend                => 0,
            CharacterName.FellowDetective           => 0,
            _ => default(int)
        };
    }
    #endregion
}
public static class Soundnamespace
{
    public const string Main = nameof(Main);
    public const string BGM = nameof(BGM);
    public const string SFX = nameof(SFX);
}


public static class CharacterName
{
    public const string Player              = nameof(Player);
    public const string Friend2             = nameof(Friend2);
    public const string Friend1             = nameof(Friend1);
    public const string PoliceChief         = nameof(PoliceChief);
    public const string GirlFriend          = nameof(GirlFriend);
    public const string FellowDetective     = nameof(FellowDetective);
}