using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    CircleCollider2D _bombCollider;
    Rigidbody2D _rb;
    Animator _animator;

    [SerializeField]
    GameObject colObj;
    [SerializeField, Header("�{���̔����͈̔�")]
    private int bombRadius = 0;
    [SerializeField, Header("�{���̔����̍L����X�s�[�h")]
    private int bombSpeed = 0;
    [SerializeField, Header("�{���̐�����΂��p���[")]
    private int power = 0;

    // Start is called before the first frame update
    void Start()
    {
        _bombCollider = colObj.GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            BombEvent();
        }
        */
    }

    void BombEvent()
    {//�A�j���[�V��������Ăяo��

        StartCoroutine(BombCor());
        GetComponent<SpriteRenderer>().enabled = false;
    }


    public void AnimStart()
    {
        _animator.SetBool("BombBool", true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {//�R���C�_�[�ʂ蔲�������΂�

        if (col.gameObject.GetComponent<NewBuildingcon>() != null)
        {
            if (col.gameObject.GetComponent<NewBuildingcon>().isopareton == false)
            {//���ނ����쒆���ǂ����̔��f

                //������΂�����
                Vector3 hit;
                hit = col.ClosestPoint(this.transform.position);//trigger�Փˈʒu
                hit = transform.InverseTransformPoint(hit);//�G�ꂽ�I�u�W�F�N�g�̃��[�J�����W
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(hit * power, ForceMode2D.Impulse);
                SoundManager.Instance.PlaySE(SESoundData.SE.ExplosionBomb);
            }
        }
    }

    IEnumerator BombCor()
    {//�����̈�A�̏���

        _bombCollider.enabled = false;
        _bombCollider.enabled = true;
        _bombCollider.isTrigger = true;

        float count = 0;
        float time = 1f / bombSpeed;

        _rb.bodyType = RigidbodyType2D.Static;

        while (count < time)
        {
            if (GameManager.Instance.IsEndedGame == true) 
            { 
                enabled = false;
                break;
            }
            _bombCollider.radius += Time.deltaTime * bombSpeed * bombRadius;
            count += Time.deltaTime;
            yield return null;
        }
        _bombCollider.enabled = false;
    }
}
