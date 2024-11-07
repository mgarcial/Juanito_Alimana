using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour
{
    public Transform boss;
    public Transform targetPosition;
    [SerializeField] private float jumpForce = 100f;
    public GameObject PedroNavaja;
    public float zMoveDistance = 5f;
    public float zMoveDuration = 1f;
    [SerializeField] private CinemachineVirtualCamera bossCamera;

    [SerializeField] Collider2D bossCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            {
            bossCamera.Priority = 20;
            StartCoroutine(Jump());
            AudioManager.GetInstance().PlayBossMusic();            
        }
    }
    public IEnumerator Jump()
    {
        Debug.Log("memato");
        Vector3 jumpDirection = (targetPosition.position - boss.position).normalized + Vector3.up;
        Rigidbody2D bossRb = boss.GetComponent<Rigidbody2D>();
        bossRb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        yield return StartCoroutine(MoveOnZ());

    }

    public IEnumerator MoveOnZ()
    {
        float elapsedTime = 0;
        Vector3 startZPosition = boss.position;
        Vector3 targetZPosition = startZPosition + new Vector3(0, 0, zMoveDistance);

        while (elapsedTime < zMoveDuration)
        {
            boss.position = Vector3.Lerp(startZPosition, targetZPosition, elapsedTime / zMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        bossCollider.isTrigger = true;
        PedroNavaja.SetActive(true);
    }
}
