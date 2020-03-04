using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using OVR;

public class FlyInDream : LogicBranch
{
    public override string Name => "FlyInDream";

    public float minimumWaitTimeBeforeTextUpdate = 1.5f;

    // References to the UI interface that should be modified
    [Header("UI Elements to update")]
    public TextMeshProUGUI textbox;
    public Image img;
    public GameObject continuePrompt; // Disabled until after waitTime has passed;


    [Header("UIContent")]
    [SerializeField] private OVRInput.Button buttonToProgress = OVRInput.Button.Any;
    [SerializeField] private int currentContentIndex = 0;
    [SerializeField] private UIContent[] tutorialContent = new UIContent[0];

    [Header("Transitioning")]
    public GameObject launchPad;
    public float timeLimit = 60;
    public int targetNumberOfRings = 5;
    public string nextLogicBranchName;

    // returns true once at or past the last slide
    [SerializeField] private bool showedAllContent => currentContentIndex >= tutorialContent.Length - 1;
    [SerializeField] private int ringCount = 0;

#if DEBUG
    [ContextMenu("Move to next branch")]
    private void DebugLogicBranchTransition()
    {
        ringCount = targetNumberOfRings + 1;
    }
#endif

    #region private classes
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
        StartCoroutine(RunTimeLimitTimer(timeLimit));
        StartCoroutine(CountNumberOfRings());
    }

    IEnumerator ManageFlyingTutorialUI() {
        UpdateTutorialWithUIContent(tutorialContent[currentContentIndex]);
        for( ; currentContentIndex < tutorialContent.Length; currentContentIndex++) {
            UIContent currentContent = tutorialContent[currentContentIndex];
            UpdateTutorialWithUIContent(currentContent);
            yield return new WaitForSecondsRealtime(minimumWaitTimeBeforeTextUpdate);
            continuePrompt.SetActive(true);

            if (!currentContent.autoProgress && !showedAllContent) {
                // Wait until progress to next is true, then reset it
                yield return new WaitUntil(() => OVRInput.GetDown(buttonToProgress) || Input.GetKeyDown("space"));
            }

            continuePrompt.SetActive(false);
        }
        launchPad?.SetActive(true);
    }

    IEnumerator CountNumberOfRings() {
        yield return new WaitUntil(() => ringCount > targetNumberOfRings);
        Debug.Log("CountNumberOfRings() triggered next logic branch");

        MoveToBranch(nextLogicBranchName);
        StopAllCoroutines();
    }

    public void IncrementRingCount() {
        ringCount++;
    }

    // Limits the player's time in this callable to the given number or seconds
    IEnumerator RunTimeLimitTimer(float seconds) {
        yield return new WaitForSeconds(seconds);
        Debug.Log("RunTimeLimitTimer(" + seconds+") triggered next logic branch");

        MoveToBranch(nextLogicBranchName);
        StopAllCoroutines();
    }

    private void UpdateTutorialWithUIContent(UIContent content) {
        textbox.text = content.text;
        img.sprite = content.sprite;
        continuePrompt.SetActive(false);
    }
}
