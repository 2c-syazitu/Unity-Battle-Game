using UnityEngine;

public class BuffEffect
{
    Character.StatasType type;
    Buff.BuffCalculationType calcType;
    float value;

    public BuffEffect(Character.StatasType type, Buff.BuffCalculationType calcType, float value)
    {
        this.type = type;
        this.calcType = calcType;
        this.value = value;
    }

    public Character.StatasType getBuffType()
    {
        return type;
    }

    public Buff.BuffCalculationType getBuffCalculationType()
    {
        return calcType;
    }

    public float getValue()
    {
        return value;
    }

}
