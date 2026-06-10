using UnityEngine;

public class UI : MonoBehaviour
{
    private bool skillTreeEnabled;
    private bool intentoryEnabled;

    public bool alternativeInput { get; private set; }
    private PlayerInputSet input;

    public UI_SkillTree skillTreeUI         { get; private set; }
    public UI_SkillToolTip skillToolTipUI   { get; private set; }
    public UI_Options optionsUI             { get; private set; }
    public UI_DeathScreen deathScreenUI     { get; private set; }

    [SerializeField] private GameObject[] uiElements;

    private void Awake()
    {
        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true); //Added "true" in parameter to get component in children even the game object is disable.
        optionsUI = GetComponentInChildren<UI_Options>(true);
        deathScreenUI = GetComponentInChildren<UI_DeathScreen>(true);

        skillToolTipUI = GetComponentInChildren<UI_SkillToolTip>();

        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
    }

    private void Start()
    {
        
    }

    public void Initialize(PlayerInputSet inputSet)
    {
        input = inputSet;

        input.UI.SkillTreeUI.performed += ctx => ToggleSkillTreeUI();

        // do the same with future inventory UI;

        input.UI.AlternativeInput.performed += ctx => alternativeInput = true;
        input.UI.AlternativeInput.canceled += ctx => alternativeInput = false;

        input.UI.OptionsUI.performed += ctx =>
        {
            foreach (var uiElement in uiElements)
            {
                if (uiElement.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }

            Time.timeScale = 0;
            skillTreeEnabled = false;
            OpenOptionsUI(true);
        };
    }
    
    public void SetPlayerControl(bool enable)
    {
        if (enable)
            input.Player.Enable();
        else
            input.Player.Disable();
    }

    private void StopPlayerControlsIfNeeded()
    {
        foreach (var uiElement in uiElements)
        {
            if (uiElement.activeSelf)
            {
                SetPlayerControl(false);
                return;
            }
        }

        SetPlayerControl(true);
    }

    public void SetTooltipAbobe()
    {
        //itemTooltip.transform.SetAsLastSibling();
        skillToolTipUI.transform.SetAsLastSibling();
        //statTooltipUI.transform.SetAsLastSibling();
    }

    public void HideAllTooltip()
    {
        skillToolTipUI.ShowToolTip(false, null);
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeUI.transform.SetAsLastSibling();
        SetTooltipAbobe();

        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);

        HideAllTooltip();

        // Set player control opposite of skill tree;
        StopPlayerControlsIfNeeded();
    }

    public void ToggleInventoryUI()
    {
        intentoryEnabled = !intentoryEnabled;

        SetTooltipAbobe();
        StopPlayerControlsIfNeeded();
    }

    public void SwitchToInGameUI()
    {
        HideAllTooltip();

        //SwitchTo(inGameUI.gameObject);

        StopPlayerControlsIfNeeded();

        skillTreeEnabled = false;
        intentoryEnabled = false;
    }

    private void SwitchTo(GameObject targetObject)
    {
        foreach (var uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }

        targetObject.SetActive(true);
    }

    public void OpenOptionsUI(bool openOptionsUI)
    {
        HideAllTooltip();
        StopPlayerControlsIfNeeded();
        SwitchTo(optionsUI.gameObject);
    }

    public void OpenStorageUI(bool openStorageUI)
    {


        StopPlayerControlsIfNeeded();

        if (openStorageUI == false)
            HideAllTooltip();
    }

    public void OpenMachantUI(bool openMachantUI)
    {


        StopPlayerControlsIfNeeded();

        if (openMachantUI == false)
            HideAllTooltip();
    }

    public void OpenDeathScreenUI()
    {
        SwitchTo(deathScreenUI.gameObject);
        // Already disable player input from player health;
    }
}
