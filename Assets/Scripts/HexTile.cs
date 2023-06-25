using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileType {Field, Forest, Hill, Volcano}
public class HexTile : MonoBehaviour
{

    public TileType type;

    public Hex hexSelf = new Hex(0,0);

    private bool isNextLavaTarget = false;

    private void OnMouseUp()
    {
        SpriteRenderer m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color += new Color(+0.2f,+0.2f,+0.2f,0);
    }

    private void OnMouseDown()
    {
        SpriteRenderer m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
        Debug.Log("Mouse Click Detected on "+ name);
        m_SpriteRenderer.color += new Color(-0.2f,-0.2f,-0.2f,0);
    }

    public void SetAsNextLavaTarget()
    {
        if (!isNextLavaTarget)
        {
            VolcanoEruption.SetNextLavaTarget(hexSelf);
        }
        isNextLavaTarget = true;
    }

}
