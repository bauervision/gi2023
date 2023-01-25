using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collectibles : IEnumerable<Collectible>
{

    public List<Collectible> myCollection;

    public Collectibles()
    {
        List<Collectible> myCollection = new List<Collectible>();
    }

    public IEnumerator<Collectible> GetEnumerator()
    {
        return myCollection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return myCollection.GetEnumerator();
    }

}