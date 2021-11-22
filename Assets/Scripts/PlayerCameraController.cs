using Mirror;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    [SerializeField] GameObject cam;

    public override void OnStartAuthority()
    {
        cam.SetActive(true);
    }
}
