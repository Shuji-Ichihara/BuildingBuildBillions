using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private DistanceJoint2D dis = null;

    Vector3 _PlayerScreenPos, _HookVec,_HookDirection;
    Quaternion _RotationPlayer;

    bool _Pressed = true;
    [SerializeField]
    GameObject hook = null;
    [SerializeField]
    readonly float distanceMax = 7;
    float distance=0;
    [SerializeField]
    float power = 0.5f;

    LineRenderer line;
    int posisionCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        dis = GetComponent<DistanceJoint2D>();
        line = gameObject.GetComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {//左クリック
         //Hookdis();
            if (_Pressed)
            {
                line.SetPosition(0, this.gameObject.transform.position);
                _Pressed = false;
                Direction();
                Hook();
            }
            Debug.Log("左クリック");
            if(Input.GetMouseButton(1))
            {
                Debug.Log("右クリック");
                if(distance>1)
                {
                    distance -= Time.deltaTime * 3;
                    dis.distance = distance;
                }
                
            }

            
            posisionCount++;
            line.positionCount = posisionCount;
            
            line.SetPosition(posisionCount - 1, hook.gameObject.transform.position);
        }
        else if (Input.GetMouseButtonUp(0))
        {//
            //StartCoroutine(HookReturn());
            _Pressed = true;
            Hookdis();

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Hook();
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Hookdis();
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(-3*Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(3*Time.deltaTime, 0, 0);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.up*10, ForceMode2D.Impulse);
        }
    }

    void Hook()
    {
        distance = distanceMax;
        dis.distance = distance;
        dis.enabled = true;
        hook.SetActive(true);
        hook.GetComponent<Rigidbody2D>().AddForce(_HookDirection*power,ForceMode2D.Impulse);
        //Debug.Log(_HookVec);
    }

    IEnumerator HookReturn() 
    {
        while (distance>1)
        {
            distance-=Time.deltaTime*3;
            dis.distance = distance;
            yield return null;
        }
        //Hookdis();
    }

    void Hookdis()
    {
        hook.SetActive(false);
        line.positionCount = 0;
        dis.enabled = false;
    }

    void Direction()
    {//進む方向決め
        _PlayerScreenPos = Camera.main.WorldToScreenPoint(transform.localPosition);

        _RotationPlayer = Quaternion.LookRotation
            (Vector3.forward, Input.mousePosition - _PlayerScreenPos);

         _HookVec= _RotationPlayer.eulerAngles;
        _HookDirection = Quaternion.Euler(_HookVec) * Vector3.up;
    }

}
