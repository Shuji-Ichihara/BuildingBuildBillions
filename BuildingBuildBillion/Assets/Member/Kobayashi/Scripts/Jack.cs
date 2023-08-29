using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Jack : MonoBehaviour
{
    [SerializeField] private List<Sprite> _jackSprite = new List<Sprite>();//�W���b�L�̉摜�ۊǗp\
    [SerializeField] private float _maxAllowedAngleUp = 5f;
    [SerializeField] private float _maxAllowedAngleDown = 175f;
    private SpriteRenderer _mainSprite;//�I�u�W�F�N�g��SpriteRenderer�Q�Ɨp
    private Animator _jackAnim;
    private bool isOne = false;
    private bool isUp = false;
    private bool isDown = false;
   
    private void Awake()
    {
        _mainSprite = this.gameObject.GetComponent<SpriteRenderer>();//SpriterRenderer�i�[
        _jackAnim = this.gameObject.GetComponent<Animator>();
       _mainSprite.sprite = _jackSprite[0];
    }
    private void FixedUpdate()
    {
        if(isUp == true && isDown == true && isOne == false)
        {
            JackingStart();
            isOne= true;
        }

    }
    public void UpEventHandler(Collider2D collision)
    {
        Vector2 contactNormal = collision.GetComponent<Collider2D>().ClosestPoint(transform.position) - (Vector2)transform.position;

        // �u���b�N�̖@���x�N�g���Ƃ̊p�x���v�Z
        float angle = Vector2.Angle(Vector2.up, contactNormal);

        // �p�x�����͈͓̔��ɂȂ��ꍇ�͏������X�L�b�v
        if (angle > _maxAllowedAngleUp)
        {
            isUp = false;
            return;
        }
        // �p�x�����e�͈͓��̏ꍇ�̏���
        isUp = true;
    }
    public void DownEventHandler(Collider2D collision)
    {
        Vector2 contactNormal = collision.GetComponent<Collider2D>().ClosestPoint(transform.position) - (Vector2)transform.position;

        // �u���b�N�̖@���x�N�g���Ƃ̊p�x���v�Z
        float angle = Vector2.Angle(Vector2.up, contactNormal);

        Debug.Log(angle);
        // �p�x�����͈͓̔��ɂȂ��ꍇ�͏������X�L�b�v
        if (angle < _maxAllowedAngleDown)
        {
            isDown = false;
            return;
        }
        // �p�x�����e�͈͓��̏ꍇ�̏���
        isDown = true;
    }

    public void ExitUpEventHandler()
    {
        isUp = false;
    }
    public void ExitDownEventHandler()
    {
        isDown = false;
    }
    /// <summary>
    /// �W���b�L���L���鏈��
    /// </summary>
    public async void JackingStart()
    {
        await Task.Delay(500);
        _jackAnim.SetTrigger("JackActive");
        _mainSprite.sprite = _jackSprite[1];
        await Task.Delay(500);
        _mainSprite.sprite = _jackSprite[2];
    }
}
