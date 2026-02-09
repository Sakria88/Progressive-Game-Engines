using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzBuzz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Ascending Order 
        for (int i = 0; i <= 100; i++)
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
        //Descending Order
        for (int i = 0; i >= 100; i++)
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



    }
  

//public class FizzBuzzHandler : MonoBehaviour
//{
//    void Start()
//    {
//        // Calling the method to execute the logic
//        RunFizzBuzzSequence(0, 100);
//    }

//    /// <summary>
//    /// This method iterates through a range of numbers and logs "Fizz", "Buzz", 
//    /// or "FizzBuzz" based on divisibility by 3 and 5.
//    /// </summary>
//    /// <param name="start">The number to begin at.</param>
//    /// <param name="end">The number to end at.</param>
//    void RunFizzBuzzSequence(int start, int end)
//    {
//        // --- Ascending Order Logic ---
//        Debug.Log("Starting Ascending Order:");

//        for (int i = start; i <= end; i++)
//        {
//            // Call our helper function to handle the FizzBuzz logic for a single number
//            CheckFizzBuzz(i);
//        }

//        // --- Descending Order Logic ---
//        Debug.Log("Starting Descending Order:");

//        for (int i = end; i >= start; i--)
//        {
//            // Reusing the logic for the countdown
//            CheckFizzBuzz(i);
//        }
//    }

//    /// <summary>
//    /// Determines if a specific number is Fizz, Buzz, or FizzBuzz and logs it.
//    /// </summary>
//    void CheckFizzBuzz(int i)
//    {
//        // Check for common multiples of 3 and 5 first
//        if (i % 3 == 0 && i % 5 == 0)
//        {
//            Debug.Log("FizzBuzz");
//        }
//        // Check if divisible by 3
//        else if (i % 3 == 0)
//        {
//            Debug.Log("Fizz");
//        }
//        // Check if divisible by 5
//        else if (i % 5 == 0)
//        {
//            Debug.Log("Buzz");
//        }
//        // If none of the above, just print the number
//        else
//        {
//            Debug.Log(i);
//        }
//    }
//}

void Update()
    {
        
    }
}
