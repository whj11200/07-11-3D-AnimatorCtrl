using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 1. �ڱ� �ڽ�(Touch Pad) 2. position ���� ��ŸƮ ���������� ���Ѵ�.
// 3. �е� ��ư�� �������� �ľ�
// 4. ����� ��ġ�� ���ȿ� ��ġ �Ǵ��� �ȵǴ��� ���� ���������� �Ǵ�
public class TouchPad : MonoBehaviour
{
    [SerializeField]
    [Tooltip("��ġ�е�������")]
    private RectTransform _touchPad;
    [SerializeField]
    private Vector3 _StartPos;
    [SerializeField]
    private float _dragRadius = 150f;
    [Tooltip("�÷��̾� Ŭ���� ����")] public Player player;

    bool _IsBtnPressed = false;

    private int _TouchId = -1; // �̰ɷ� �������� �������� �ľ��ϴ� ��
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _touchPad = GetComponent<RectTransform>();
        _StartPos = _touchPad.position;
    }
    public void ButtonDown()
    {
        _IsBtnPressed = true;
        
       
    }
    public void ButtonUp()
    {
        _IsBtnPressed = false;
        
    }
    
    void FixedUpdate()//���� ������Ʈ
    {
       if(Application.platform == RuntimePlatform.Android)
        {
            HandleTouch();
        }
       if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            HandleInput(Input.mousePosition);
        }
    }

    void HandleTouch()
    {
        // ����Ͽ��� ���ȿ��� ��ġ �Ǵ� ���� �Ǵ�
        int i = 0;
        // ���� �̵��ߴٸ� 0���� �̵�
        if (Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                i++;
                // ��ġ�� �ִ� ������ x,y���� ������
                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y);
                // ���� ��ġ�� �ߴٸ� Began
                if(touch.phase == TouchPhase.Began)
                {
                    
                    // ���� ������ x���� _StartPos.x(x����ؾ� float������ ����)
                    if (touch.position.x < _StartPos.x + _dragRadius && touch.position.y < _StartPos.y + _dragRadius &&
                        touch.position.x > _StartPos.x - _dragRadius && touch.position.y > _StartPos.y - _dragRadius)
                    {
                        // ��ġ�� x���� ��ǥ�� ���ȿ� �ִٸ�
                        _TouchId = i;
                       
                    }
                    
                }
                // ���� ��ġ�ϴ� ��ư�� �����̰� �ְų� ���ȿ��� ���߾��� �ִٸ�
                // �����̴� �� Moved && ������ �ִ°� Stationary
                if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (_TouchId == i)
                    {
                        HandleInput(touchPos);
                        
                    }
                }
                // ���� ��ġ�� �ϴ� ��ư�� ������ ���� ������ 
                if(touch.phase == TouchPhase.Ended)
                {
                    // ��ġ �� i�϶�
                    if(_TouchId == i)
                    {
                        // ��ġ�� 1�� �ȴ� ��������� ���������ʴ´�.
                        _TouchId = -1;
                    }
                }
            } 
        }
        
    }
    void HandleInput(Vector3 intput)
    {
        // ���� �Լ��� ������ ���̽�ƽ�� �����̴� �Լ�
        // ��ư�� ������ �� 
        if (_IsBtnPressed)
        {
            // �����Ӱ� ���� ��ġ�� ���� �Ÿ��� ������ �����ü� �ִ�.
            Vector3 differVetor = (intput - _StartPos);
              // ��ġ�е尡 �� ���� ����� 
            if(differVetor.sqrMagnitude > (_dragRadius*_dragRadius))
            {
                differVetor.Normalize();
                _touchPad.position = _StartPos + differVetor * _dragRadius;
                
                //�淮�� ���� ��ä �������� ������ ���� ���� �ʴ���.
            }
            // �����̶�� 
            else
            {
                // �״�� �����̰� ��
                _touchPad.position = _StartPos + differVetor * _dragRadius;
            }

        }
        // ��ư�� �������ʴ´ٸ�
        else
        {
            _touchPad.position = _StartPos;
        }
        Vector3 differ = _touchPad.position - _StartPos;
        Vector3 NormalDiffer = new Vector3(differ.x / _dragRadius,differ.y/_dragRadius);
        if(player != null)
        {
            player.OnStickPos(NormalDiffer);    
        }
    }
}
