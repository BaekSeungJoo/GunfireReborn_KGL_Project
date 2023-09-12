using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float destroyTime = 1.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private Vector3 offset;

    private TextMeshProUGUI damageText;
    private float destroyTimer = 0f;

    private PhotonView photonView;

    private void Awake()
    {
        damageText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        photonView = GetComponent<PhotonView>();

        transform.localPosition += offset;
    }

    private void Update()
    {
        // 텍스트가 위로 움직임
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 데미지 텍스트 자동 파괴
        destroyTimer += Time.deltaTime;
        if (destroyTimer > destroyTime)
        {
            photonView.RPC("DestroyDmgText", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DestroyDmgText()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public void SetDamageText(int _damage, Color _color)
    {
        damageText.text = _damage.ToString();
        damageText.color = _color;
    }
}
