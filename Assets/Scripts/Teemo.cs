using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teemo : MonoBehaviour
{
    //Referencias para los sprites
    [Header("Sprites")]
    [SerializeField] private Sprite teemoNormal;
    [SerializeField] private Sprite teemoAtaque;
    [SerializeField] private Sprite teemoEnemigo1; ///normal
    [SerializeField] private Sprite teemoEnemigo2; //primer golpe
    [SerializeField] private Sprite teemoEnemigo3; //muelto

    //Variables que se usan para determinar el tiempo de movimiento del sprite.
    private Vector2 startPosition = new Vector2(0f, -2.56f);
    private Vector2 endPosition = Vector2.zero;
    private float showDuration = 0.5f;
    private float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool hittable = true;
    public enum TeemoType { Standard, HardHat, Hongo};
    private TeemoType teemoType;
    private float hardRate = 0.25f;
    private float hongoRate = 0f;
    private int lives;
    
    private IEnumerator ShowHide(Vector2 start, Vector2 end)
    {
        transform.localPosition = start;

        //Para que aparezca el Teemo
        float elapsed = 0f;
        while (elapsed < showDuration) 
        {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;

        yield return new WaitForSeconds(duration);

        elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition =start;
    }
    

    private void Start()
    {
        CreateNext();
        StartCoroutine(ShowHide(startPosition, endPosition));
    }

    private void CreateNext()
    {
        float random = Random.Range(0f, 1f);
        if(random < hongoRate)
        {
            //Hacer que aparezcan los honguitos
            teemoType = TeemoType.Hongo;
            animator.enabled = true;
        }
            else
        {
            animator.enabled = false;
            random = Random.Range (0f, 1f);
            if (random < hardRate)
            {
                teemoType = TeemoType.HardHat;
                spriteRenderer.sprite = teemoEnemigo1;
                lives = 2;
            }
                else
            {
                teemoType = TeemoType.Standard;
                spriteRenderer.sprite = teemoNormal;
                lives = 1;
            }
        }

        hittable = true;

    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if(hittable)
        {
            switch (teemoType)
            {
                case TeemoType.Standard:
            spriteRenderer.sprite = teemoAtaque;
            //Detiene la animación
            StopAllCoroutines();
            StartCoroutine(QuickHide());

            hittable = false;
            break;

                case TeemoType.HardHat:
            if (lives == 2)
            {
                spriteRenderer.sprite = teemoEnemigo2;
                lives--;
            }
            else
                {
                    spriteRenderer.sprite = teemoEnemigo3;
                    StopAllCoroutines();
                    StartCoroutine(QuickHide());
                    hittable = false;   
                }
                break;
                case TeemoType.Hongo:
                    break;
                    default:
                    break;
            }
        }
    }

    private IEnumerator QuickHide()
    {
        yield return new WaitForSeconds(0.25f);

        if(!hittable)
        {
            Hide();
        }
    }

    public void Hide()
    {
        transform.localPosition = startPosition;
    }
    
}
