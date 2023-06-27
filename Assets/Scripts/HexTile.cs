using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TileType {Field, Forest, Hill, Volcano}
public enum Mode {Regular, Rain, Walls}
public class HexTile : MonoBehaviour
{
    public Mode mode = Mode.Regular;

    public Color baseColor;

    public TileType type;

    public Hex hexSelf = new Hex(0,0);

    bool clickable = false;

    SpriteRenderer m_SpriteRenderer;

    private bool isNextLavaTarget = false;
    void Start()
    {
        m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
    }


    public void MakeClickable(bool x)
    {
        clickable = x;
    }
    private void OnMouseUp()
    {
        if(clickable)
        {
            m_SpriteRenderer.color += new Color(+0.2f,+0.2f,+0.2f,0);
        }
    }

    private void OnMouseDown()
    {
        if(clickable)
        {
            
            Debug.Log("Mouse Click Detected on "+ name+" during " + mode);
            m_SpriteRenderer.color += new Color(-0.2f,-0.2f,-0.2f,0);
            if (mode == Mode.Regular) SetAsNextLavaTarget();
            else if (mode == Mode.Rain) Rain();
            else if (mode == Mode.Walls) Walls();
        }
    }

    public void SetAsNextLavaTarget()
    {
        if (!isNextLavaTarget)
        {
            VolcanoEruption.SetNextLavaTarget(hexSelf);
        }
        isNextLavaTarget = true;
    }

    public void Rain()
    {
            //VolcanoEruption.SetNextLavaTarget(hexSelf);

    }

    public void Walls()
    {
            //VolcanoEruption.SetNextLavaTarget(hexSelf);

    }

    public void ColorUp(Color c)
    {
        m_SpriteRenderer.color = c;
    }

    public void ResetColor()
    {
        m_SpriteRenderer.color = baseColor;
    }


}
