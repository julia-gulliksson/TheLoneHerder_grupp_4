using UnityEngine;

public class Fence : MonoBehaviour, IFence
{
    public int fenceSide { get; set; }

    void Start()
    {
        try
        {
            fenceSide = transform.parent.GetComponent<FenceHealth>().side;
        }
        catch
        {
            Debug.LogWarning("Fence parent has no script");
            fenceSide = 0;
        }
    }

    public void WolfHit()
    {
        GameEventsManager.current.WolfFoundTarget(fenceSide);
    }

    public void WolfLost()
    {
        GameEventsManager.current.WolfLostTarget(fenceSide);
    }
}
