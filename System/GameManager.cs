using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using static Soundnamespace;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float typingSpeed;
    public float autoTypeingSpeed;

    public bool autoPlay = false;
    public bool skipPlay = false;
    public bool hidUI = false;

    public bool isPlatNextScens;                            //수색을 하거나 다른 이벤트가 있을시 false
    [SerializeField] private GetLoadScens nextPlayScens;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.anyKeyDown && hidUI)
        {
            UISysetm.Instance.UI_OffHidUI();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            nextPlayScens.PlayScens();
        }

    }

    #region Scens Function
    public void GetNextScenes(GetLoadScens get)
    {
        nextPlayScens = get;
    }
    public void PlayDialogScenes()
    {
        nextPlayScens.PlayScens();
    }
    public static void LoadScenes(string loadSceneName)
    {
        SceneManager.LoadSceneAsync(loadSceneName);
    }

    public static void UnLoadScenes(string unloadSceneName)
    {
        SceneManager.UnloadSceneAsync(unloadSceneName);
    }
    #endregion
}

