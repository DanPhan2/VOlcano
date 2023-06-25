using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VolcanoEruption : MonoBehaviour
{
    public List<GameObject> hexBag= new List<GameObject>();
    public List<(int dq, int dr)> hexNeighbours = new List<(int dq, int dr)>{(1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1)};
    Object prefabLava;
    Transform parentObject;
    bool isRunning = true;

    void Start()
    {
        // HexBag = GameObject.FindGameObjectsWithTag("Hex").ToList();
        // RemoveLavaFilled();
        prefabLava = Resources.Load<Object>("Prefabs/Lava");
        parentObject = GameObject.FindGameObjectsWithTag("Map")[0].transform;
        HexMap.GenerateMap();
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
        yield return new WaitForSeconds(5);
        Debug.Log(GameObject.FindGameObjectsWithTag("Hex")[0].name == "Hex -6,0");
        GameObject[] gameObject = GameObject.FindGameObjectsWithTag("Hex");
        Debug.Log(System.Array.IndexOf(GameObject.FindGameObjectsWithTag("Hex"), System.Array.Find(gameObject, element => element.name == "Hex 0,0")));
        int ran = Random.Range(-6, 7);
        Debug.Log(ran);
        isRunning = true;
    }

    void RemoveLavaFilled()
    {
        List<GameObject> lavaFilled = new List<GameObject>();
            foreach (GameObject hex in hexBag)
            {
                if (hex.transform.childCount >1)
                {
                    lavaFilled.Add(hex);
                }
            }
            foreach (GameObject doomedHex in lavaFilled)
            {
                    hexBag.Remove(doomedHex);
            }
    }

    void AddToBag()
    {
        List<GameObject> lavaFilled = new List<GameObject>();
        foreach (GameObject hex in hexBag)
        {
            if (hex.transform.childCount >1)
            {
                lavaFilled.Add(hex);
            }
        }
        foreach (GameObject lavaHex in lavaFilled)
        {
                System.String name = lavaHex.name;
                foreach (var neighbour in hexNeighbours)
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
