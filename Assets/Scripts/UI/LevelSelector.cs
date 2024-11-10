using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LevelSelector : MonoBehaviour
{

    public GameObject fadePanel;
    private CanvasGroup fadeCanvasGroup;
    private Button[] levelButtons;
    private int highestLevel;
    // Start is called before the first frame update
    void Start()
    {
        highestLevel = Preferences.GetMaxLvl();

        levelButtons = GetComponentsInChildren<Button>();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > highestLevel)
            {
                levelButtons[i].interactable = false;
            }
        }
        fadeCanvasGroup = fadePanel.GetComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0;
        fadePanel.SetActive(false);

    }
    public void LoadLevel(int levelNum)
    {
        AudioManager.GetInstance().PlayConfirmButton();
        Preferences.SetCurrentLvl(levelNum);

        Button selectedButton = levelButtons[levelNum - 1];
        selectedButton.transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);

        StartCoroutine(LoadLevelAfterDelay(levelNum));
    }
    private IEnumerator LoadLevelAfterDelay(int levelNum)
    {
        yield return new WaitForSeconds(1.0f);

        fadePanel.SetActive(true);
        fadeCanvasGroup.DOFade(1f, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene("Level " + levelNum);
        });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
