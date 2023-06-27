using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Preparation : MonoBehaviour
{
    // Start is called before the first frame update
    public int numberOfPlayers = 0;
    public string[] colorNames = {"Biały", "Fioletowy", "Niebieski", "Różowy"};
    public Color[] colorValues = {};//uzupełniane w edytorze
    public int currentPlayer = 0;
    bool activeChoice = false;

    VolcanoEruption mainScript;

    public TMP_Text chooseText;

    List<(int dq, int dr)> mouseNeighbours = new List<(int dq, int dr)>{(1, 0), (1, -1)};
    List<HexTile> clump = new List<HexTile> ();
    void Start()
    {
        
        colorNames = new string[4] {"Biały", "Fioletowy", "Niebieski", "Różowy"};
        mainScript = transform.GetComponent<VolcanoEruption>();
        //mainScript.MakeHexesClickable(true);
    }
    public void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            int index = mainScript.hexNeighbours.FindIndex(a => a == (mouseNeighbours[1]));
            int _index = index + 1;
            if (_index >=6) {_index = 0;}
            mouseNeighbours = new List<(int dq, int dr)>(){mainScript.hexNeighbours[index],mainScript.hexNeighbours[_index]};
        }
        if(clump.Count > 0)
        {            
            foreach(var hx in clump)
            {
                hx.ResetColor();
            }
            clump.Clear();
        }
        if (activeChoice)
        {
            //jak najedzie się na obiekt, on i dwaj sąsiedzi wypełniają się kolorem
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null) {
                //Debug.Log(hit.collider.gameObject.name);
                HexTile hexTile = hit.transform.gameObject.GetComponent<HexTile>();
                
                if (hexTile != null)
                {
                    (int dq, int dr) n1 = mouseNeighbours[0];
                    (int dq, int dr) n2 = mouseNeighbours[1];
                    Hex[] clumpedHexes = {hexTile.hexSelf, new Hex(hexTile.hexSelf.Q+n1.dq, hexTile.hexSelf.R+n1.dr), new Hex(hexTile.hexSelf.Q+n2.dq, hexTile.hexSelf.R+n2.dr)};
                        //nie ma lawy, nie ma innej bazy i nie jest w sąsiedztwie innej bazy i nie wzgórza
                    bool candidate = true;
                    foreach (Hex h in clumpedHexes)
                    {
                        if (HexMap.IsInRange(h.Q, h.R) && mainScript.QualifyForPlayerBase(h))
                        {
                            HexTile tile = mainScript.getHex(h).GetComponent<HexTile>(); 
                            tile.ColorUp(colorValues[currentPlayer-1]);
                            clump.Add(tile);
                        }
                        else {candidate  = false;}
                    }
                    
                    if (candidate == true&& Input.GetMouseButtonDown(0)) 
                    {
                        foreach (Hex h in clumpedHexes)
                        {
                            mainScript.playerHexes.Add(h);
                            clump.Clear();
                            activeChoice = false;
                        }
                    }
                    
                }
            }

            
            else {

            }

        }
        else
        {
            StartChoosing();
        }
    }
    public void SetNumberOfPlayers(int nr)
    {
        numberOfPlayers = nr;
        currentPlayer = 1;
    }

    public void StartChoosing()
    {
        StartCoroutine(ChoosePosition());
    }

    public IEnumerator ChoosePosition()
    {
        activeChoice = true;
        //"Gracz 1 ("+colorNames[currentPlayer-1]+")" 
        chooseText.text = "Gracz "+currentPlayer+" ("+colorNames[currentPlayer-1]+")";
        //tickDisplay.SetActive(true); 
        while(activeChoice)
        {
            yield return null;
        }
        currentPlayer++;
        if (currentPlayer > numberOfPlayers)
        {
            mainScript.display.PreparationDisplay.SetActive(false);
            mainScript.display.Control.SetActive(true);
            GetComponent<Preparation>().enabled = false;

        }
        //tickDisplay.SetActive(false);
    }

    public void SwitchActiveChoice()
    {
        activeChoice = false;
    }


}
