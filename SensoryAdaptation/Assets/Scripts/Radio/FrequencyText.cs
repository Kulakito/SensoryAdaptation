using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FrequencyText : MonoBehaviour
{
    [SerializeField] TMP_Text te;
    [SerializeField] Slider slider;
    [SerializeField, TextArea] string pureText;
    [SerializeField] float neededFrequency;
    int[] ids;
    float radioValue;


    void Start()
    {
        ResetIds();

        StartCoroutine(FlipFlopText());
    }

    IEnumerator FlipFlopText()
    {
        while (true)
        {
            radioValue = slider.value;
            char[] newText = new char[pureText.Length];

            for (int i = 0; i < newText.Length; i++)
                newText[i] = pureText[i];

            for (int i = 0; i < ids.Length; i++)
            {
                float n = i / (float)(ids.Length - 1);
                float m = Mathf.Abs(neededFrequency - radioValue);
                if (n < m)
                {
                    newText[ids[i]] = (char)('À' + Random.Range(0, 36));
                }
            }

            te.text = string.Join("", newText);
            yield return new WaitForSeconds(.125f);
        }
    }

    void ResetIds()
    {
        ids = new int[pureText.Length];
        List<int> temp = new List<int>();
        for (int i = 0; i < pureText.Length; i++)
            temp.Add(i);
        for (int i = 0; i < ids.Length; i++)
        {
            int j = Random.Range(0, temp.Count);
            ids[i] = temp[j];
            temp.RemoveAt(j);
        }
    }
}
