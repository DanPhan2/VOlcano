using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex{
    public readonly int Q; // x
    public readonly int R;  // y

    public Hex (int q, int r){
        this.Q = q;
        this.R = r;
    }

    public Vector2 Position(Vector2 size){
        float radius = size.x/2;
        //float y_1 = size.y * 0.6f;

        float x = radius * (1.5f) * this.Q ;
        float y = radius * (Mathf.Sqrt(3)/2 * this.Q  +  Mathf.Sqrt(3) * this.R);

        return new Vector2(x, y);
    }

    // override object.Equals
    public override bool Equals(object obj)
    {       
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return Equals(obj);
    }
    
    public override int GetHashCode()
    {
        return Q;
    }

    public bool Equals(Hex other)
    {
        if (other == null) return false;
        return (this.Q == other.Q && this.R == other.R);
    }
}