using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


public class VolcanoEruption : MonoBehaviour
{
    List<Hex> hexBag = new List<Hex>();
    List<Hex> lavaFilledHexes = new List<Hex>();
    List<(int dq, int dr)> hexNeighbours = new List<(int dq, int dr)>{(1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1)};
    GameObject[] hexes; 
    Object prefabLava;
    public Display display;
    //Timer logic
    bool isRunning = true;
    public float timeBetweenTicks;
    public float timeBeforeTickRemaining;

    static List<Hex> nextLavaTargets = new List<Hex>();

    //Tick related
    public bool flag = false;
    public GameObject tickDisplay;

    void Start()
    {   
        prefabLava = Resources.Load<Object>("Prefabs/Lava");
        HexMap.GenerateMap();
        hexes = GameObject.FindGameObjectsWithTag("Hex");

        LavaToChosenHex(new Hex(0, 0));

        timeBeforeTickRemaining = timeBetweenTicks;
    }

    // Update is called once per frame

    public void Update()
    {
        if (isRunning)
        {
            if (timeBeforeTickRemaining > 0)
            {
                timeBeforeTickRemaining -= Time.deltaTime;
                if (timeBeforeTickRemaining < 0)
                {
                    timeBeforeTickRemaining = 0.0f;
                }
                UpdateDisplay();
            }
            else
            {
                StartCoroutine(TickCoroutine());
            }
        }
    }

    public IEnumerator TickCoroutine()
    {
        flag = true;
        isRunning = false;
        tickDisplay.SetActive(true);
        while(flag)
        {
            yield return null;
        }

        isRunning = true;
        LavaEruption();
        tickDisplay.SetActive(false);
        timeBeforeTickRemaining = timeBetweenTicks;
    }

    public void SwitchFlag()
    {
        flag = false;
    }

    void LavaEruption()
    {
        Hex chosenHex = ChooseHexForLava();
        LavaToChosenHex(chosenHex);
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

    void LavaToChosenHex(Hex rawHex)
    {
        Debug.Log("LavaPour: " + rawHex.Q + ", " + rawHex.R);
        lavaFilledHexes.Add(rawHex);
        GameObject hex = getHex(rawHex);
        Transform parentObject = hex.transform;
        GameObject newObject = PrefabUtility.InstantiatePrefab(prefabLava, parentObject) as GameObject;
        RemoveFromBag(rawHex);
        AddHexesToBag();
        //AddNeighboursToBag(rawHex);
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
        // List<GameObject> lavaFilled = new List<GameObject>();
        // foreach (GameObject hex in hexBag)
        // {
        //     if (hex.transform.childCount >1)
        //     {
        //         lavaFilled.Add(hex);
        //     }
        // }
        // foreach (GameObject lavaHex in lavaFilled)
        // {
        //         System.String name = lavaHex.name;
        //         foreach (var neighbour in hexNeighbours)
        //         {

        //         }
        // }
        //weź wszystkie heksy
        //znajdź te zalane lawą
        //przeiteruj po sąsiadach
        //jeśli nie są zalani lawą dorzuć do worka token
    }

    void RemoveFromBag(Hex rawHex)
    {
        do {
            hexBag.Remove(rawHex);
        } while (hexBag.Remove(rawHex));
    }

    GameObject getHex(Hex hex)
    {
        return hexes[HexMap.GetHexIndex(hex.Q, hex.R)];
    }

    GameObject getHex(int q, int r)
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

    bool DoesntHaveLava(Hex hex)
    {
        return !lavaFilledHexes.Contains(hex);
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
            Debug.Log("Hex: "+ pair.hex.Q +','+pair.hex.R+' '+ percent.ToString("0.00")+'%');
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
}
