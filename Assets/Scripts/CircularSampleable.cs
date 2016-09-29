using UnityEngine;
using System.Collections;

public class CircularSampleable : MonoBehaviour, IVoxelGridSampleable {

    public float radius;

    CircleCollider2D _collider;
    Rigidbody2D _rbd2d;


    void Awake()
    {
        _collider = gameObject.AddComponent<CircleCollider2D>();
        _collider.radius = radius;
        _collider.offset = Vector2.zero;
        _collider.isTrigger = true;

        _rbd2d = gameObject.AddComponent<Rigidbody2D>();
        _rbd2d.gravityScale = 0f;
        _rbd2d.drag = 0;

    }

	// Use this for initialization
	void Start () {

        _rbd2d.AddForce(new Vector2(Random.value * 2, Random.value * 2), ForceMode2D.Impulse);
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //World borders
        float halfWidth = VoxelSampleManager.width / 2;
        float halfHeight = VoxelSampleManager.height / 2;

        float xPos = transform.position.x;
        float yPos = transform.position.y;

        //Bounce off right side wall
        if ( xPos + radius > halfWidth)
        {
            _reflVelocity(Vector2.left);
        }

        //Bounce of left side wall
        if (xPos - radius < (-halfWidth))
        {
            _reflVelocity(Vector2.right);
        }

        //Bounce of top wall
        if (yPos + radius > halfHeight)
        {
            _reflVelocity(Vector2.down);
        }

        //Bounce off bottom wall
        if (yPos - radius < (-halfHeight))
        {
            _reflVelocity(Vector2.up);
        }
	}

    //Utility function to calculate reflected velocity
    void _reflVelocity(Vector2 normal)
    {
        Vector2 vel = _rbd2d.velocity;
        Vector2 reflVel = Vector2.Reflect(vel, normal);
        _rbd2d.velocity = reflVel;
    }

    public float GetSampleAt(Vector2 voxelCenter)
    {
        float vx = voxelCenter.x;
        float vy = voxelCenter.y;

        float cx = transform.position.x;
        float cy = transform.position.y;
        return (radius * radius) / ((vx - cx) * (vx - cx ) + (vy - cy ) * (vy  - cy )) ;
    }
}
