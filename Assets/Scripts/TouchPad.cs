using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 1. 자기 자신(Touch Pad) 2. position 값을 스타트 포지션으로 정한다.
// 3. 패드 버튼을 눌렀는지 파악
// 4. 모바일 터치시 원안에 터치 되는지 안되는지 지를 정수값으로 판단
public class TouchPad : MonoBehaviour
{
    [SerializeField]
    [Tooltip("터치패드포지션")]
    private RectTransform _touchPad;
    [SerializeField]
    private Vector3 _StartPos;
    [SerializeField]
    private float _dragRadius = 150f;
    [Tooltip("플레이어 클래스 연결")] public Player player;

    bool _IsBtnPressed = false;

    private int _TouchId = -1; // 이걸로 원안인지 원밖인지 파악하는 것
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
    
    void FixedUpdate()//고정 업데이트
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
        // 모바일에서 원안에서 터치 되는 지만 판단
        int i = 0;
        // 만약 이동했다면 0에서 이동
        if (Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                i++;
                // 터치에 있는 포지션 x,y값은 움직임
                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y);
                // 만약 터치를 했다면 Began
                if(touch.phase == TouchPhase.Began)
                {
                    
                    // 터지 포지션 x값에 _StartPos.x(x라고해야 float값으로 변경)
                    if (touch.position.x < _StartPos.x + _dragRadius && touch.position.y < _StartPos.y + _dragRadius &&
                        touch.position.x > _StartPos.x - _dragRadius && touch.position.y > _StartPos.y - _dragRadius)
                    {
                        // 터치한 x값의 좌표가 원안에 있다면
                        _TouchId = i;
                       
                    }
                    
                }
                // 만약 터치하는 버튼이 움직이고 있거나 원안에서 멈추어져 있다면
                // 움직이는 건 Moved && 가만히 있는건 Stationary
                if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (_TouchId == i)
                    {
                        HandleInput(touchPos);
                        
                    }
                }
                // 만약 터치는 하는 버튼이 움직이 않을 때에는 
                if(touch.phase == TouchPhase.Ended)
                {
                    // 터치 가 i일때
                    if(_TouchId == i)
                    {
                        // 터치는 1이 된다 결론적으로 움직이지않는다.
                        _TouchId = -1;
                    }
                }
            } 
        }
        
    }
    void HandleInput(Vector3 intput)
    {
        // 위의 함수는 실제로 조이스틱을 움직이는 함수
        // 버튼을 눌렀을 시 
        if (_IsBtnPressed)
        {
            // 움직임과 현재 위치를 뺴면 거리와 방향을 가져올수 있다.
            Vector3 differVetor = (intput - _StartPos);
              // 터치패드가 원 밖을 벗어나면 
            if(differVetor.sqrMagnitude > (_dragRadius*_dragRadius))
            {
                differVetor.Normalize();
                _touchPad.position = _StartPos + differVetor * _dragRadius;
                
                //방량은 유지 한채 원끝에서 원밖을 벗어 나지 않느다.
            }
            // 원안이라면 
            else
            {
                // 그대로 움직이게 함
                _touchPad.position = _StartPos + differVetor * _dragRadius;
            }

        }
        // 버튼을 누르지않는다면
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
