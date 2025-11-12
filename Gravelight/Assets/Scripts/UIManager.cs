using UnityEngine;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private float delayBeforeMenu = 5f;

    private void Start()
    {
        StartCoroutine(ShowMainMenuAfterDelay());
    }

    private IEnumerator ShowMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeMenu);

        yield return StartCoroutine(ScreenFader.Instance.FadeToWhite(0.8f));

        yield return new WaitForSeconds(0.8f);

        titleScreen.SetActive(false);
        mainMenu.SetActive(true);

        yield return StartCoroutine(ScreenFader.Instance.FadeFromWhite(0.8f));
    }
}
