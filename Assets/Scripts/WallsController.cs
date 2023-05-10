using UnityEngine;

public class WallsController : MonoBehaviour
{
    private const string BulletTag = "Bullet";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(BulletTag))
            other.gameObject.SetActive(false);
    }
}
