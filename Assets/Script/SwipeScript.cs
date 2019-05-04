using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeScript : MonoBehaviour {

	Vector3 startPos, mousePos, direction;

    RaycastHit hit;
	[SerializeField]
    private float throwForceInYandZ;
    public GameObject signalBall;
    public GameObject ball;
    public LayerMask layerBall;
    public Color normalColor, pressColor;

    [Space(10)]
    public GameObject circle;
    public int quantCircles;
    public float maxDistancePower;
    public Color colorCircle;
    public Color colorCircleBack;
    public GameObject trajetory;

    [Space(10)]
    public Transform arrowPoint;
    public Transform posArrow;
    public Transform arrowBody;

    private GameObject[] circlesClone;
    private GameObject[] backCirclesClone;
    private SpriteRenderer[] spriteArrow;
    private float distancePower;
    private float spacing = 0.1f;
    private bool playHitBall = false;
    
	Rigidbody BallRb;

    private MeshCollider meshColliderSignal; 
	void Start()
	{
        spriteArrow = new SpriteRenderer[2];
        spriteArrow[0] = arrowBody.GetComponent<SpriteRenderer>();
        spriteArrow[1] = arrowPoint.GetComponent<SpriteRenderer>();

		BallRb = GetComponent<Rigidbody> ();
        meshColliderSignal = signalBall.GetComponent<MeshCollider>();
        signalBall.GetComponent<MeshRenderer>().material.SetColor("_Color", normalColor);

        circlesClone = new GameObject[quantCircles];
        backCirclesClone = new GameObject[quantCircles];
        for (int cont = 0; cont<quantCircles; cont++)
        {
            circlesClone[cont] = Instantiate(circle,trajetory.transform, true);
            backCirclesClone[cont] = Instantiate(circle, trajetory.transform, true);
        }
        float currentValor = 0;
        float factorScale = 1;
        for(int cont = 0; cont<quantCircles; cont++)
        {

            circlesClone[cont].transform.localPosition = new Vector3(currentValor, 0, 0);
            circlesClone[cont].transform.localScale = new Vector3(circlesClone[cont].transform.localScale.x / factorScale, circlesClone[cont].transform.localScale.y, circlesClone[cont].transform.localScale.z / factorScale);
            circlesClone[cont].GetComponent<MeshRenderer>().material.SetColor("_Color", colorCircle);

            backCirclesClone[cont].transform.localPosition = new Vector3(-currentValor/2, 0, 0);
            backCirclesClone[cont].transform.localScale = new Vector3(backCirclesClone[cont].transform.localScale.x / (factorScale*1.1f), backCirclesClone[cont].transform.localScale.y, backCirclesClone[cont].transform.localScale.z / (factorScale * 1.1f));
            backCirclesClone[cont].GetComponent<MeshRenderer>().material.SetColor("_Color", colorCircleBack);

            currentValor += spacing;
            factorScale += 0.1f;
        }
        trajetory.transform.SetParent(transform);
        trajetory.transform.position = ball.transform.position;

        trajetory.SetActive(false);
        //TrageroryProjectile();
    }

    // Update is called once per frame
    void FixedUpdate () {
        Ray rayCamera = Camera.main.ScreenPointToRay(Input.mousePosition);

        #if UNITY_ANDOID
        Vector3 positionEvent = Input.GetTouch(0).position;
        if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {

            Ray ray = Camera.main.ScreenPointToRay(positionEvent);

            if (Physics.Raycast(ray, out hit, 500f, layerBall))
            {
                signalBall.GetComponent<MeshRenderer>().material.SetColor("_Color", pressColor);
                startPos = positionEvent;
                playHitBall = true;
            }
            else
                playHitBall = false;
		}

		
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {



            endPos = positionEvent;

            direction = startPos - endPos;


            direction.Normalize();
            BallRb.AddForce(direction.x * throwForceInYandZ, 0, direction.y * throwForceInYandZ);

            if (transform.position.y < -10)
            {
                Destroy(this.gameObject);
            }
            signalBall.GetComponent<MeshRenderer>().material.SetColor("_Color", normalColor);
            playHitBall = false;
        }
        #elif UNITY_EDITOR
       
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, 500f, layerBall))
            {
                signalBall.GetComponent<MeshRenderer>().material.SetColor("_Color", pressColor);
                trajetory.SetActive(true);
                playHitBall = true;
            }
            else
                playHitBall = false;
            
            
        }

        if(Input.GetMouseButton(0) && playHitBall)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = mousePos - ball.transform.position;
            TrageroryProjectile(direction);
            DinamicScaleWithDistance();
            
        }
        if (Input.GetMouseButtonUp(0) && playHitBall)
        {
            trajetory.SetActive(false);

            direction = ball.transform.position - mousePos;
           

            direction.Normalize();

            BallRb.AddForce(direction.x * throwForceInYandZ*distancePower, 0, direction.z * throwForceInYandZ*distancePower);

            if (transform.position.y < -10)
            {
                Destroy(this.gameObject);
            }
            signalBall.GetComponent<MeshRenderer>().material.SetColor("_Color", normalColor);
            playHitBall = false;
        }
#endif
        signalBall.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
	public void TrageroryProjectile(Vector3 direction)
    {
        direction.Normalize();
        float angle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg;
        trajetory.transform.rotation = Quaternion.Euler(0, angle+90, 0);

        arrowPoint.transform.position = posArrow.position;
        arrowPoint.transform.rotation = posArrow.rotation;
        arrowBody.rotation = Quaternion.Euler(90, angle+180, 0);
        arrowBody.localScale = new Vector3(arrowBody.localScale.x, distancePower / 2, arrowBody.localScale.z);
    }
    private void DinamicScaleWithDistance()
    {
        mousePos.y = ball.transform.position.y;

        distancePower = Vector3.Distance(mousePos,ball.transform.position);
        if (distancePower > maxDistancePower) distancePower = maxDistancePower;
        float factorCircle = distancePower / quantCircles;
        float currentValor = 0;
        foreach(GameObject circle in backCirclesClone)
        {
            currentValor -= factorCircle;
            circle.transform.localPosition = new Vector3(currentValor, 0, 0);
        }
        Color colorArrow = ColorWithDisntancePower();
        spriteArrow[0].color = colorArrow;
        spriteArrow[1].color = colorArrow;
    }
    private Color ColorWithDisntancePower()
    {
        float colorH = 100 - (distancePower*100) / maxDistancePower;
        colorH = colorH / 360;
        return Color.HSVToRGB(colorH, 1, 1);
    }
}
