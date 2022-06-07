using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Koko.Utilities
{
    public static class KokoUtilities
    {
        private static Vector2 ReturnTextDirection(TextDir direction)
        {
            Vector2 textDirection = Vector2.zero;
            switch (direction)
            {
                case TextDir.Right:
                    textDirection = Vector2.right;
                    break;
                case TextDir.Left:
                    textDirection = Vector2.left;
                    break;
                case TextDir.Down:
                    textDirection = Vector2.down;
                    break;
                case TextDir.Up:
                    textDirection = Vector2.up;
                    break;
                default:
                    textDirection = Vector2.zero;
                    break;
            }

            return textDirection;
        }
        //Creates a text for some time with a direction to move and an easeAnimation
        public static void CreateWorldText(Vector2 pos, float duration, Ease easeAnimation, TextDir direction, RectTransform textObject, float text)
        {
          
            Vector2 goalPosition = pos + (ReturnTextDirection(direction) *5);
            textObject.GetComponent<TMP_Text>().text = "+"+text.ToString();
            textObject.DOLocalMove(goalPosition, duration).SetEase(easeAnimation).OnComplete(() =>
             {
                 textObject.anchoredPosition = Vector3.zero;
                 textObject.gameObject.SetActive(false);
             });
          
        }

        //Creates a pool for said object
        public static void CreatePoolForObject(GameObject prefab, Transform target, int amount, List<Transform> targetList)
        {
            for (int x = 0; x < amount; x++)
            {
                GameObject newDot = GameObject.Instantiate(prefab, target);
                newDot.SetActive(false);
                targetList.Add(newDot.transform);
               
            }
        }

        public static void CreatePoolForObject(GameObject prefab, Transform target, int amount, List<RectTransform> targetList)
        {
            for (int x = 0; x < amount; x++)
            {
                GameObject newDot = GameObject.Instantiate(prefab, target);
                newDot.SetActive(false);
                targetList.Add(newDot.GetComponent<RectTransform>());

            }
        }
    }
}

