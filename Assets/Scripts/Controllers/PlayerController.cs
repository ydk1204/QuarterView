using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;
    float _yAngle = 0.0f;

    Vector3 _destPos;
    // Start is called before the first frame update
    void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        //Managers.Resource.Instantiate("UI/UI_Button");
        //Managers.UI.ShowPopupUI<UI_Button>();

        Managers.UI.ShowSceneUI<UI_Inven>();
    }

    public enum PlayerState
    {
        Die,
        Moving,
        idle,
        Channeling,
        Jumping,
        Falling,
    }

    PlayerState _state = PlayerState.idle;

    void UpdateDie()
    {

    }

    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            transform.LookAt(_destPos);
        }

        // 애니메이션
        Animator anim = GetComponent<Animator>();
        //현재 게임 상태에 대한 정보를 넘겨준다.
        anim.SetFloat("speed", _speed);
    }

    void Updateidle()
    {
        //애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.idle:
                Updateidle();
                break;
        }
    }


        void OnMouseClicked(Define.MouseEvent evt)
        {
            if (_state == PlayerState.Die)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
            {
                _destPos = hit.point;
                _state = PlayerState.Moving;
                //Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
            }
        }
    }
