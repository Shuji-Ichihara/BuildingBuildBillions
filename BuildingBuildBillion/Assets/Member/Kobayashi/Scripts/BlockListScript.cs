using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockInfoDict
{
    public string BlackName;
    public BlockParamAsset blockParamAsset;
}

public class BlockListScript : MonoBehaviour
{   
    public List<BlockInfoDict> list = new List<BlockInfoDict>();
    
}
