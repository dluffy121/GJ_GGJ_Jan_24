using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    EProjectileType m_projectileType;
    [SerializeField]
    float m_projectileSpeed;
    [SerializeField]
    float m_rotationSpeed;
    [SerializeField]
    Vector3 m_throwDirection;
    [SerializeField]
    int m_projectileScore;
    [SerializeField]
    Rigidbody2D m_projectileRigidbody;
    [SerializeField]
    AudioClip m_moneyCollect;


    private float m_randomProjectile;

    public float MaxAngle { get; set; }
    public float MinAngle { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        m_randomProjectile = Random.Range(MinAngle, MaxAngle);
        AddStartingForce();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(m_randomProjectile, m_throwDirection.y, 0) * m_projectileSpeed * Time.deltaTime;
        Vector3 l_rotation = transform.localEulerAngles;
        l_rotation.z += Time.deltaTime * m_rotationSpeed;
        transform.localEulerAngles = l_rotation;
    }

    private void AddStartingForce()
    {
        m_projectileRigidbody.AddForce(new Vector2(m_randomProjectile, m_throwDirection.y) * m_projectileSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform.CompareTag("Platform"))
        {
            if(!m_projectileType.Equals(EProjectileType.Obstacle))
                GameEvents.OnDropProjectile?.Invoke();
            Destroy(this.gameObject);
        }
        else if (collider.transform.CompareTag("Thela"))
        {         
            GameEvents.updateScore?.Invoke(m_projectileScore,collider.transform.position);
            gameObject.GetComponent<Collider2D>().enabled = false;
            SoundManager.PlaySoundEffect(m_moneyCollect);
            Destroy(this.gameObject);
        }
        else if(collider.transform.CompareTag("Player"))
        {
            GameEvents.OnPlayerStun?.Invoke(collider.transform.parent.name);
        }
    }
}
