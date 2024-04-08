using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teemo : MonoBehaviour
{
    //Referencias para los sprites
    [Header("Sprites")]
    [SerializeField] private Sprite teemoNormal;
    [SerializeField] private Sprite teemoAtaque;


    //Variables que se usan para determinar el tiempo de movimiento del sprite.
    private Vector2 startPosition = new Vector2(0f, -2.56f);
    private Vector2 endPosition = Vector2.zero;
    private float showDuration = 0.5f;
    private float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool hittable = true;
    public enum TeemoType { Standard, Bomb};
    
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
        StartCoroutine(ShowHide(startPosition, endPosition));
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if(hittable)
        {
            spriteRenderer.sprite = teemoAtaque;
            //Detiene la animación
            StopAllCoroutines();
            StartCoroutine(QuickHide());
            hittable = false;
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
