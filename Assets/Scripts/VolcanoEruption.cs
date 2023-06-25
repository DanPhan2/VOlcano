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
    bool isRunning = true;

    List<Hex> nextLavaTargets = new List<Hex>();

    void Start()
    {   
        // HexBag = GameObject.FindGameObjectsWithTag("Hex").ToList();
        // RemoveLavaFilled();
        prefabLava = Resources.Load<Object>("Prefabs/Lava");
        HexMap.GenerateMap();
        hexes = GameObject.FindGameObjectsWithTag("Hex");
        LavaToChosenHex(new Hex(0, 0));
        //wrzuć do worka wszystkie sąsiadujące
    }

    // Update is called once per frame

    public void Update()
    {
        if (isRunning)
        {
            StartCoroutine(MyCoroutine());
        }
    }

    public IEnumerator MyCoroutine()
    {
        isRunning = false;
        yield return new WaitForSeconds(1);
        Hex chosenHex = ChooseHexForLava();
        LavaToChosenHex(chosenHex);
        isRunning = true;
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

        RemoveFromAllInstancesFromBag(rawHex);

        AddNeighboursToBag(rawHex);
    }

    void AddNeighboursToBag(Hex hex)
    {
        System.Random rnd = new System.Random();

        foreach ((int dq, int dr) neigh in hexNeighbours)
        {
            Hex neighHex = new Hex(hex.Q + neigh.dq, hex.R + neigh.dr);

            if (HexMap.IsInRange(neighHex) && DoesntHaveLava(neighHex)) {
                hexBag.Insert(rnd.Next(hexBag.Count), neighHex);
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

    void SetNextLavaTarget(Hex hex)
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

}
