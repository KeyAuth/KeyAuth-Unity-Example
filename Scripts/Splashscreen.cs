using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Splashscreen : MonoBehaviour
{
    public TextMeshProUGUI exampleLbl;

    private void Start()
    {
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = "U";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "n";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "i";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "t";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "y";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + " E";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "x";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "a";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "m";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "p";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "l";
        yield return new WaitForSeconds(0.3f);
        exampleLbl.text = exampleLbl.text + "e";
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(1);
    }
}
