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

    [SerializeField] private GameManager gameManager;

    //Variables que se usan para determinar el tiempo de movimiento del sprite.
    private Vector2 startPosition = new Vector2(0f, -2.56f);
    private Vector2 endPosition = Vector2.zero;
    private float showDuration = 0.5f;
    private float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private Vector2 boxOffset;
    private Vector2 boxSize;
    private Vector2 boxOffsetHidden;
    private Vector2 boxSizeHidden;

    private bool hittable = true;
    public enum TeemoType { Standard, HardHat, Hongo};
    private TeemoType teemoType;
    private float hardRate = 0.25f;
    private float hongoRate = 0f;
    private int lives;


   public void SetLevel(int level)
    {
        hongoRate = Mathf.Min(level * 0.025f, 0.25f);
        hardRate = Mathf.Min(level * 0.25f, 1f);

        float durationMin = Mathf.Clamp(1 - level * 0.1f, 0.01f, 1f);
        float durationMax = Mathf.Clamp(2 - level * 0.1f, 0.01f, 2f);
        duration = Random.Range(durationMin, durationMax);

    }
    
    public void SetIndex(int Index)
    {
        teemoIndex = index;
    }

     public void Activate(int level)
    {
        SetLevel(level);
        CreateNext();
        StartCoroutine(ShowHide(startPosition, endPosition));
    }


    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        boxOffset = boxCollider2D.offset;
        boxSize = boxCollider2D.size;
        boxOffsetHidden = new Vector2(boxOffset.x, -startPosition.y / 2f);
        boxSizeHidden = new Vector2(boxSize.x, 0f);

    }

     public IEnumerator ShowHide(Vector2 start, Vector2 end)
    {
        transform.localPosition = start;

        //Para que aparezca el Teemo
        float elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(start, end, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffsetHidden, boxOffset, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSizeHidden, boxSize, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
        boxCollider2D.offset = boxOffset;
        boxCollider2D.size = boxSize;

        yield return new WaitForSeconds(duration);

        elapsed = 0f;
        while (elapsed < showDuration)
        {
            transform.localPosition = Vector2.Lerp(end, start, elapsed / showDuration);
            boxCollider2D.offset = Vector2.Lerp(boxOffset, boxOffsetHidden, elapsed / showDuration);
            boxCollider2D.size = Vector2.Lerp(boxSize, boxSizeHidden, elapsed / showDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = start;
        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;

        if (hittable)
        {
            hittable = false;
            gameManager.Missed(teemoIndex, teemoType != TeemoType.Hongo);
        }
    }

     public void OnMouseDown()
    {
        if (hittable)
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
                 
                gameManager.GameOver(1);
                break;
                default:
                 break;
            }
        }
    }

    public void CreateNext()
    {
        float random = Random.Range(0f, 1f);
        if (random < hongoRate)
        {
            //Hacer que aparezcan los honguitos
            teemoType = TeemoType.Hongo;
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
            random = Random.Range(0f, 1f);
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


     public IEnumerator QuickHide()
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

        boxCollider2D.offset = boxOffsetHidden;
        boxCollider2D.size = boxSizeHidden;
    }
    
    public void StopGame()
    {
        hittable = false;
        StopAllCoroutines();
    }
}
