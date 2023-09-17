using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[CreateAssetMenu(menuName = "CreateBlockParamAsset")]
public class BlockParamAsset : ScriptableObject
{
    public int num;
    public string BlockName = "";
    public VideoClip BlockVideClip = null;
    public string BlockExplanation  = "";
}
