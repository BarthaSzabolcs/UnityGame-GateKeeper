using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingText : MonoBehaviour
{
    #region Show In Editor

    [Header("Components:")]
    [SerializeField] Rigidbody2D self;
    [SerializeField] Image image;
    [SerializeField] Text text;

    #endregion
    #region Hide In Editor

    public FlyingTextData data;

    #endregion

    public void Initialize(int displayedText)
    {
        text.text = displayedText.ToString();
        text.color = data.color;

        image.sprite = data.displayedImage;
        image.color = Color.white;

        self.velocity = data.velocity;

        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(data.lifeTime);

        for(int i = 0; i < 60; i++)
        {
            image.color = new Color(1, 1, 1, 1f * (1f / i));
            text.color = new Color(data.color.r, data.color.g, data.color.b, 1f * (1f/ i));

            yield return new WaitForSeconds(data.fadeOutDuration / 60);
        }
        //image.CrossFadeAlpha(1, data.fadeOutDuration, false);
        //text.CrossFadeAlpha(1, data.fadeOutDuration, false);

        //yield return new WaitForSeconds(data.fadeOutDuration);

        ObjectPool.Instance.Pool(ObjectPool.Types.flyingText, gameObject);
    }

}
