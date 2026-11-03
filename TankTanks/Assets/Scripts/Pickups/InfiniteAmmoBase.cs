using UnityEngine;
using System.Collections;



public class InfiniteAmmoBase : PickUpBase
{
    [SerializeField] private float duration = 10f;

    private static Coroutine activeCoroutine;
    UniversalPowerupBar PowerupBar;

    protected override void ApplyEffect(GameObject Player) //This is currently not working!!!!!!!!
    {
        PowerupBar.StartTimer(duration, Color.yellow); 

        HitScanFinal gun = Player.GetComponent<HitScanFinal>();
        Debug.Log(gun);

        if (gun == null)
            return;

        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        activeCoroutine = StartCoroutine(InfiniteAmmoRoutine(gun));
    }

    private IEnumerator InfiniteAmmoRoutine(HitScanFinal gun)
    {
        float originalHeatPerShot = gun.HeatPerShot;

        gun.HeatPerShot = 0f;

        yield return new WaitForSeconds(duration);

        gun.HeatPerShot = originalHeatPerShot;

        activeCoroutine = null;
    }
}
