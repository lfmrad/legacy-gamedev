using UnityEngine;
using System.Collections;

public class SetCollider : MonoBehaviour {

	void Start () {
			
		Transform childTransform;
		BoxCollider2D boxCollider2D;
			
		childTransform = transform.GetChild(0);
		boxCollider2D = GetComponent<BoxCollider2D>();
			
		boxCollider2D.size = new Vector2 (childTransform.localScale.x, 1 + childTransform.localScale.y + Mathf.Abs(childTransform.localPosition.y) * 2 - transform.localScale.y);

//		if (childTransform.tag == "FieldAttached") {
//			
//			float fieldHeight;
//
//			fieldHeight = childTransform.GetComponentInChildren<FieldGenerator>().fieldHeight;
//
//			boxCollider2D.size = new Vector2 (childTransform.localScale.x, fieldHeight * 2);
//
//		}
	}

}
