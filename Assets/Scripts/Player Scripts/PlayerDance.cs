using UnityEngine;
using StarterAssets;

public class PlayerDanceLock : MonoBehaviour
{
    private ThirdPersonController playerController;

    private void Awake()
    {
        playerController = GetComponent<ThirdPersonController>();
    }

    public void SetDancing(bool dancing)
    {
        playerController.enabled = !dancing;
    }
}