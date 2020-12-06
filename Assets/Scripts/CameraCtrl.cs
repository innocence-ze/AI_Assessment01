using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform target;
    public float dis;

    Vector3 center = Vector3.zero;
    void Update()
    {
        var tCenter = Vector3.zero;
        foreach (Transform item in target)
        {
            tCenter += item.position;
        }
        tCenter /= target.childCount;

        center = Vector3.Lerp(center, tCenter, 0.1f);

        var targetPos = new Vector3(transform.position.x, center.y, transform.position.z);
        transform.rotation = Quaternion.LookRotation(center - targetPos);
        transform.position = center - transform.forward * dis;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(center, 0.5f);
    }
}
