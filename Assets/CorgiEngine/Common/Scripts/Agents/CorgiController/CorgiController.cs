using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.CorgiEngine
{	
	[RequireComponent(typeof(BoxCollider2D))]

	// DISCLAIMER : this controller's been built from the ground up for the Corgi Engine. It takes clues and inspirations from various methods and articles freely 
	// available online. Special thanks to @prime31 for his talent and patience, Yoann Pignole, Mysteriosum and Sebastian Lague, among others for their great articles
	// and tutorials on raycasting. If you have questions or suggestions, feel free to contact me at unitysupport@reuno.net

	/// <summary>
	/// The character controller that handles the character's gravity and collisions.
	/// It requires a Collider2D and a rigidbody to function.
	/// </summary>
	[AddComponentMenu("Corgi Engine/Character/Core/Corgi Controller")] 
	public class CorgiController : MonoBehaviour 
	{
		/// the various states of our character
		public CorgiControllerState State { get; protected set; }
		/// the initial parameters
		[Header("Default Parameters")]
		public CorgiControllerParameters DefaultParameters;
		/// the current parameters
		public CorgiControllerParameters Parameters{get{return _overrideParameters ?? DefaultParameters;}}

		[Header("Collision Masks")]
		[Information("You need to define what layer(s) this character will consider a walkable platform/moving platform etc. By default, you want Platforms, MovingPlatforms, OneWayPlatforms, MovingOneWayPlatforms, in this order.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// The layer mask the platforms are on
		public LayerMask PlatformMask=0;
		/// The layer mask the moving platforms are on
		public LayerMask MovingPlatformMask=0;
		/// The layer mask the one way platforms are on
		public LayerMask OneWayPlatformMask=0;
		/// The layer mask the moving one way platforms are on
		public LayerMask MovingOneWayPlatformMask=0;
		/// The possible ways a character can detach from a oneway or moving platform
		public enum DetachmentMethods { Layer, Object }
		/// When a character jumps from a oneway or moving platform, collisions are off for a short moment. You can decide if they should happen on a whole moving/1way platform layer basis or just with the object the character just left
		public DetachmentMethods DetachmentMethod = DetachmentMethods.Layer;

		/// gives you the object the character is standing on
		public GameObject StandingOn { get; protected set; }	
		/// gives you the collider the character is standing on
		public Collider2D StandingOnCollider { get; protected set; }	
		/// the current velocity of the character
		public Vector2 Speed { get{ return _speed; } }
		/// the value of the forces applied at one point in time 
		public Vector2 ForcesApplied { get; protected set; }
		/// the wall we're currently colliding with
		public GameObject CurrentWallCollider { get; protected set; }

		[Header("Raycasting")]
		[Information("Here you can define how many rays are cast horizontally and vertically. You'll want them as far as possible from each other, but close enough that no obstacle or enemy can fit between 2 rays.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// the number of rays cast horizontally
		public int NumberOfHorizontalRays = 8;
		/// the number of rays cast vertically
		public int NumberOfVerticalRays = 8;
		/// a small value added to all raycasts to accomodate for edge cases	
		public float RayOffset=0.05f; 

		[Header("Stickiness")]
		[Information("Here you can define whether or not you want your character stick to slopes when walking down them, and how long the raycast handling that should be (0 means automatic length).",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// If this is set to true, the character will stick to slopes when walking down them
		public bool StickWhenWalkingDownSlopes = false;
		/// The length of the raycasts used to stick to downward slopes
		public float StickyRaycastLength = 0f;


		public Vector3 ColliderSize {get
		{
			return Vector3.Scale(transform.localScale, _boxCollider.size);
		}}

		public Vector3 ColliderCenterPosition {get
		{
			return _boxCollider.bounds.center;
			}}

		public virtual Vector3 ColliderBottomPosition { get
		{
			_boundsBottomCenterPosition.x = _boxCollider.bounds.center.x;
			_boundsBottomCenterPosition.y = _boxCollider.bounds.min.y;
			_boundsBottomCenterPosition.z = 0;
			return _boundsBottomCenterPosition;
			}}

		public virtual Vector3 ColliderLeftPosition { get
		{
			_boundsLeftCenterPosition.x = _boxCollider.bounds.min.x;
			_boundsLeftCenterPosition.y = _boxCollider.bounds.center.y;
			_boundsLeftCenterPosition.z = 0;
			return _boundsLeftCenterPosition;
			}}

		public virtual Vector3 ColliderTopPosition { get
		{
			_boundsTopCenterPosition.x = _boxCollider.bounds.center.x;
			_boundsTopCenterPosition.y = _boxCollider.bounds.min.y;
			_boundsTopCenterPosition.z = 0;
			return _boundsTopCenterPosition;
			}}

		public virtual Vector3 ColliderRightPosition { get
		{
			_boundsRightCenterPosition.x = _boxCollider.bounds.center.x;
			_boundsRightCenterPosition.y = _boxCollider.bounds.min.y;
			_boundsRightCenterPosition.z = 0;
			return _boundsRightCenterPosition;
		}}

		public float Friction { get
		{
			return _friction;
		}}

		// parameters override storage
		protected CorgiControllerParameters _overrideParameters;
		// private local references			
		protected Vector2 _speed;
		protected float _friction=0;
		protected float _fallSlowFactor; 
		protected float _currentGravity = 0;
		protected Vector2 _externalForce;
		protected Vector2 _newPosition;
		protected Transform _transform;
		protected BoxCollider2D _boxCollider;
		protected GameObject _lastStandingOn;
		protected LayerMask _platformMaskSave;
		protected PathMovement _movingPlatform=null;
		protected float _movingPlatformCurrentGravity;
		protected bool _gravityActive=true;
		protected Collider2D _ignoredCollider = null;

		protected const float _smallValue=0.0001f;
		protected const float _obstacleHeightTolerance=0.05f;
		protected const float _movingPlatformsGravity=-500;

		protected Vector2 _originalColliderSize;
		protected Vector2 _originalColliderOffset;
		protected Vector2 _originalSizeRaycastOrigin;

		protected Vector3 _crossBelowSlopeAngle;

		protected RaycastHit2D[] _sideHitsStorage;	
		protected RaycastHit2D[] _belowHitsStorage;	
		protected RaycastHit2D[] _aboveHitsStorage;	
		protected RaycastHit2D _stickRaycast;

		protected Vector2 _horizontalRayCastFromBottom = Vector2.zero;
		protected Vector2 _horizontalRayCastToTop = Vector2.zero;
		protected Vector2 _verticalRayCastFromLeft = Vector2.zero;
		protected Vector2 _verticalRayCastToRight = Vector2.zero;
		protected Vector2 _aboveRayCastStart = Vector2.zero;
		protected Vector2 _aboveRayCastEnd = Vector2.zero;
		protected Vector2 _stickRayCastOrigin = Vector2.zero;

		protected Vector3 _boundsBottomCenterPosition;
		protected Vector3 _boundsLeftCenterPosition;
		protected Vector3 _boundsRightCenterPosition;
		protected Vector3 _boundsTopCenterPosition;

		protected PathMovement _movingPlatformTest;
		protected SurfaceModifier _frictionTest;

		protected RaycastHit2D[] _raycastNonAlloc = new RaycastHit2D[0];

		// rays parameters
		protected Rect _rayBoundsRectangle;

		protected List<RaycastHit2D> _contactList;

		/// <summary>
		/// initialization
		/// </summary>
		protected virtual void Awake()
		{
			// we get the various components
			_transform=transform;
			_boxCollider = (BoxCollider2D)GetComponent<BoxCollider2D>();
			_originalColliderSize = _boxCollider.size;
			_originalColliderOffset = _boxCollider.offset;

			// we test the boxcollider's x offset. If it's not null we trigger a warning.
			if (_boxCollider.offset.x!=0)
			{
				Debug.LogWarning("The boxcollider for "+gameObject.name+" should have an x offset set to zero. Right now this may cause issues when you change direction close to a wall.");
			}

			// raycast list and state init
			_contactList = new List<RaycastHit2D>();
			State = new CorgiControllerState();

			// we add the edge collider platform and moving platform masks to our initial platform mask so they can be walked on	
			_platformMaskSave = PlatformMask;	
			PlatformMask |= OneWayPlatformMask;
			PlatformMask |= MovingPlatformMask;
			PlatformMask |= MovingOneWayPlatformMask;

			_sideHitsStorage = new RaycastHit2D[NumberOfHorizontalRays];	
			_belowHitsStorage = new RaycastHit2D[NumberOfVerticalRays];	
			_aboveHitsStorage = new RaycastHit2D[NumberOfVerticalRays];

			CurrentWallCollider = null;
			State.Reset();
			SetRaysParameters();
		}

		/// <summary>
		/// Use this to add force to the character
		/// </summary>
		/// <param name="force">Force to add to the character.</param>
		public virtual void AddForce(Vector2 force)
		{
			_speed += force;	
			_externalForce += force;
		}

		/// <summary>
		///  use this to set the horizontal force applied to the character
		/// </summary>
		/// <param name="x">The x value of the velocity.</param>
		public virtual void AddHorizontalForce(float x)
		{
			_speed.x += x;
			_externalForce.x += x;
		}

		/// <summary>
		///  use this to set the vertical force applied to the character
		/// </summary>
		/// <param name="y">The y value of the velocity.</param>
		public virtual void AddVerticalForce(float y)
		{
			_speed.y += y;
			_externalForce.y += y;
		}

		/// <summary>
		/// Use this to set the force applied to the character
		/// </summary>
		/// <param name="force">Force to apply to the character.</param>
		public virtual void SetForce(Vector2 force)
		{
			_speed = force;
			_externalForce = force;	
		}

		/// <summary>
		///  use this to set the horizontal force applied to the character
		/// </summary>
		/// <param name="x">The x value of the velocity.</param>
		public virtual void SetHorizontalForce (float x)
		{
			_speed.x = x;
			_externalForce.x = x;
		}

		/// <summary>
		///  use this to set the vertical force applied to the character
		/// </summary>
		/// <param name="y">The y value of the velocity.</param>
		public virtual void SetVerticalForce (float y)
		{
			_speed.y = y;
			_externalForce.y = y;

		}

		/// <summary>
		/// This is called every frame
		/// </summary>
		protected virtual void Update()
		{	
			EveryFrame();
		}

		/// <summary>
		/// Every frame, we apply the gravity to our character, then check using raycasts if an object's been hit, and modify its new position 
		/// accordingly. When all the checks have been done, we apply that new position. 
		/// </summary>
		protected virtual void EveryFrame()
		{
			ApplyGravity ();
			FrameInitialization ();

			// we initialize our rays
			SetRaysParameters();
			HandleMovingPlatforms();

			// we store our current speed for use in moving platforms mostly
			ForcesApplied = _speed;

			// we cast rays on all sides to check for slopes and collisions
			CastRaysToTheSides();
			CastRaysBelow();	
			CastRaysAbove();

			// we move our transform to its next position
			_transform.Translate(_newPosition,Space.World);			

			SetRaysParameters();	

			ComputeNewSpeed ();


			SetStates ();

			_externalForce.x=0;
			_externalForce.y=0;
		}

		protected virtual void FrameInitialization()
		{
			_contactList.Clear();
			// we initialize our newposition, which we'll use in all the next computations			
			_newPosition = Speed * Time.deltaTime;
			State.WasGroundedLastFrame = State.IsCollidingBelow;
			State.WasTouchingTheCeilingLastFrame = State.IsCollidingAbove;
			CurrentWallCollider = null;
			State.Reset(); 
		}

		protected virtual void ApplyGravity()
		{
			_currentGravity = Parameters.Gravity;
			if (_speed.y > 0)
			{
				_currentGravity = _currentGravity / Parameters.AscentMultiplier;
			}
			if (_speed.y < 0)
			{
				_currentGravity = _currentGravity * Parameters.FallMultiplier;
			}


			if (_gravityActive)
			{
				_speed.y += (_currentGravity + _movingPlatformCurrentGravity) * Time.deltaTime;
			}

			if (_fallSlowFactor!=0)
			{
				_speed.y*=_fallSlowFactor;
			}
		}

		/// <summary>
		/// If the CorgiController is standing on a moving platform, we match its speed
		/// </summary>
		protected virtual void HandleMovingPlatforms()
		{
			if (_movingPlatform!=null)			
			{
				if (!float.IsNaN(_movingPlatform.CurrentSpeed.x) && !float.IsNaN(_movingPlatform.CurrentSpeed.y) && !float.IsNaN(_movingPlatform.CurrentSpeed.z))
				{
					_transform.Translate(_movingPlatform.CurrentSpeed*Time.deltaTime);
				}

				if ( (Time.timeScale==0) || float.IsNaN(_movingPlatform.CurrentSpeed.x) || float.IsNaN(_movingPlatform.CurrentSpeed.y) || float.IsNaN(_movingPlatform.CurrentSpeed.z) )
				{
					return;
				}

				if ((Time.deltaTime<=0))
				{
					return;
				}

				State.OnAMovingPlatform=true;

				GravityActive(false);

				_movingPlatformCurrentGravity=_movingPlatformsGravity;

				_newPosition.y = _movingPlatform.CurrentSpeed.y*Time.deltaTime;		

				_speed = - _newPosition / Time.deltaTime;	
				_speed.x = -_speed.x;

				SetRaysParameters();
			}
		}

		/// <summary>
		/// Disconnects the CorgiController from its current moving platform.
		/// </summary>
		public virtual void DetachFromMovingPlatform()
		{
			GravityActive(true);
			State.OnAMovingPlatform=false;
			_movingPlatform=null;
			_movingPlatformCurrentGravity=0;
		}

		/// <summary>
		/// Casts rays to the sides of the character, from its center axis.
		/// If we hit a wall/slope, we check its angle and move or not according to it.
		/// </summary>
		protected virtual void CastRaysToTheSides() 
		{			
			float movementDirection=1;	
			if ((_speed.x < 0) || (_externalForce.x<0))
				movementDirection = -1;

			if (_movingPlatform != null)
			{
				if (Mathf.Abs(_movingPlatform.CurrentSpeed.x) > Mathf.Abs(_speed.x))
				{
					movementDirection = Mathf.Sign (_movingPlatform.CurrentSpeed.x);
				}
			}

			float horizontalRayLength = Mathf.Abs(_speed.x*Time.deltaTime) + _rayBoundsRectangle.width/2 + RayOffset*2;

			_horizontalRayCastFromBottom.x = _rayBoundsRectangle.center.x;
			_horizontalRayCastFromBottom.y = _rayBoundsRectangle.yMin + _obstacleHeightTolerance;										

			_horizontalRayCastToTop.x = _rayBoundsRectangle.center.x;
			_horizontalRayCastToTop.y = _rayBoundsRectangle.yMax-_obstacleHeightTolerance;				

			if (_sideHitsStorage.Length != NumberOfHorizontalRays)
			{
				_sideHitsStorage = new RaycastHit2D[NumberOfHorizontalRays];	
			}

			for (int i=0; i<NumberOfHorizontalRays;i++)
			{	
				Vector2 rayOriginPoint = Vector2.Lerp(_horizontalRayCastFromBottom,_horizontalRayCastToTop,(float)i/(float)(NumberOfHorizontalRays-1));

				if ( State.WasGroundedLastFrame && i == 0 )			
					_sideHitsStorage[i] = MMDebug.RayCast (rayOriginPoint,movementDirection*(Vector2.right),horizontalRayLength,PlatformMask,Color.red,Parameters.DrawRaycastsGizmos);	
				else
					_sideHitsStorage[i] = MMDebug.RayCast (rayOriginPoint,movementDirection*(Vector2.right),horizontalRayLength,PlatformMask & ~OneWayPlatformMask & ~MovingOneWayPlatformMask,Color.red,Parameters.DrawRaycastsGizmos);			

				if (_sideHitsStorage[i].distance >0)
				{	
					if (_sideHitsStorage[i].collider == _ignoredCollider)
					{
						break;
					}

					float hitAngle = Mathf.Abs(Vector2.Angle(_sideHitsStorage[i].normal, Vector2.up));		

					State.LateralSlopeAngle = hitAngle	;					

					if (hitAngle > Parameters.MaximumSlopeAngle)
					{												
						if (movementDirection < 0)
						{
							State.IsCollidingLeft = true;
						} 
						else
						{
							State.IsCollidingRight = true;						
						}
						CurrentWallCollider = _sideHitsStorage [i].collider.gameObject;

						State.SlopeAngleOK=false;

						if (movementDirection<=0)
						{
							_newPosition.x = -Mathf.Abs(_sideHitsStorage[i].point.x - _horizontalRayCastFromBottom.x) 
								+ _rayBoundsRectangle.width/2 
								+ RayOffset*2;
						}
						else
						{						
							_newPosition.x = Mathf.Abs(_sideHitsStorage[i].point.x - _horizontalRayCastFromBottom.x) 
								- _rayBoundsRectangle.width/2 
								- RayOffset*2;						
						}			

						// if we're in the air, we prevent the character from being pushed back.
						if (!State.IsGrounded)
						{
							_newPosition.x=0;
						}		

						_contactList.Add(_sideHitsStorage[i]);

						_speed.x = 0;

						break;
					}
				}						
			}


		}

		/// <summary>
		/// Every frame, we cast a number of rays below our character to check for platform collisions
		/// </summary>
		protected virtual void CastRaysBelow()
		{
			_friction=0;

			if (_newPosition.y < -_smallValue)
			{
				State.IsFalling=true;
			}
			else
			{
				State.IsFalling = false;
			}

			if ((Parameters.Gravity > 0) && (!State.IsFalling))
				return;

			float rayLength = _rayBoundsRectangle.height/2 + RayOffset ; 	

			if (State.OnAMovingPlatform)
			{
				rayLength*=2;
			}	

			if (_newPosition.y<0)
			{
				rayLength+=Mathf.Abs(_newPosition.y);
			}			

			_verticalRayCastFromLeft.x = _rayBoundsRectangle.xMin + _newPosition.x;
			_verticalRayCastFromLeft.y = _rayBoundsRectangle.center.y + RayOffset;

			_verticalRayCastToRight.x = _rayBoundsRectangle.xMax + _newPosition.x;
			_verticalRayCastToRight.y = _rayBoundsRectangle.center.y + RayOffset;					

			if (_belowHitsStorage.Length != NumberOfVerticalRays)
			{
				_belowHitsStorage = new RaycastHit2D[NumberOfVerticalRays];	
			}


			float smallestDistance=float.MaxValue; 
			int smallestDistanceIndex=0; 						
			bool hitConnected=false; 		

			for (int i=0; i<NumberOfVerticalRays;i++)
			{			
				Vector2 rayOriginPoint = Vector2.Lerp(_verticalRayCastFromLeft, _verticalRayCastToRight, (float)i/(float)(NumberOfVerticalRays-1));

				if ((_newPosition.y>0) && (!State.WasGroundedLastFrame))
					_belowHitsStorage[i] = MMDebug.RayCast (rayOriginPoint,-(Vector2.up),rayLength,PlatformMask & ~OneWayPlatformMask & ~MovingOneWayPlatformMask,Color.blue,Parameters.DrawRaycastsGizmos);	
				else
					_belowHitsStorage[i] = MMDebug.RayCast (rayOriginPoint,-(Vector2.up),rayLength,PlatformMask,Color.blue,Parameters.DrawRaycastsGizmos);					

				if ((Mathf.Abs(_belowHitsStorage[smallestDistanceIndex].point.y - _verticalRayCastFromLeft.y)) <  _smallValue)
				{
					break;
				}		

				if (_belowHitsStorage[i])
				{
					if (_belowHitsStorage[i].collider == _ignoredCollider)
					{
						continue;
					}

					hitConnected=true;
					State.BelowSlopeAngle = Vector2.Angle( _belowHitsStorage[i].normal, Vector2.up )  ;
					_crossBelowSlopeAngle = Vector3.Cross (Vector3.up, _belowHitsStorage [i].normal);
					if (_crossBelowSlopeAngle.z < 0)
					{
						State.BelowSlopeAngle = -State.BelowSlopeAngle;
					}

					if (_belowHitsStorage[i].distance<smallestDistance)
					{
						smallestDistanceIndex=i;
						smallestDistance = _belowHitsStorage[i].distance;
					}
				}								
			}
			if (hitConnected)
			{

				StandingOn = _belowHitsStorage[smallestDistanceIndex].collider.gameObject;
				StandingOnCollider = _belowHitsStorage [smallestDistanceIndex].collider;

				// if the character is jumping onto a (1-way) platform but not high enough, we do nothing
				if (
					!State.WasGroundedLastFrame 
					&& (smallestDistance<_rayBoundsRectangle.size.y/2) 
					&& (
						OneWayPlatformMask.Contains(StandingOn.layer)
						||
						MovingOneWayPlatformMask.Contains(StandingOn.layer)
					) 
				)
				{
					State.IsCollidingBelow=false;
					return;
				}

				State.IsFalling=false;			
				State.IsCollidingBelow=true;


				// if we're applying an external force (jumping, jetpack...) we only apply that
				if (_externalForce.y>0)
				{
					_newPosition.y = _speed.y * Time.deltaTime;
					State.IsCollidingBelow = false;
				}
				// if not, we just adjust the position based on the raycast hit
				else
				{
					_newPosition.y = -Mathf.Abs(_belowHitsStorage[smallestDistanceIndex].point.y - _verticalRayCastFromLeft.y) 
						+ _rayBoundsRectangle.height/2 
						+ RayOffset;
				}

				if (!State.WasGroundedLastFrame && _speed.y>0)
				{
					_newPosition.y += _speed.y * Time.deltaTime;
				}				

				if (Mathf.Abs(_newPosition.y)<_smallValue)
					_newPosition.y = 0;

				// we check if whatever we're standing on applies a friction change
				_frictionTest = _belowHitsStorage[smallestDistanceIndex].collider.gameObject.GetComponentNoAlloc<SurfaceModifier>();
				if (_frictionTest != null)
				{
					_friction=_belowHitsStorage[smallestDistanceIndex].collider.GetComponent<SurfaceModifier>().Friction;
				}

				// we check if the character is standing on a moving platform
				_movingPlatformTest = _belowHitsStorage[smallestDistanceIndex].collider.gameObject.GetComponentNoAlloc<PathMovement>();
				if (_movingPlatformTest != null && State.IsGrounded)
				{
					_movingPlatform=_movingPlatformTest.GetComponent<PathMovement>();
				}
				else
				{
					DetachFromMovingPlatform();
				}
			}
			else
			{
				State.IsCollidingBelow=false;
				if(State.OnAMovingPlatform)
				{
					DetachFromMovingPlatform();
				}
			}	

			if (StickWhenWalkingDownSlopes)
			{
				StickToSlope ();
			}
		}

		/// <summary>
		/// If we're in the air and moving up, we cast rays above the character's head to check for collisions
		/// </summary>
		protected virtual void CastRaysAbove()
		{			
			if (_newPosition.y<0)
				return;

			float rayLength = State.IsGrounded ? RayOffset : _newPosition.y;
			rayLength+=_rayBoundsRectangle.height/2;

			bool hitConnected=false; 

			_aboveRayCastStart.x = _rayBoundsRectangle.xMin + _newPosition.x;
			_aboveRayCastStart.y = _rayBoundsRectangle.center.y;

			_aboveRayCastEnd.x = _rayBoundsRectangle.xMax + _newPosition.x;
			_aboveRayCastEnd.y = _rayBoundsRectangle.center.y;	

			if (_aboveHitsStorage.Length != NumberOfVerticalRays)
			{
				_aboveHitsStorage = new RaycastHit2D[NumberOfVerticalRays];	
			}

			float smallestDistance=float.MaxValue; 

			for (int i=0; i<NumberOfVerticalRays;i++)
			{							
				Vector2 rayOriginPoint = Vector2.Lerp(_aboveRayCastStart,_aboveRayCastEnd,(float)i/(float)(NumberOfVerticalRays-1));
				_aboveHitsStorage[i] = MMDebug.RayCast (rayOriginPoint,(Vector2.up),rayLength,PlatformMask & ~OneWayPlatformMask & ~MovingOneWayPlatformMask,Color.green,Parameters.DrawRaycastsGizmos);	

				if (_aboveHitsStorage[i])
				{
					hitConnected=true;
					if (_aboveHitsStorage[i].collider == _ignoredCollider)
					{
						break;
					}
					if (_aboveHitsStorage[i].distance<smallestDistance)
					{
						smallestDistance = _aboveHitsStorage[i].distance;
					}
				}					
			}	

			if (hitConnected)
			{
				_newPosition.y = smallestDistance - _rayBoundsRectangle.height/2   ;

				if ( (State.IsGrounded) && (_newPosition.y<0) )
				{
					_newPosition.y=0;
				}

				State.IsCollidingAbove=true;

				if (!State.WasTouchingTheCeilingLastFrame)
				{
					_speed = new Vector2(_speed.x, 0f);
				}
			}	
		}

		/// <summary>
		/// If we're going down a slope, sticks the character to the slope to avoid bounces
		/// </summary>
		protected virtual void StickToSlope()
		{
			if ((_newPosition.y >= 0) 
				|| (!StickWhenWalkingDownSlopes)
				|| (!State.WasGroundedLastFrame)
				|| (_externalForce.y > 0)
				|| (_movingPlatform != null))
			{
				return;
			}

			float rayLength = 0;
			if (StickyRaycastLength == 0)
			{
				rayLength = _rayBoundsRectangle.width * Mathf.Abs(Mathf.Tan(Parameters.MaximumSlopeAngle));
				rayLength += _rayBoundsRectangle.height/2 + RayOffset;
			}
			else
			{
				rayLength = StickyRaycastLength;
			}

			_stickRayCastOrigin.x = (_newPosition.x > 0) ? _rayBoundsRectangle.min.x : _rayBoundsRectangle.max.x;
			_stickRayCastOrigin.x += _newPosition.x;

			_stickRayCastOrigin.y = _rayBoundsRectangle.center.y + RayOffset;

			_stickRaycast = MMDebug.RayCast (_stickRayCastOrigin, Vector2.down, rayLength,PlatformMask,Color.yellow,Parameters.DrawRaycastsGizmos);	

			if (_stickRaycast)
			{
				if (_stickRaycast.collider == _ignoredCollider)
				{
					return;
				}

				_newPosition.y = -Mathf.Abs(_stickRaycast.point.y - _stickRayCastOrigin.y) 
					+ _rayBoundsRectangle.height/2 
					+ RayOffset;

				State.IsCollidingBelow=true;
			}	
		}

		protected virtual void ComputeNewSpeed()
		{
			// we compute the new speed
			if (Time.deltaTime > 0)
			{
				_speed = _newPosition / Time.deltaTime;	
			}	

			// we apply our slope speed factor based on the slope's angle
			if (State.IsGrounded)
			{
				_speed.x *= Parameters.SlopeAngleSpeedFactor.Evaluate(Mathf.Abs(State.BelowSlopeAngle) * Mathf.Sign(_speed.y));
			}

			if (!State.OnAMovingPlatform)				
			{
				// we make sure the velocity doesn't exceed the MaxVelocity specified in the parameters
				_speed.x = Mathf.Clamp(_speed.x,-Parameters.MaxVelocity.x,Parameters.MaxVelocity.x);
				_speed.y = Mathf.Clamp(_speed.y,-Parameters.MaxVelocity.y,Parameters.MaxVelocity.y);
			}
		}

		protected virtual void SetStates()
		{
			// we change states depending on the outcome of the movement
			if( !State.WasGroundedLastFrame && State.IsCollidingBelow )
			{
				State.JustGotGrounded=true;
			}

			if (State.IsCollidingLeft || State.IsCollidingRight || State.IsCollidingBelow || State.IsCollidingAbove)
			{
				OnCorgiColliderHit();
			}	
		}

		/// <summary>
		/// Creates a rectangle with the boxcollider's size for ease of use and draws debug lines along the different raycast origin axis
		/// </summary>
		public virtual void SetRaysParameters() 
		{		
			_rayBoundsRectangle.xMin = _boxCollider.bounds.min.x;
			_rayBoundsRectangle.yMin = _boxCollider.bounds.min.y;
			_rayBoundsRectangle.xMax = _rayBoundsRectangle.xMin + _boxCollider.bounds.size.x;
			_rayBoundsRectangle.yMax = _rayBoundsRectangle.yMin + _boxCollider.bounds.size.y;
		}

		public virtual void SetIgnoreCollider(Collider2D newIgnoredCollider)
		{
			_ignoredCollider = newIgnoredCollider;
		}

		/// <summary>
		/// Disables the collisions for the specified duration
		/// </summary>
		/// <param name="duration">the duration for which the collisions must be disabled</param>
		public virtual IEnumerator DisableCollisions(float duration)
		{
			// we turn the collisions off
			CollisionsOff();
			// we wait for a few seconds
			yield return new WaitForSeconds (duration);
			// we turn them on again
			CollisionsOn();
		}

		/// <summary>
		/// Resets the collision mask with the default settings
		/// </summary>
		public virtual void CollisionsOn()
		{
			PlatformMask=_platformMaskSave;
			PlatformMask |= OneWayPlatformMask;
			PlatformMask |= MovingPlatformMask;
			PlatformMask |= MovingOneWayPlatformMask;
		}

		/// <summary>
		/// Turns all collisions off
		/// </summary>
		public virtual void CollisionsOff()
		{
			PlatformMask=0;
		}

		/// <summary>
		/// Disables the collisions with one way platforms for the specified duration
		/// </summary>
		/// <param name="duration">the duration for which the collisions must be disabled</param>
		public virtual IEnumerator DisableCollisionsWithOneWayPlatforms(float duration)
		{
			if (DetachmentMethod == DetachmentMethods.Layer)
			{
				// we turn the collisions off
				CollisionsOffWithOneWayPlatformsLayer ();
				// we wait for a few seconds
				yield return new WaitForSeconds (duration);
				// we turn them on again
				CollisionsOn();	
			}
			else
			{
				// we set our current platform collider as ignored
				SetIgnoreCollider (StandingOnCollider);
				// we wait for a few seconds
				yield return new WaitForSeconds (duration);
				// we turn clear it
				SetIgnoreCollider (null);	
			}
		}

		/// <summary>
		/// Disables the collisions with moving platforms for the specified duration
		/// </summary>
		/// <param name="duration">the duration for which the collisions must be disabled</param>
		public virtual IEnumerator DisableCollisionsWithMovingPlatforms(float duration)
		{
			if (DetachmentMethod == DetachmentMethods.Layer)
			{
				// we turn the collisions off
				CollisionsOffWithMovingPlatformsLayer ();
				// we wait for a few seconds
				yield return new WaitForSeconds (duration);
				// we turn them on again
				CollisionsOn ();
			}
			else
			{
				// we set our current platform collider as ignored
				SetIgnoreCollider (StandingOnCollider);
				// we wait for a few seconds
				yield return new WaitForSeconds (duration);
				// we turn clear it
				SetIgnoreCollider (null);				
			}
		}

		/// <summary>
		/// Disables collisions only with the one way platform layers
		/// </summary>
		public virtual void CollisionsOffWithOneWayPlatformsLayer()
		{

			PlatformMask -= OneWayPlatformMask;
			PlatformMask -= MovingOneWayPlatformMask;
		}

		/// <summary>
		/// Disables collisions only with moving platform layers
		/// </summary>
		public virtual void CollisionsOffWithMovingPlatformsLayer()
		{
			PlatformMask -= MovingPlatformMask;
			PlatformMask -= MovingOneWayPlatformMask;
		}

		/// <summary>
		/// Resets all overridden parameters.
		/// </summary>
		public virtual void ResetParameters()
		{
			_overrideParameters = DefaultParameters;
		}

		/// <summary>
		/// Slows the character's fall by the specified factor.
		/// </summary>
		/// <param name="factor">Factor.</param>
		public virtual void SlowFall(float factor)
		{
			_fallSlowFactor=factor;
		}

		/// <summary>
		/// Activates or desactivates the gravity for this character only.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, activates the gravity. If set to <c>false</c>, turns it off.</param>	   
		public virtual void GravityActive(bool state)
		{
			if (state)
			{
				_gravityActive = true;
			}
			else
			{
				_gravityActive = false;
			}
		}

		/// <summary>
		/// Resizes the collider to the new size set in parameters
		/// </summary>
		/// <param name="newSize">New size.</param>
		public virtual void ResizeCollider(Vector2 newSize)
		{
			float newYOffset =_originalColliderOffset.y -  (_originalColliderSize.y - newSize.y)/2 ;

			_boxCollider.size = newSize;
			_boxCollider.offset = newYOffset*Vector3.up;
			SetRaysParameters();
		}

		/// <summary>
		/// Returns the collider to its initial size
		/// </summary>
		public virtual void ResetColliderSize()
		{
			_boxCollider.size = _originalColliderSize;
			_boxCollider.offset = _originalColliderOffset;
			SetRaysParameters();
		}

		/// <summary>
		/// Determines whether this instance can go back to original size.
		/// </summary>
		/// <returns><c>true</c> if this instance can go back to original size; otherwise, <c>false</c>.</returns>
		public virtual bool CanGoBackToOriginalSize()
		{
			// if we're already at original size, we return true
			if (_boxCollider.size == _originalColliderSize)
			{
				return true;
			}
			float headCheckDistance = _originalColliderSize.y*transform.localScale.y ;

			// we cast two rays above our character to check for obstacles. If we didn't hit anything, we can go back to original size, otherwise we can't
			_originalSizeRaycastOrigin.x = _boxCollider.bounds.min.x;
			_originalSizeRaycastOrigin.y = _boxCollider.bounds.max.y + _smallValue;
			bool headCheckLeft = MMDebug.RayCast(_originalSizeRaycastOrigin,Vector2.up,headCheckDistance,PlatformMask,Color.cyan,true);

			_originalSizeRaycastOrigin.x = _boxCollider.bounds.max.x;
			bool headCheckRight = MMDebug.RayCast(_originalSizeRaycastOrigin,Vector2.up,headCheckDistance,PlatformMask,Color.cyan,true);
			if (headCheckLeft || headCheckRight)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Returns the character's bounds width
		/// </summary>
		public virtual float Width()
		{
			return _rayBoundsRectangle.width;
		}

		/// <summary>
		/// Returns the character's bounds height
		/// </summary>
		public virtual float Height()
		{
			return _rayBoundsRectangle.height;
		}

		/// <summary>
		/// triggered when the character's raycasts collide with something 
		/// </summary>
		protected virtual void OnCorgiColliderHit() 
		{
			foreach (RaycastHit2D hit in _contactList )
			{	
				if (Parameters.Physics2DInteraction)
				{
					Rigidbody2D body = hit.collider.attachedRigidbody;
					if (body == null || body.isKinematic || body.bodyType == RigidbodyType2D.Static)
						return;

					Vector3 pushDir = new Vector3(_externalForce.x, 0, 0);

					body.velocity = pushDir.normalized * Parameters.Physics2DPushForce;		
				}	
			}		
		}

		/// <summary>
		/// triggered when the character enters a collider
		/// </summary>
		/// <param name="collider">the object we're colliding with.</param>
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{

			CorgiControllerPhysicsVolume2D parameters = collider.gameObject.GetComponentNoAlloc<CorgiControllerPhysicsVolume2D>();
			if (parameters == null)
				return;
			// if the object we're colliding with has parameters, we apply them to our character.
			_overrideParameters = parameters.ControllerParameters;
		}

		/// <summary>
		/// triggered while the character stays inside another collider
		/// </summary>
		/// <param name="collider">the object we're colliding with.</param>
		protected virtual void OnTriggerStay2D( Collider2D collider )
		{
		}

		/// <summary>
		/// triggered when the character exits a collider
		/// </summary>
		/// <param name="collider">the object we're colliding with.</param>
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{		
			CorgiControllerPhysicsVolume2D parameters = collider.gameObject.GetComponentNoAlloc<CorgiControllerPhysicsVolume2D>();
			if (parameters == null)
				return;

			// if the object we were colliding with had parameters, we reset our character's parameters
			_overrideParameters = null;
		}				
	}
}