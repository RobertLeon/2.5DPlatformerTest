//Created by Robert Bryant
//
//Item Table
using UnityEngine;


[CreateAssetMenu(fileName = "Loot Table", menuName = "Items/LootTable", order = 1)]
public class ItemTable : ScriptableObject
{ 
    public Loot[] itemTable;                //Items and their weight

    private int totalWeight;                //Total weight of the item table
    private int itemDrop;                   //Number used for picking the item dropped


    //Picks an item from the item table
    public Items DropItem()
    {
        //
        totalWeight = GetWeight(itemTable);

        //Random number for the starting point
        itemDrop = Random.Range(0, totalWeight);

        //Item being picked
        Items randomItem = null;

        //Loop through each item in the table
        foreach(Loot item in itemTable)
        {
            //Pick the current item
            if (itemDrop < item.weight)
            {
                randomItem = item.drop;
                break;
            }
            //Lower the number by the item's weight
            itemDrop -= item.weight;
        }
        
        return randomItem;
    }

    //Gets the total weight of the loot table
    private int GetWeight(Loot[] table)
    {
        int tableWeight = 0;

        //Add each item's weight to the total
        foreach (Loot item in table)
        {
            tableWeight += item.weight;
        }

        return tableWeight;
    }

}

[System.Serializable]
public class Loot
{
    public Items drop;          //Item to drop
    public int weight;          //Item's weight in the table
    
    //Constructor
    public Loot(Items drop, int weight)
    {
        this.drop = drop;
        this.weight = weight;
    }
}
