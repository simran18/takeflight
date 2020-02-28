using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using OVR;

public class FlyInDream : LogicBranch
{
    public override string Name => "FlyInDream";

    public float minimumWaitTimeBeforeTextUpdate = 3;

    // References to the UI interface that should be modified
    [Header("UI Elements to update")]
    public TextMeshProUGUI textbox;
    public Image img;
    public GameObject continuePrompt; // Disabled until after waitTime has passed;


    [Header("UIContent")]
    public bool progressToNext;
    [SerializeField] private OVRInput.Button buttonToProgress = OVRInput.Button.Any;
    [SerializeField] private int currentContentIndex = 0;
    [SerializeField] private UIContent[] tutorialContent = new UIContent[0];

    [Header("Next Logic Branch")]
    public string nextLogicBranchName;

    // returns true once at or past the last slide
    [SerializeField] private bool showedAllContent => currentContentIndex >= tutorialContent.Length - 1;

    # region private classes
    // UI Content groups together all of the information that should be displayed at once
    [System.Serializable]
    private class UIContent
    {
        public string text; // What to say
        public Sprite sprite; // A picture to display
        public bool autoProgress = false; // if true, continues automatically to the next content after the waitTime
    }
    #endregion

    protected override void OnCall() {
        StartCoroutine(ManageFlyingTutorialUI());
    }

    // Start is called before the first frame update
    private void Update() {
        progressToNext = OVRInput.GetDown(buttonToProgress) || Input.GetKeyDown("space");
    }

    IEnumerator ManageFlyingTutorialUI() {
        UpdateWithUIContent(tutorialContent[currentContentIndex]);
        for( ; currentContentIndex < tutorialContent.Length; currentContentIndex++) {
            UIContent currentContent = tutorialContent[currentContentIndex];
            UpdateWithUIContent(currentContent);
            yield return new WaitForSecondsRealtime(minimumWaitTimeBeforeTextUpdate);
            continuePrompt.SetActive(true);

            if (!currentContent.autoProgress && !showedAllContent) {
                // Wait until progress to next is true, then reset it
                yield return new WaitUntil(() => progressToNext); 
                progressToNext = false;
            }

            continuePrompt.SetActive(false);
        }
        
        MoveToBranch(nextLogicBranchName);
    }

    // A public function that can be accessed in a UnityEvent to trigger progression
    public void TriggerProgressToNext() {
        progressToNext = true;
    }

    private void UpdateWithUIContent(UIContent content) {
        textbox.text = content.text;
        img.sprite = content.sprite;
        continuePrompt.SetActive(false);
    }
}
