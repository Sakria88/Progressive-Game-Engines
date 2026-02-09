using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzBuzzHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SolveFizzBuzz(0, 100);
        

    }
    void SolveFizzBuzz(int start, int end)
    {
        Debug.Log("Starting Ascending Order:");
        int i = start;
        while (i <= end)
        {
            CheckFizzBuzz(i);
            i++;
        }
        Debug.Log("Starting Descending Order:");
        i = end;
        while (i >= start)
        {
            CheckFizzBuzz(i);
            i--;
        }
    }
        void CheckFizzBuzz(int i)
        {
            if (i % 3 == 0 && i % 5 == 0)
            {
                Debug.Log("FizzBuzz");
            }
            else if (i % 3 == 0)
            {
                Debug.Log("Fizz");
            }
            else if (i % 5 == 0)
            {
                Debug.Log("Buzz");
            }
            else
            {
                Debug.Log(i);
            }
    }


    void Update()
    {

    }
}
  