using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEditor;
 using TMPro;

public class HexMap : MonoBehaviour
{

 // Use this for initialization
 [MenuItem("Haha/2D Object/CreateGameObjects")]


public static void GenerateMap()
{
    Transform parentObject =  GameObject.FindGameObjectsWithTag("Map")[0].transform;
    Object prefab = Resources.Load<Object>("Prefabs/Hex");
    int hex_radius = 6;
    for (int q = -hex_radius; q <= hex_radius; q++)
    {
        for (int r = -hex_radius; r <= hex_radius; r++)
        {
            if ((-hex_radius<=q+r) && (q+r<=hex_radius))
            {
            // Instantiate a Hex
            Hex h = new Hex(q, r);

            GameObject newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject; 
                newObject.transform.SetParent(parentObject);
                newObject.transform.position = h.Position(newObject.transform.GetComponent<RectTransform>().sizeDelta);
                
                newObject.name = "Hex " +q +',' +r;
                newObject.transform.GetComponentInChildren<TextMeshPro>().text = q.ToString() +','+r.ToString();
            Debug.Log(q + "," + r);
            SpriteRenderer m_SpriteRenderer = newObject.transform.GetComponent<SpriteRenderer>();

            if ((0<=q+r) && (q+r<=0) && (q<=0) && (r<=0)&& (q>=0) && (r>=0)){
                m_SpriteRenderer.color = Color.red;
            }
            else if ((-2<=q+r) && (q+r<=2)&& (q<=2) && (r<=2)&& (q>=-2) && (r>=-2)){
                m_SpriteRenderer.color = Color.gray;
            }
            else if ((-4<=q+r) && (q+r<=4)&& (q<=4) && (r<=4)&& (q>=-4) && (r>=-4)){
                m_SpriteRenderer.color = Color.green;
            }
            else if ((-6<=q+r) && (q+r<=6)&& (q<=6) && (r<=6)&& (q>=-6) && (r>=-6)){
                m_SpriteRenderer.color = Color.yellow;
            }
            }
        }
    }
}
}