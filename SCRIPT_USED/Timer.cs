using UnityEngine;
public class Timer
{
    public float preset;
    private float time;
    public bool done;
    public void Update(bool input)
    {
        if (input){
            time += Time.deltaTime;
            if (time >= preset) done = true;
        }
        else {
            time = 0;
            done = false;
        }
    }
}