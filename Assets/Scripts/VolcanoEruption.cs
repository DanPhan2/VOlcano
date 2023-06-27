using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public enum SpecialEvent {Eruption, Rain, Hunger, Nothing, Disaster, Rot}
public class VolcanoEruption : MonoBehaviour
{
    List<Hex> hexBag = new List<Hex>();
    List<Hex> lavaFilledHexes = new List<Hex>();
    public List<Hex> playerHexes = new List<Hex>();
    public List<SpecialEvent> eventsToSelect = new List<SpecialEvent>(){
        SpecialEvent.Nothing,
        SpecialEvent.Disaster,

        SpecialEvent.Rain,
        SpecialEvent.Rain,

        SpecialEvent.Hunger,
        SpecialEvent.Hunger,
        SpecialEvent.Hunger,
        SpecialEvent.Hunger,
        SpecialEvent.Hunger,
        SpecialEvent.Hunger,

        SpecialEvent.Rot,
        SpecialEvent.Rot,
        SpecialEvent.Rot,
        SpecialEvent.Rot,
        SpecialEvent.Rot,
        SpecialEvent.Rot,

        SpecialEvent.Eruption, 
        SpecialEvent.Eruption, 
        SpecialEvent.Eruption, 
        SpecialEvent.Eruption
    };
    public SpecialEvent currentRandomEvent;
    public List<(int dq, int dr)> hexNeighbours = new List<(int dq, int dr)>{(1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1)};
    GameObject[] hexes; 
    Object prefabLava;
    public Display display;
    //Timer logic
    bool isRunning;
    public float timeBetweenTicks;
    public float timeBeforeTickRemaining;

    static List<Hex> nextLavaTargets = new List<Hex>();

    //Tick related
    public bool eventFlag = false;
    public bool mainFlag = false;
    public bool secondFlag = false;
    public GameObject tickDisplay;

    //Interaction related
    public bool hexesClickable = false;

    void Start()
    {   
        prefabLava = Resources.Load<Object>("Prefabs/Lava");
        HexMap.GenerateMap();
        hexes = GameObject.FindGameObjectsWithTag("Hex");

        LavaToChosenHex(new Hex(0, 0));

        timeBeforeTickRemaining = timeBetweenTicks;

        isRunning = false;
    }

    // Update is called once per frame
    public void RunTime(bool yes)
    {
        isRunning = yes;
        //MakeHexesClickable(yes);
    }
    public void Update()
    {
        //Debug.Log(timeBeforeTickRemaining);
        if (isRunning)
        {
            if (timeBeforeTickRemaining > 0)
            {
                timeBeforeTickRemaining -= Time.deltaTime;
                if (timeBeforeTickRemaining < 0)
                {
                    timeBeforeTickRemaining = 0.0f;
                    //Debug.Log(timeBeforeTickRemaining);
                }
                UpdateDisplay();
            }
            else
            {
                //Debug.Log("should tick");
                StartCoroutine(TickCoroutine());
            }
        }
    }
    void  DrawEvent()
    {
        System.Random rnd = new System.Random();
        int s = rnd.Next(eventsToSelect.Count);
        Debug.Log(s);
        currentRandomEvent = eventsToSelect[s];
    }
    public IEnumerator TickCoroutine()
    {
        
        mainFlag = true;
        isRunning = false;
        Debug.Log("tick");
        DrawEvent();
        StartCoroutine(Eruption());

        while(mainFlag || secondFlag)
        {
            yield return null;
        }
        Debug.Log("tick ended");
        isRunning = true;
        timeBeforeTickRemaining = timeBetweenTicks;
    }

    public void SwitchSecondFlag()
    {
        secondFlag = false;
    }

    

    void RemoveLavaFilled(Hex hex)
    {
        lavaFilledHexes.Remove(hex);
        System.Random rnd = new System.Random();

        foreach ((int dq, int dr) neigh in hexNeighbours)
        {
            Hex neighHex = new Hex(hex.Q + neigh.dq, hex.R + neigh.dr);

            if (!HexMap.IsInRange(neighHex)) continue;

            if (!DoesntHaveLava(neighHex)) {
                Debug.Log("Adding: " + neighHex.Q + ", " + neighHex.R);
                hexBag.Add(hex);
            } else 
            {
                hexBag.Remove(neighHex);
            }
        }
    }



    void AddHexesToBag()
    {
        foreach (Hex hex in lavaFilledHexes)
        {
            AddNeighboursToBag(hex);
        }
    }

    void AddNeighboursToBag(Hex hex)
    {

        foreach ((int dq, int dr) neigh in hexNeighbours)
        {
            Hex neighHex = new Hex(hex.Q + neigh.dq, hex.R + neigh.dr);

            if (HexMap.IsInRange(neighHex) && DoesntHaveLava(neighHex)) {
                hexBag.Add(neighHex);
            }
        }
    }

