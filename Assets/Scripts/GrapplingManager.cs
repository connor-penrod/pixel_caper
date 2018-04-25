using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using System;

public class GrapplingManager : MonoBehaviour{

	public GameObject anchorPoint;

    [HideInInspector]
    public LineRenderer lineRenderer;
    [HideInInspector]
    public DistanceJoint2D hinge;
    [HideInInspector]
    public CorgiController cg;
    [HideInInspector]
    public bool isGrappling = false;
	int wrapCount = 0;
	float grappleDistance = Mathf.Infinity;
	Vector2 hitPosition;
	float lastAngle = 0f;
	int swingDirX = 0;

	// Use this for initialization
	void Start () {
		cg = GetComponent<CorgiController>();
		hinge = GetComponent<DistanceJoint2D>();
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetPosition(1,transform.position);
		lineRenderer.enabled = false;
		hinge.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {


		//on left click, move line renderer, reveal it, and set grapple distance
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position));
			if (hit.collider != null && hit.collider.attachedRigidbody != null) {
				lineRenderer.positionCount = 2;
				lineRenderer.SetPosition(0, hit.point);
				lineRenderer.SetPosition(1, transform.position);
				lineRenderer.enabled = true;

				grappleDistance = Vector2.Distance(hit.point, transform.position);
                this.GetComponent<Rigidbody2D>().gravityScale = 1 ;
                this.GetComponent<Rigidbody2D>().drag = 0;
                this.GetComponent<Rigidbody2D>().angularDrag= 0;
                this.GetComponent<BoxCollider2D>().isTrigger = false;
				anchorPoint.transform.position = hit.point;
				hinge.distance = grappleDistance;

				Vector2 prevVelocity = cg.Speed;

				hinge.enabled = true;
				//StartCoroutine("ThrowRope", hit.point);

				this.GetComponent<Rigidbody2D>().velocity = prevVelocity;

				cg.enabled = false;
				hitPosition = hit.point;
				isGrappling = true;

				/*hinge.connectedBody = hit.collider.attachedRigidbody;
				hinge.anchor = transform.position;
				//hinge.connectedAnchor = hit.point;
				hinge.enableCollision = true;
				//hinge.useLimits = true;
				hinge.enabled = true;*/
			}
		}

		//on right click, release grapple and hide line renderer
		if(Input.GetMouseButtonDown(1))
		{
			isGrappling = false;
			lineRenderer.enabled = false;

            Vector2 prevVelocity = this.GetComponent<Rigidbody2D>().velocity;
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.GetComponent<Rigidbody2D>().angularDrag = 100000;
            this.GetComponent<Rigidbody2D>().drag = 100000;
            this.GetComponent<BoxCollider2D>().isTrigger = true;
			cg.enabled = true;
            hinge.enabled = false;
			lineRenderer.positionCount = 2;
            cg.AddForce(prevVelocity);
        }
		if(Input.GetKeyDown(KeyCode.Space))
		{
			isGrappling = false;
			lineRenderer.enabled = false;

			Vector2 prevVelocity = this.GetComponent<Rigidbody2D>().velocity;
			this.GetComponent<Rigidbody2D>().gravityScale = 0;
			this.GetComponent<Rigidbody2D>().angularDrag = 100000;
			this.GetComponent<Rigidbody2D>().drag = 100000;
			this.GetComponent<BoxCollider2D>().isTrigger = true;
			cg.enabled = true;
			hinge.enabled = false;
			lineRenderer.positionCount = 2;
			cg.AddForce(prevVelocity);
			cg.AddVerticalForce(5f);
		}

		//perform force adjustments for grappling
		if(isGrappling && lineRenderer.positionCount > 1)
		{
			lineRenderer.SetPosition(lineRenderer.positionCount-1, transform.position);
			anchorPoint.transform.position = lineRenderer.GetPosition(lineRenderer.positionCount-2);
			float lineDistance = Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount-1), (Vector2)lineRenderer.GetPosition(lineRenderer.positionCount-2))-0.1f;
			RaycastHit2D wrapCheck = Physics2D.Raycast(transform.position, ((Vector2)lineRenderer.GetPosition(lineRenderer.positionCount-2) - (Vector2)transform.position).normalized, lineDistance);
			if(wrapCheck.collider != null)
			{
				lineRenderer.SetPosition(lineRenderer.positionCount-1, wrapCheck.point);
				lineRenderer.positionCount += 1;
				lineRenderer.SetPosition(lineRenderer.positionCount-1, transform.position);
				StartCoroutine("ReadjustDistance");
				Debug.Log(hinge.distance);
				wrapCount += 1;
			}

			if(wrapCount > 0 && lineRenderer.positionCount > 2)
			{
				RaycastHit2D unwrapCheck = Physics2D.Raycast(transform.position, ((Vector2)lineRenderer.GetPosition(lineRenderer.positionCount-3) - (Vector2)transform.position).normalized,
					Vector2.Distance(transform.position, lineRenderer.GetPosition(lineRenderer.positionCount-3))-0.01f);
				//Debug.DrawLine((Vector2)transform.position, ((Vector2)lineRenderer.GetPosition(lineRenderer.positionCount-3) - (Vector2)transform.position).normalized);


				if(unwrapCheck.collider == null)
				{
					float wrapAngle = Vector2.Angle(
						lineRenderer.GetPosition(lineRenderer.positionCount-3)-lineRenderer.GetPosition(lineRenderer.positionCount-2),
						transform.position-lineRenderer.GetPosition(lineRenderer.positionCount-2));
					Debug.Log(wrapAngle);
					if(wrapAngle > 160)
					{
						lineRenderer.SetPosition(lineRenderer.positionCount-2, transform.position);
						lineRenderer.positionCount -= 1;
						hinge.distance = Vector2.Distance(transform.position, (Vector2)lineRenderer.GetPosition(lineRenderer.positionCount-2));
						wrapCount -= 1;
					}
				}
				/*
				if(swingDirX > 0 && (transform.position.x - lineRenderer.GetPosition(lineRenderer.positionCount-2).x) < 0)
				{
					lineRenderer.SetPosition(lineRenderer.positionCount-2, transform.position);
					lineRenderer.positionCount -= 1;
					hinge.distance = Vector2.Distance(transform.position, (Vector2)lineRenderer.GetPosition(lineRenderer.positionCount-2));
					unWrappable = false;
				}
				else if(swingDirX < 0 && (transform.position.x - lineRenderer.GetPosition(lineRenderer.positionCount-2).x) > 0)
				{
					lineRenderer.SetPosition(lineRenderer.positionCount-2, transform.position);
					lineRenderer.positionCount -= 1;
					hinge.distance = Vector2.Distance(transform.position, (Vector2)lineRenderer.GetPosition(lineRenderer.positionCount-2));
					unWrappable = false;
				}*/
			}

			if(Input.GetKey(KeyCode.W) && hinge.distance > 0.1f)
			{
				hinge.distance -= 0.01f;
			}
			else if(Input.GetKey(KeyCode.S))
			{
				hinge.distance += 0.01f;
			}

			if(Input.GetKey(KeyCode.A))
			{
				this.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 0.0002f);
			}
			else if(Input.GetKey(KeyCode.D))
			{
				this.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 0.0002f);
			}
		}
	}
	IEnumerator ReadjustDistance() {
		yield return new WaitForSeconds(0.01f);
		hinge.distance = Vector2.Distance(transform.position, (Vector2)lineRenderer.GetPosition(lineRenderer.positionCount-2));
	}
	IEnumerator ThrowRope(Vector2 target)
	{
		lineRenderer.enabled = true;
		for(int i = 1; i <= 9; i++)
		{
			if(lineRenderer.positionCount != 2)
			{
				yield break;
			}
			lineRenderer.SetPosition(1, transform.position);
			lineRenderer.SetPosition(0,
				(Vector2)transform.position + (target-(Vector2)transform.position).normalized*(Vector2.Distance((Vector2)transform.position, target)/(10-i)));
			yield return null; //return new WaitForSeconds(0.02f);
		}
	}
}
