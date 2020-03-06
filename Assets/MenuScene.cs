using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour {
    public GameObject startPanel;
    public Button playButton;
    public Button customizationButton;
    public Button exitButton;

    public GameObject customizationPanel;
    public Button backButton;

    public string[] customizationParts;
    public Transform customizationGroupContainer;
    public GameObject customizationGroupPrefab;

    public Transform weapon;

    void Start() {
        playButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        customizationButton.onClick.AddListener(ShowCustomizationPanel);
        exitButton.onClick.AddListener(() => Application.Quit());

        backButton.onClick.AddListener(ShowStartPanel);

        foreach (string customizationPart in customizationParts) {
            GameObject customizationGroup = Instantiate(customizationGroupPrefab);
            customizationGroup.transform.Find("Label").GetComponent<Text>().text = customizationPart;

            bool isAttached = PlayerPrefs.GetInt(customizationPart, 0) == 0 ? false : true;

            GameObject targetMesh = weapon.Find(customizationPart).gameObject;
            targetMesh.SetActive(isAttached);

            Button button = customizationGroup.transform.Find("Button").GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = isAttached ? "Detach" : "Attach";

            button.onClick.AddListener(() => {
                bool wasAttached = PlayerPrefs.GetInt(customizationPart, 0) == 0 ? false : true;

                if (wasAttached) {
                    PlayerPrefs.SetInt(customizationPart, 0);
                    button.GetComponentInChildren<Text>().text = "Attach";
                    targetMesh.SetActive(false);
                }
                else {
                    PlayerPrefs.SetInt(customizationPart, 1);
                    button.GetComponentInChildren<Text>().text = "Detach";
                    targetMesh.SetActive(true);
                }
            });

            customizationGroup.transform.SetParent(customizationGroupContainer, false);
        }
    }

    void ShowCustomizationPanel() {
        startPanel.SetActive(false);
        customizationPanel.SetActive(true);
    }

    void ShowStartPanel() {
        customizationPanel.SetActive(false);
        startPanel.SetActive(true);
    }
}
