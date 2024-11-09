using UnityEngine;
using DG.Tweening; 

public class MenuController : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject levelPanel;
    public GameObject configPanel;
    public GameObject infoPanel;

    private CanvasGroup menuCanvasGroup;
    private CanvasGroup levelCanvasGroup;
    private CanvasGroup configCanvasGroup;
    private CanvasGroup infoCanvasGroup;


    void Start()
    {
        menuCanvasGroup = MenuPanel.GetComponent<CanvasGroup>();
        levelCanvasGroup = levelPanel.GetComponent<CanvasGroup>();
        configCanvasGroup = configPanel.GetComponent<CanvasGroup>();
        infoCanvasGroup = infoPanel.GetComponent<CanvasGroup>();

        // Configura los paneles invisibles al inicio
        SetPanelActive(levelPanel, levelCanvasGroup, false);
        SetPanelActive(configPanel, configCanvasGroup, false);
        SetPanelActive(infoPanel, infoCanvasGroup, false);
    }
    public void ShowLevelSelect()
    {
        ShowPanel(levelPanel, levelCanvasGroup);
    }

    public void ShowConfig()
    {
        ShowPanel(configPanel, configCanvasGroup);
    }

    public void ShowDescription()
    {
        ShowPanel(infoPanel, infoCanvasGroup);
    }

    private void ShowPanel(GameObject panelToShow, CanvasGroup canvasGroupToShow)
    {
        menuCanvasGroup.DOFade(0f, 0.5f).OnComplete(() => MenuPanel.SetActive(false));

        SetPanelActive(panelToShow, canvasGroupToShow, true);
        canvasGroupToShow.DOFade(1f, 0.5f);
    }

    private void SetPanelActive(GameObject panel, CanvasGroup canvasGroup, bool active)
    {
        panel.SetActive(active);
        canvasGroup.alpha = active ? 1 : 0;
        canvasGroup.blocksRaycasts = active;
    }

    public void ReturnToMainMenu()
    {
        SetPanelActive(levelPanel, levelCanvasGroup, false);
        SetPanelActive(configPanel, configCanvasGroup, false);
        SetPanelActive(infoPanel, infoCanvasGroup, false);
        MenuPanel.SetActive(true);
        menuCanvasGroup.DOFade(1f, 0.5f);
    }

}
