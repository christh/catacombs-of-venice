using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IR
{
    public class HealthPopup : MonoBehaviour
    {
        TextMeshPro text;
        Color textColor = Color.white;
        float disappearAfter = 0.1f;
        float fadeOutSpeed = 3f;
        float popupMoveSpeedY = 3f;
        float popupMoveSpeedX = 3f;

        public static HealthPopup Damage(Vector2 position, int amount)
        {
            Transform popupTransform = Instantiate(AssetLookup.a.damagePopup, position, Quaternion.identity);
            HealthPopup popup = popupTransform.GetComponent<HealthPopup>();
            popup.Setup(amount);
            return popup;
        }

        public static HealthPopup Heal(Vector2 position, int amount)
        {
            Transform popupTransform = Instantiate(AssetLookup.a.damagePopup, position, Quaternion.identity);
            HealthPopup popup = popupTransform.GetComponent<HealthPopup>();
            popup.popupMoveSpeedX = 2f;
            popup.popupMoveSpeedY = 2f;
            popup.fadeOutSpeed = 2f;
            popup.textColor = Color.green;
            popup.Setup(amount);
            return popup;
        }


        // Start is called before the first frame update
        public void Awake()
        {
            text = transform.GetComponent<TextMeshPro>();
        }

        public void Setup(int damage)
        {
            var rand = Random.value;
            if (rand < 0.5f)
            {
                popupMoveSpeedX = -popupMoveSpeedX;
            }
            
            text.SetText(damage.ToString());
        }

        public void Update()
        {
            transform.position += new Vector3(popupMoveSpeedX, popupMoveSpeedY) * Time.deltaTime;
            disappearAfter -= Time.deltaTime;
            if (disappearAfter < 0)
            {
                textColor.a -= fadeOutSpeed * Time.deltaTime;
                text.color = textColor;

                if (textColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }


    }
}