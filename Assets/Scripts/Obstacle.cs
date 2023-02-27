using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float leftEdge;
    private float rightEdge;
    private float downEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
        rightEdge = 10f;
        downEdge = 0f;
    }

    private void Update()
    {
        if (this.gameObject.tag == "Left")
        {        
            transform.position -= Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime;
        }
        if (this.gameObject.tag == "Right")
        {
            transform.position += Vector3.left * GameManager.Instance.gameSpeed * Time.deltaTime;
        }
        if (this.gameObject.tag == "Meteor")
        {
            transform.position += new Vector3(-1.5f, -1, 0) * GameManager.Instance.gameSpeed * Time.deltaTime;
        }
        /*
        if (transform.position.x < leftEdge || transform.position.x > rightEdge 
            || transform.position.y < downEdge) {
            Destroy(gameObject);
        }*/
    }

}
