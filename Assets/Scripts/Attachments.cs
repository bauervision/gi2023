using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attachments : IEnumerable<Attachment>
{
    public List<Attachment> myAttachments;

    public Attachments()
    {
        List<Attachment> myAttachments = new List<Attachment>();
    }

    public IEnumerator<Attachment> GetEnumerator()
    {
        return myAttachments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return myAttachments.GetEnumerator();
    }

}