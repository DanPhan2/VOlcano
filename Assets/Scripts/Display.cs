using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Display : MonoBehaviour
{
    public VolcanoEruption mainScript;
    
    Hex chosenHex;

    public TMP_Text ProbabilityTextHexes;
    public TMP_Text ProbabilityTextPercent;

    public TMP_Text chosenHexTextLava;
    public TMP_Text chosenHexTextRain;

    public TMP_Text TimerText;

    public GameObject Control;

    public GameObject PreparationDisplay;

    public GameObject RainDisplay;

    public GameObject EruptionDisplay;

    public GameObject HungerDisplay;

    public GameObject RotDisplay;

    public GameObject DisasterDisplay;

    public GameObject NothingDisplay;

    public GameObject EndScreen;

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

    public void SetRainDisplay(Hex hex)
    {
        chosenHex = hex;
        RainDisplay.SetActive(true);
        chosenHexTextRain.text = "Wybrane pole: "+hex.Q+","+hex.R;
        //oznaczenie wybranego Hexa
        //buttony ok i skip
    }
    public void SetDisasterDisplay()
    {
        DisasterDisplay.SetActive(true);
        //oznaczenie wybranego Hexa
        //buttony ok i skip
    }
    public void SetRotDisplay()
    {
        RotDisplay.SetActive(true);
        //oznaczenie wybranego Hexa
        //buttony ok i skip
    }
    public void SetNothingDisplay()
    {
        NothingDisplay.SetActive(true);
        //oznaczenie wybranego Hexa
        //buttony ok i skip
    }
    public void SetHungerDisplay()
    {
        HungerDisplay.SetActive(true);
        //oznaczenie wybranego Hexa
        //buttony ok i skip
    }
    public void SetEruptionDisplay(Hex hex)
    {
        chosenHex = hex;
        EruptionDisplay.SetActive(true);
        chosenHexTextLava.text = "Wybrane pole: "+hex.Q+","+hex.R;
        //oznaczenie wybranego Hexa
        //buttony ok i skip
    }

    public void Erupt()
    {
        mainScript.LavaToChosenHex(chosenHex);
    }

    public void Rainy()
    {
        mainScript.ExecuteRain(chosenHex);
    }
}
