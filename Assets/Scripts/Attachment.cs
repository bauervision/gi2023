using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Attachment
{
    public bool isAttached;
    public bool saved;
    public string name;

    // probably a link to the mesh

    public Attachment(string newAttachmentName)
    {
        this.isAttached = true;
        this.saved = false;
        this.name = newAttachmentName;
    }

}