    void RemoveFromBag(Hex rawHex)
    {
        do {
            hexBag.Remove(rawHex);
        } while (hexBag.Remove(rawHex));
    }

    public GameObject getHex(Hex hex)
    {
        return hexes[HexMap.GetHexIndex(hex.Q, hex.R)];
    }

    public GameObject getHex(int q, int r)
    {
        return hexes[HexMap.GetHexIndex(q, r)];
    }

    void RemoveFromAllInstancesFromBag(Hex hex) {
        do {
            hexBag.Remove(hex);
        } while (hexBag.Remove(hex));
    }


    public static void SetNextLavaTarget(Hex hex)
    {
        nextLavaTargets.Add(hex);
    }

    Hex ChooseHexForLava()
    {
        if (nextLavaTargets.Any())
        {
            foreach (Hex potentialHex in nextLavaTargets)
            {
                if (IsNextToLava(potentialHex))
                {
                    //TODO chyba trzeba usunac jeszcze z listyNext
                    Debug.Log("Choosing clicked");
                    nextLavaTargets.Remove(potentialHex);
                    return potentialHex;
                }
            }
        }

        Hex hex = DrawFromBag();
        return hex;
    }

    Hex DrawFromBag()
    {
        System.Random rnd = new System.Random();
        int s = rnd.Next(hexBag.Count);
        Debug.Log(s);
        return hexBag[s];
    }

    bool IsNextToLava(Hex hex)
    {
        bool lavaNeighbour = false;
        
        //iteracja po sąsiadach - czy którykolwiek ma lawę
        foreach ((int dq, int dr) neigh in hexNeighbours)
        {
            Hex neighHex = new Hex(hex.Q + neigh.dq, hex.R + neigh.dr);

            if (HexMap.IsInRange(neighHex) && !DoesntHaveLava(neighHex)) {
                lavaNeighbour = true;
            }
        }

        return lavaNeighbour;
    }

    bool IsNextToPlayerBase(Hex hex)
    {
        bool yes = false;
        
        //iteracja po sąsiadach - czy którykolwiek ma lawę
        foreach ((int dq, int dr) neigh in hexNeighbours)
        {
            Hex neighHex = new Hex(hex.Q + neigh.dq, hex.R + neigh.dr);

            if (HexMap.IsInRange(neighHex) && playerHexes.Contains(neighHex)) {
                yes = true;
            }
        }

        return yes;
    }

    bool DoesntHaveLava(Hex hex)
    {
        return !lavaFilledHexes.Contains(hex);
    }
    bool DoesntHaveLava(GameObject hex)
    {
        return hex.transform.childCount > 1; ///////!!!!!!!!!!!!!!!!
    }

    public Dictionary<Hex, float> GetProbability()
    {
        Dictionary<Hex, float> probDict = new Dictionary<Hex, float>();
        float allHexes = hexBag.Count;
        var numberOfHexes = from n in hexBag
         group n by n into pair
         select new { hex = pair.Key, count = pair.Count()};

        foreach(var pair in numberOfHexes)
        {
            float percent = pair.count/allHexes * 100;
            probDict.Add(pair.hex,percent);
            //Debug.Log("Hex: "+ pair.hex.Q +','+pair.hex.R+' '+ percent.ToString("0.00")+'%');
        }
        //przjeście po liście z pewniakami i ustawienie ich na 100%
        foreach (Hex hex in nextLavaTargets)
        {
            if (probDict.ContainsKey(hex)) 
            {
                probDict[hex] = 100.0f;
            }
            
        }
        return probDict;
    }

    public float GetTickTimer()
    {
        return timeBeforeTickRemaining;
    } 

    void UpdateDisplay()
    {
        display.SetProbabilityDisplay(GetProbability());
        display.SetTimer(GetTickTimer());
    }

    public void MakeHexesClickable(bool clickable, Mode mode = Mode.Regular)
    {
        hexesClickable = clickable;
        foreach (GameObject hex in hexes)
        {
            HexTile tile = hex.transform.GetComponent<HexTile>();
            tile.mode = mode;
            if (mode == Mode.Regular && DoesntHaveLava(hex))
            {
                tile.MakeClickable(clickable);
            }
            else if (mode == Mode.Rain  && !DoesntHaveLava(hex))
            {
                tile.MakeClickable(clickable);
            }

            else if (mode == Mode.Walls  && DoesntHaveLava(hex))
            {
                tile.MakeClickable(clickable);
            }
        }
    }
    public bool QualifyForPlayerBase(Hex h)
    {
        if (!DoesntHaveLava(h)) {Debug.Log("DoesHaveLava(h)");return false;}
        if (playerHexes.Contains(h)) {Debug.Log("basa");return false;}
        if ((-2<=h.Q+h.R) && (h.Q+h.R<=2)&& (h.Q<=2) && (h.R<=2)&& (h.Q>=-2) && (h.R>=-2)) {Debug.Log("wzgórza");return false;}
        if (IsNextToPlayerBase(h)) {Debug.Log("sąsiad");return false;}
        
        return true;
    }


