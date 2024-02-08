using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnDamageText : MonoBehaviour
{
    [SerializeField] GameObject damagetextPrefab;
    public void SpawnDamageTextMethod(float damage)
    {
        GameObject instance = Instantiate(damagetextPrefab, transform);
        RectTransform canvas = instance.GetComponent<RectTransform>();
        instance.transform.Find("Damage").transform.localPosition = new Vector3(Random.Range(-canvas.rect.width / 2, canvas.rect.width / 2), Random.Range(-canvas.rect.height / 2, canvas.rect.height / 2));
        instance.transform.Find("Damage").GetComponent<Text>().text = damage.ToString();
        instance.GetComponent<Animation>().Play();
    }
}
