using UnityEngine;

public class Tile : MonoBehaviour
{
    public int index;
    public bool isOccupied;
    public GameObject O, X;

    void Start () {
        X = Instantiate(X, this.transform);
        O = Instantiate(O, this.transform);

        if (X == null || O == null) Debug.Log("X OR O is NULL");

        X.transform.localScale = new Vector3(X.transform.localScale.x * 0.2f, X.transform.localScale.y * 0.2f, 1f);
        O.transform.localScale = new Vector3(O.transform.localScale.x * 0.2f, O.transform.localScale.y * 0.2f, 1f);

        O.SetActive(false);
        X.SetActive(false);
    }

    void OnMouseEnter()
    {
        if (!isOccupied) {
            Highlight();
        }   
    }

    void OnMouseExit() {
        if (!isOccupied) {
            UnHighlight();
        } 
    }

    void OnMouseDown() {
        if (!GameManager.instance.isAi) {
            GameManager.instance.MakeMove(this.index);
        }
    }

    void Highlight() {
        if (GameManager.instance.isX) {
            X.SetActive(true);
            SpriteRenderer[] xStripes = X.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer stripeRenderer in xStripes) {
                stripeRenderer.color = Color.gray; 
            }
        } else {
            O.SetActive(true);
            SpriteRenderer oRender = O.GetComponent<SpriteRenderer>();
            oRender.color = Color.gray; 
        }
    }

    void UnHighlight() {
        if (GameManager.instance.isX) {
            X.SetActive(false);
            SpriteRenderer[] xStripes = X.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer stripeRenderer in xStripes) {
                stripeRenderer.color = Color.white; 
            }
        } else {
            O.SetActive(false);
            SpriteRenderer oRender = O.GetComponent<SpriteRenderer>();
            oRender.color = Color.white; 
        }
    }
}
