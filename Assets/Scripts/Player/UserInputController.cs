using UnityEngine;

public class UserInputController : MonoBehaviour
{
    [SerializeField] private User user;
    void Update()
    {
        if (!user.IsMyTurn)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D hit2 = Physics2D.Raycast(mousePosition2D, Vector2.zero, Mathf.Infinity);

            if (hit2.collider != null)
            {
                if (hit2.collider.gameObject.TryGetComponent<CardObject>(out CardObject cardObject) &&
                    cardObject != null)
                {
                    user.CardPressed(cardObject);
                    return;
                }
            }
        }
    }
}
