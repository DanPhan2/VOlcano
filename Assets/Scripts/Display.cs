using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Display : MonoBehaviour
{
    public TMP_Text ProbabilityTextHexes;
    public TMP_Text ProbabilityTextPercent;

    public TMP_Text TimerText;

    public void SetProbabilityDisplay(Dictionary<Hex, float> probabilityDictonary)
    {
        var sortedDict = from entry in probabilityDictonary orderby entry.Value descending select entry;
        ProbabilityTextHexes.text = "";
        ProbabilityTextPercent.text = "";
        int i =0;
        foreach (var line in sortedDict)
        {
            ProbabilityTextHexes.text += "Hex " + line.Key.Q +","+line.Key.R +"\n";
            ProbabilityTextPercent.text += line.Value.ToString("0.00")+"%" +"\n";
            if (i==10) break;
            i++;
        }
        
    }

    public void SetTimer(float timer)
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        TimerText.text = "Do kolejnego ticku pozostało:\n"+string.Format("{0:00}:{1:00}", minutes, seconds);

    }
}
