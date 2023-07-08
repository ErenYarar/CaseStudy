using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch; // Touch kullanmak yerine

public class PlayerMovement : MonoBehaviour
{
    Animator playerAnimator; // Oyuncunun Animator bileşeni
    Rigidbody playerRb; // Oyuncunun Rigidbody bileşeni
    private float minFloat = 500f; // Etrafındaki nesnelere uygulanacak minimum vurma gücü
    private float maxFloat = 1000f; // Etrafındaki nesnelere uygulanacak maksimum vurma gücü
    private float forceHit; // Etrafındaki nesnelere uygulanacak vurma gücü

    [Header("Joystick")]
    [SerializeField] Vector2 JoystickSize = new Vector2(300, 300);  // Joystick boyutu
    [SerializeField] FloatingJoystick Joystick; // FloatingJoystick sınıfı

    private Finger MovementFinger; // Kullanıcının ekranın belirli bir bölümüne dokunup dokunmadığı kontrolü
    private Vector2 MovementAmount; // Navmesh agent'ın hareketi

    [Space]
    [Header("AI")]
    [SerializeField] private NavMeshAgent Player; // Oyuncunun NavMeshAgent bileşeni

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        // x ve y pozisyonunda navmeshagent hareketi için 
        Vector3 scaledMovement = Player.speed * Time.deltaTime * new Vector3(
            MovementAmount.x,
            0,
            MovementAmount.y
        );

        if (scaledMovement.magnitude > 0) 
        {
            playerAnimator.SetBool("isRunning", true); // Koşma animasyonunu etkinleştir
        }
        else
        {
            playerAnimator.SetBool("isRunning", false); // Koşma animasyonunu devre dışı bırak
        }

        Player.transform.LookAt(transform.position + scaledMovement, Vector3.up); //Gidilen yöne bakar
        Player.Move(scaledMovement); // Oyuncuyu hareket ettirir
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))  // Etrafındaki düşmanlara çarptığında
        {
            playerAnimator.SetBool("isFloating", true); // "isFloating" animasyon parametresini true yapar

            forceHit = UnityEngine.Random.Range(minFloat, maxFloat); // Vurma gücünü rastgele belirler
            Vector3 moveDirection = playerRb.transform.position - collision.transform.position;
            playerRb.AddForce(moveDirection.normalized * forceHit); // Düşmana vurma gücünü uygular

            StartCoroutine(WaitForAnimEnded()); // Animasyonun bitmesini bekler
        }
    }

    IEnumerator WaitForAnimEnded()
    {
        yield return new WaitForSeconds(.5f); // 0.5 saniye bekletir sonra:
        playerAnimator.SetBool("isFloating", false); // "isFloating" animasyon parametresini false yapar
    }

    //Ekrana dokunma ile sağlanan işlevselliği etkinleştirmek için bu yöntemin çağrılması gereklidir. 
    //OnEnableda API'ler, dokunuşlara fazladan işlem ekler ve bu nedenle varsayılan olarak devre dışı bırakılır
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable() // Touch input disable edilir ve handler kapatılır
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger MovedFinger)
    {
        if (MovedFinger == MovementFinger) // Ekranda parmak hareket ettirildiğinde
        {
            Vector2 knobPosition; // knobPosition pozisyonu eklenir
            float maxMovement = JoystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch; // Mevcut dokunma yeri güncellenir

            if (Vector2.Distance(
                    currentTouch.screenPosition,
                    Joystick.RectTransform.anchoredPosition
            ) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition).normalized * maxMovement; // Knob objesi joystick dışına çıkmaması için
            }
            else
            {
                knobPosition = currentTouch.screenPosition - Joystick.RectTransform.anchoredPosition;
            }

            Joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = new Vector2(-knobPosition.x, -knobPosition.y) / maxMovement; 
            // MovementAmount = knobPosition / maxMovement;
        }
    }

    private void HandleLoseFinger(Finger LostFinger) // Parmak çekildiğinde
    {
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero; // Knob orijinal pozisyonuna (Orta noktaya) döner
            Joystick.gameObject.SetActive(false); // Joystick kapatılır
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        if (MovementFinger == null) // Ekrana dokunup, hareket ettirilmediğinde
        {
            MovementFinger = TouchedFinger;
            MovementAmount = Vector2.zero; // Dokunulan yerin orta noktasında başlatır
            Joystick.gameObject.SetActive(true); // Joystick ekrana verir
            Joystick.RectTransform.sizeDelta = JoystickSize; // Joystick size eklenir
            Joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition); 
        }
    }

    // Ekranın en uç noktalarına tıklayıp joystick'in kaybolmasını engeller
    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < JoystickSize.x / 2)
        {
            StartPosition.x = JoystickSize.x / 2;
        }
        else if (StartPosition.x > Screen.width - JoystickSize.x / 2) // Ekranın aşağı ucuna tıklanırsa yukarıya çeker
        {
            StartPosition.x = Screen.width - JoystickSize.x / 2;
        }

        if (StartPosition.y < JoystickSize.y / 2)
        {
            StartPosition.y = JoystickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - JoystickSize.y / 2) // Ekranın yukarı ucuna tıklanırsa aşağıya çeker
        {
            StartPosition.y = Screen.height - JoystickSize.y / 2;
        }

        return StartPosition;
    }
}
