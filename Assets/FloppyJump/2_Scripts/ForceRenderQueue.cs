using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class ForceRenderQueue : MonoBehaviour
{
    public enum UsedQueue { Background = 1000, Geometry = 2000, AlphaTests = 2450, Transparent = 3000, Overlay = 4000 };
    public UsedQueue baseQueue = UsedQueue.Background;
    public int renderQueueIndex = 0;

    
}
