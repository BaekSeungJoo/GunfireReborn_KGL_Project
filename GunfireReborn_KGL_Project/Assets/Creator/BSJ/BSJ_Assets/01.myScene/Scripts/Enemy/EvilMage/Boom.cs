using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

public class Boom : MonoBehaviour
{

    public Transform m_Target;
    public float m_InitialAngle = 30f; // ó�� ���󰡴� ����

    private EvilMage evilMage;        // �θ� ���� Ŭ����
    private Vector3 startPos;         // ������ �ʱ� ��ġ
    private Rigidbody m_Rigidbody;    // Rigidbody

    private void Awake()
    {
        evilMage = GetComponentInParent<EvilMage>();
    }

    private void OnEnable()
    {
        m_Target = evilMage._TargetPlayer.transform;                                                // Ÿ�� �÷��̾�
        m_Rigidbody = GetComponent<Rigidbody>();                                                    // źȯ rigidbody
        Vector3 velocity = GetVelocity(transform.position, m_Target.position, m_InitialAngle);      // ������ � �Լ�
        m_Rigidbody.velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� �ڵ� => ����Ʈ�� ������Ʈ Ǯ�� �־ ����� ����
        // Instantiate(boomVFX, transform.position, Quaternion.identity);
        // Destroy(transform.gameObject);

        // ���� �ڵ�
        // ����Ʈ Ȱ��ȭ
        evilMage.Active_BombEffect(gameObject.transform.position);
        // ���ӿ�����Ʈ(źȯ) ��Ȱ��ȭ
        this.gameObject.SetActive(false);
    }

    public Vector3 GetVelocity(Vector3 startPos, Vector3 target, float initialAngle)
    {
        // Unity ���� ������Ʈ�� �������� �߷��� ũ�⸦ ��Ÿ���� ��. 9.81
        float gravity = Physics.gravity.magnitude;

        // ó�� ���ư��� ������ �������� ����
        float angle = initialAngle * Mathf.Deg2Rad;

        // Ÿ�� ��ġ
        Vector3 targetPos = new Vector3(target.x, 0, target.z);

        // ó�� �߻� ��ġ
        Vector3 shotPos = new Vector3(startPos.x, 0, startPos.z);

        // �Ÿ� ���ϱ�
        float distance = Vector3.Distance(targetPos, shotPos);

        // ���� ���� ��� (�ʱ� �ӵ� ��� �� �߷��� ������ �ݿ��ϱ� ����)
        float yOffset = startPos.y - target.y;

        // { �߻�ü�� �ʱ� �ӵ��� ����ϴ� �ֿ� ����
        // �Ÿ��� ������ ��� : Mathf.Pow(distance, 2)
        // �Ÿ� ������ �߷��� ������ ���Ѵ�. �� ���� �߻� ������ ���� ź��Ʈ �� ���� ���� yOffset���� ����
        // �߻�ü�� �߻� ������ ���� �ڻ��� : (1 / Mathf.Cos(angle))
        // Mathf.Cos(angle) : �� ������ ���Ѵ�. �� �κ��� �߻� ������ ������ ����
        // Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
        
        float initialVelocity
            = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
        
        // } �߻�ü�� �ʱ� �ӵ��� ����ϴ� �ֿ� ����


        // �ʱ� �ӵ��� ����Ͽ� 3D �������� �߻�ü�� �ӵ� ���� ���
        // y ������ �ʱ�ӵ��� �߻簢��(����)�� ���� ������ �����Ͽ� �߻�ü�� �������� �ö󰬴ٰ� �ٽ� �Ʒ��� �������� � ��� ��Ÿ��
        // z ������ �ʱ�ӵ��� �߻簢��(����)�� �ڻ��� ������ �����Ͽ� �߻�ü�� ���� �������� �̵��ϴµ� ����.
        Vector3 velocity
            = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Vector3.Angle(Vector3.forward, planarTarget - planarPosition)
        // �� �κ��� ���� ��ġ���� ��ǥ ��ġ������ ���Ϳ� ���� ���� ���� ������ ���
        // (target.x > player.x ? 1 : -1)
        // �� �κ��� ��ǥ ��ġ�� ���� ��ġ�� x ��ǥ ���� ���Ͽ� ��ǥ�� ���� ��ġ�� �����ʿ� �ִ��� ���ʿ� �ִ��� �Ǵ�
        // �׿� ���� ������ ��� �Ǵ� ������ ������. �� ���� ȸ�� ������ �����ϴµ� ���
        float angleBetweenObjects
            = Vector3.Angle(Vector3.forward, targetPos - shotPos) * (target.x > startPos.x ? 1 : -1);


        // �ʱ� �ӵ� ���͸� angleBetweenObjects������ŭ Vector3.up �� ������ ȸ����Ű�� ���� ��Ÿ��
        // Quaternion.AngleAxis(angleBetweenObjects, Vector3.up)
        // �� �κ��� angleBetweenObjects ������ �������� Vector3.up �� ������ ȸ���ϴ� ���ʹϾ��� ����
        // angleBetweenObjects�� �ռ� ���� ���� ��ġ���� ��ǥ ��ġ ������ ������ ��Ÿ����, Vector3.up�� y���� �������� ȸ���� ��Ÿ��
        // * velocity �ʱ�ӵ����͸� velocity�� �����Ͽ�, �ʱ� �ӵ� ���͸� �ش� ���� angleBetweenObjects ��ŭ ȸ��
        // �̷��� ȸ�� ��Ų ���Ͱ� �����ӵ��� �ȴ�.
        // ��� ������ finalVelocity������ �ʱ� �ӵ��� ���� ��ġ���� ��ǥ ��ġ������ ����angleBetweenObjects�� �����Ͽ�
        // ȸ���� �ӵ� ���Ͱ� ����ȴ�.
        // �� �ӵ� ���͸� ����Ͽ� �߻�ü�� ���� �����ϵ��� �����ȴ�.

        Vector3 finalVelocity
            = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }

}