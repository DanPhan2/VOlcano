using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VolcanoEruption : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> HexBag= new List<GameObject>();
    public List<(int dq, int dr)> HexNeighbours = new List<(int dq, int dr)>{(1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1)};

    void Start()
    {
        HexBag = GameObject.FindGameObjectsWithTag("Hex").ToList();
        RemoveLavaFilled();

        //wrzuć do worka wszystkie sąsiadujące
    }

    // Update is called once per frame

void RemoveLavaFilled()
{
    List<GameObject> lavaFilled = new List<GameObject>();
        foreach (GameObject hex in HexBag)
        {
            if (hex.transform.childCount >1)
            {
                lavaFilled.Add(hex);
            }
        }
        foreach (GameObject doomedHex in lavaFilled)
        {
                HexBag.Remove(doomedHex);
        }
}

    void AddToBag()
    {
        List<GameObject> lavaFilled = new List<GameObject>();
        foreach (GameObject hex in HexBag)
        {
            if (hex.transform.childCount >1)
            {
                lavaFilled.Add(hex);
            }
        }
        foreach (GameObject lavaHex in lavaFilled)
        {
                String name = lavaHex.name;
                for (var neighbour in HexNeighbours)
                {

                }
        }
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
    void LavaToChosenHex(GameObject hex)
    {
        //dołóż lawę do heksa
        //usuń wszystkie tokeny tego pola z worka
    }

}
