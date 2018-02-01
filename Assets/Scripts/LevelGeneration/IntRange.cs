using System;

[Serializable]
public class IntRange
{

    public int minimum;         //Minimum value in the range
    public int maximum;         //Maximum value in the range

    //Constructor
	public IntRange(int min, int max)
    {
        minimum = min;
        maximum = max;
    }

    //Gets a random value from the range.
    public int Random
    {
        get { return UnityEngine.Random.Range(minimum, maximum); }
    }
}