    void End(){
        display.EndScreen.SetActive(true);    }

    public IEnumerator Rain()
    {
        display.SetRainDisplay(ChooseYoungestHex());
        eventFlag = true;
        //MakeHexesClickable(true, Mode.Rain);
        while(eventFlag)
        {
            yield return null;
        }

        display.RainDisplay.SetActive(false);

        //MakeHexesClickable(false, Mode.Regular);
    }

    public void ExecuteRain(Hex rawHex)
    {
        lavaFilledHexes.Remove(rawHex);
        GameObject hex = getHex(rawHex);
        Transform parentObject = hex.transform;
        Transform childObject = parentObject.GetChild(1);
        Destroy(childObject.gameObject);
        hex.GetComponent<HexTile>().ColorUp(new Color(165,42,42));
    }

    Hex ChooseYoungestHex()
    {
        return lavaFilledHexes[lavaFilledHexes.Count - 1];
    }

    public void SwitchEventFlag(bool x)
    {
        eventFlag = x;
    }

    public IEnumerator Disaster()
    {
        display.SetDisasterDisplay();
        eventFlag = true;
        //MakeHexesClickable(true, Mode.Rain);
        while(eventFlag)
        {
            yield return null;
        }
        display.DisasterDisplay.SetActive(false);
        secondFlag = false;
        //MakeHexesClickable(false, Mode.Regular);
    }
    public IEnumerator Hunger()
    {
        display.SetHungerDisplay();
        eventFlag = true;
        //MakeHexesClickable(true, Mode.Rain);
        while(eventFlag)
        {
            yield return null;
        }
        display.HungerDisplay.SetActive(false);
        secondFlag = false;
        //MakeHexesClickable(false, Mode.Regular);
    }
        public IEnumerator Rot()
    {
        display.SetRotDisplay();
        eventFlag = true;
        //MakeHexesClickable(true, Mode.Rain);
        while(eventFlag)
        {
            yield return null;
        }
        display.RotDisplay.SetActive(false);
        secondFlag = false;
        //MakeHexesClickable(false, Mode.Regular);
    }

        public IEnumerator Nothing()
    {
        display.SetNothingDisplay();
        eventFlag = true;
        //MakeHexesClickable(true, Mode.Rain);
        while(eventFlag)
        {
            yield return null;
        }
        display.NothingDisplay.SetActive(false);
        secondFlag = false;
        //MakeHexesClickable(false, Mode.Regular);
    }
    public IEnumerator Eruption()
    {
        display.SetEruptionDisplay(ChooseHexForLava());
        eventFlag = true;
        //MakeHexesClickable(true, Mode.Rain);
        while(eventFlag)
        {
            yield return null;
        }


        if (mainFlag == true)
        {
            secondFlag = true;
            mainFlag = false;
            StartRandomEventCoroutine();
        }
        else {secondFlag = false;}
        display.EruptionDisplay.SetActive(false);
        //MakeHexesClickable(false, Mode.Regular);
    }

    void StartRandomEventCoroutine()
    {
        if (currentRandomEvent == SpecialEvent.Eruption)
        {
            StartCoroutine(Eruption());
        }
        if (currentRandomEvent == SpecialEvent.Disaster)
        {
            StartCoroutine(Disaster());
        }
        if (currentRandomEvent == SpecialEvent.Rain)
        {
            StartCoroutine(Rain());
        }
        if (currentRandomEvent == SpecialEvent.Hunger)
        {
            StartCoroutine(Hunger());
        }
        if (currentRandomEvent == SpecialEvent.Nothing)
        {
            StartCoroutine(Nothing());
        }
        if (currentRandomEvent == SpecialEvent.Rot)
        {
            StartCoroutine(Rot());
        }
    }

    void LavaEruption()
    {
        Hex chosenHex = ChooseHexForLava();
        LavaToChosenHex(chosenHex);

    }
    public void LavaToChosenHex(Hex rawHex)
    {
        Debug.Log("LavaPour: " + rawHex.Q + ", " + rawHex.R);
        lavaFilledHexes.Add(rawHex);
        GameObject hex = getHex(rawHex);
        Transform parentObject = hex.transform;
        GameObject newObject = PrefabUtility.InstantiatePrefab(prefabLava, parentObject) as GameObject;
        RemoveFromBag(rawHex);
        AddHexesToBag();
        //AddNeighboursToBag(rawHex);
        if (playerHexes.Contains(rawHex))
        {
            End();
        }
    }
}
