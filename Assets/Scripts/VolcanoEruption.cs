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
        int x = Random.Range(-6, 7);
        int y = Random.Range(-6, 7);
        Debug.Log(x + " " + y);
        if (HexMap.IsInRange(x, y)) {
            Transform parentObject = hexes[HexMap.GetHexIndex(x, y)].transform;
            GameObject newObject = PrefabUtility.InstantiatePrefab(prefabLava, parentObject) as GameObject; 
        }
        isRunning = true;
    }

    void RemoveLavaFilled()
    {
        // List<GameObject> lavaFilled = new List<GameObject>();
        //     foreach (GameObject hex in hexBag)
        //     {
        //         if (hex.transform.childCount >1)
        //         {
        //             lavaFilled.Add(hex);
        //         }
        //     }
        //     foreach (GameObject doomedHex in lavaFilled)
        //     {
        //             hexBag.Remove(doomedHex);
        //     }
    }

    void AddNeighboursToBag(Hex hex)
    {
        foreach ((int dq, int dr) neigh in hexNeighbours)
        {
            Hex neighHex = new Hex(hex.Q + dq, hex.R + dr);

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
        
    GameObject DrawFromBag()
    {
    //losowy element z worka
        return null;
    }

    void LavaToChosenHex(Hex rawHex)
    {
        lavaFilledHexes.Add(rawHex);
        GameObject hex = getHex(rawHex);
        Transform parentObject = hex.transform;
        GameObject newObject = PrefabUtility.InstantiatePrefab(prefabLava, parentObject) as GameObject;

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
        //losowanie hexa z hexbaga
        Hex hex = new Hex(0,0);
        return hex;
    }

    bool IsNextToLava(Hex hex)
    {
        bool lavaNeighbour = false;
        //iteracja po sąsiadach - czy którykolwiek ma lawę
        return lavaNeighbour;
    }

    bool DoesntHaveLava(Hex hex)
    {
        return lavaFilledHexes.Contains(hex);
    }

}
