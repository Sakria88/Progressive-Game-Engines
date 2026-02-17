using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
public class BackPack 
{
    //itemName to identify the type of item in the backpack, 
    // such as "Coin" or "Health Potion"
    public string itemName;

    //itemCount to keep track of the number of coins/items 
    // in the backpack
    public int itemCount;

    //A static variable is a variable that is shared by all
    //instances of a class (making it a global variabl
    //capacity is a static variable to define the maximum 
    // number of items the backpack can hold
    public static int capacity;
    public BackPack()
    {
        this.itemCount = 0;
        BackPack.capacity = 100; // Default capacity for all backpacks
    }
    public BackPack(int capacity)
    {
        this.itemCount = 0;
        BackPack.capacity = capacity;
    }
    public void AddToBackpack()
    {
        if (itemCount < BackPack.capacity)
            {
            this.itemCount++;
            Debug.Log("Added an item to the backpack. Current count: " + itemCount);
            }
        else
        {
        Debug.Log("Backpack if FULL!!!");
}
}

}