using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SnackBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    public void ShowMessage(string text,float time)
    {
        gameObject.SetActive(true);
        this.text.text = text;
        StartCoroutine(TimeToDisapear(time));
    }

    IEnumerator TimeToDisapear(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
