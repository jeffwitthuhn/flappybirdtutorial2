# flappybirdtutorial

new project
2d

Assets -> show in explorer
download https://www.spriters-resource.com/resources/sheets/57/59894.png
put sprite sheet into Assets folder

in unity editor
Project->assets
click on the sprite sheet

in the inspector window
texture type Sprite 
Sprite mode multiple
click sprite editor

in sprite editor
dropdown slice in top left
type automatic
click slice
click apply

in assets you should now have 80 sprites (dropdown the sprite sheet) 

drag any sprite, maybe a bird sprite, say 74 into the scene tab inside the camera box
this will create a game object
drag a background sprite into the scene

position the background at (0,0,0) and then move the bird to (0,0,-1) so that it is on top of the background. 

click the camera and scale the box around to fit the background vertically. 

press play to see how it will look. (for other aspect ratios you can switch build type to android in (file-build settings)
===============

now lets add some behavior

click on the bird ad in the inspector: 
add component -> rigidbody 2D

now press play
the bird should fall!

lets add a ground. 
drag a ground sprite into the scene window and position at (0,-1,-1)
(for collisions to work you must have a rigid body on one of the objects)
click on the bird and add component -> circlecollider 2D (I'm using a radius of .07)
click on the ground and add component -> box collider 2D


===============

now lets make the bird flap when we click! 
FINALLY SOME PROGRAMMING
(I've given up and am conforming to using visual studio for unity, its just more simple to go with the flow) 
Behaviors are attached to individual game objects. 
We will add an event listener to the bird so that it knows when the player clicks.
When the player clicks we will make the bird go up instead of down
.
click on the bird -> add component -> New Script -> cs script ( called mine bird )

click on the script name in the menu to open the program associated with .cs files.. 

**begin visual studio rant
visual studio in my case
i fucking hate visual studio because it its bloaty spamware that takes forever to load. yet im using it. whatever.
monodevelop is a good alternative as it works nice with unity. 
or just sublime
or atom aparently is nice.
 but when you install unity it auto installs visual studio 
which is annoying af and might as well use it at that point 
**end visual studio rant

you will have a file template for you unity scripting. 
most of your things you will do will be using the UnityEngine API

start() happens when the game object is first initialized.
In this case right when we press play. In other cases when you spawn objects.

Update() happens every game tickish something. and will be called on every object that
has an update function in some order.

gameObject references the GameObject that the script is attached to. 

You can modify the position of a game object by assigning the gameObject.transform.position variable
to some other Vector3 for example. 

Here we will set the velocity of the bird to be going up when clicked or space

``` c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    Rigidbody2D birdRB;
    public float upspeed = 0.001f; //public allows you to change in inspector
    Vector3 up = new Vector3(0, 1, 0);

  // Use this for initialization
  void Start () {
        birdRB = gameObject.GetComponent<Rigidbody2D>(); //get the rigid body
  }
  
  // Update is called once per frame
  void Update () {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) // if space or right mouse button clicked
        {
            birdRB.velocity = up * upspeed;
        }
  }
}
```

You the code above attached to your player should allow you to right click or
tap space to go up.

====================================

Now we want the bird to move.. 

we add to the velocity a forward component

set forward speed to 1 and upspeed to 3 in the inspector.

``` c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    Rigidbody2D birdRB;
    public float upspeed = 3f;
    public float forewardspeed = 1f;
    Vector3 up = new Vector3(0, 1, 0);
    Vector3 right = new Vector3(1, 0, 0);
  // Use this for initialization
  void Start () {
        birdRB = gameObject.GetComponent<Rigidbody2D>();
  }
  
  // Update is called once per frame
  void Update () {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) 
        {
            birdRB.velocity = up * upspeed + right * forewardspeed;
        }
  }
}
```

your bird should fall to the round really fast but when jumping will move forward.

=====================================

lets freeze the bird until the user presses something. 

make a start state variable, and once it is pressed then set it to true and 
the gravity scale to 0.7f;

set gravity scale to 0 in inspector of the rigidbody.

``` c#
public class bird : MonoBehaviour {
    Rigidbody2D birdRB;
    public float upspeed = 0.001f;
    public float forewardspeed = 0.001f;
    Vector3 up = new Vector3(0, 1, 0);
    Vector3 right = new Vector3(1, 0, 0);
    bool start; 
  // Use this for initialization
  void Start () {
        start = false; 
        birdRB = gameObject.GetComponent<Rigidbody2D>();
  }
  
  // Update is called once per frame
  void Update () {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) 
        {
            birdRB.velocity = up * upspeed + right * forewardspeed;
            if (!start)
            {
                birdRB.gravityScale = 0.7f;
                start = true;
            }
        }
  }
}

```
Now you should be able to play with the speeds and tweak how you want your bird to move. 

Next, lets have the background follow the bird

set up 2 more background blocks in front of the bird. and 1 behind so that they cover the whole camera as well as extras in front.

you can copy and paste. 

do the same with the floor, copy paste

Precicise placing: 
each background block should have a position (1.43*k, 0, 0) with k =  -1, 0, 1, 2, 3

each floor should have (1.674*k,-1,-1) for k = -1, 0, 1, 2, 3


=========================================

Lets make the camera follow the bird
(we could just attach the camera to the bird, but we dont want it to move vertical.. )

add a new component to the main camera -> New Script 

will call it cameraFollow

``` c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {
    Vector3 pos;
    public GameObject player; //drag player onto this field in inspector
  // Use this for initialization
  
  // Update is called once per frame
  void Update () {
        pos = player.transform.position;
        pos.z = -10;
        pos.y = gameObject.transform.position.y;
        gameObject.transform.position = pos;
  }
}
```

The camera should follow the bird as you would expect in flappy bird like game. when you press play.
=================================================================
This game is infinite so no matter how many backgrounds we paste we'll eventually run out.. 
so we should reuse the ones we already have. 

Here, we'll do this with trigger colliders.

set each collider to a trigger collider

on each background, add a box colider of width 1, height 2.56 , and x offset of 3
(you can highlight them all and do this in one go or one at a time)

we will have it so when we cross these triggers, we will teleport the 
corresponding game object 4*1.43

name each of the backgrounds "background" so we can handle that colision specifically. 


in the bird script, we'll handle the colision. 

add this function inside of the class:
``` c#
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "background")
        {
            Vector3 pos = collision.gameObject.transform.position;
            pos.x += 4 * 1.42f;
            collision.gameObject.transform.position = pos;
        }
    }
```
this will be called when the bird's collider enters a 'trigger' collider. 

the collision object references the object that it is connected to with the 
collision.gameObject


With this script, you you should see a log every time you collide with the 
trigger and the backgrounds should be teleported forward to give you
an infinite loop of background. 


Next, do the same thing with the floors: 

name all floors to: "floor"

add a SECOND box collider to each:
make the collider a trigger
make the side 1x2.56 and x-offset 3 and y-offset 1

we'll now modify the previous function to include the floors 
(we could just teleport "Any" collider but this shows that
we can do different things with different triggers if we want)

``` c#
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("triggered " + collision.gameObject.name);
        if (collision.gameObject.name == "background" 
            || collision.gameObject.name == "floor")
        {
            Vector3 pos = collision.gameObject.transform.position;
            if (collision.gameObject.name == "floor" )
            {
                pos.x += 4 * 1.674f; 
            }
            else
            {
                pos.x += 4 * 1.42f;
            }
            collision.gameObject.transform.position = pos;
        }
    }
```


=============================

now lets make the game end when we hit the ground. 
we'll use OnCollisionEnter2D and set a global "dead"
variable determine if the game is over. 

in the update function, only accept input if the bird is not dead
on collision enter set dead to true

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    Rigidbody2D birdRB;
    public float upspeed = 0.001f;
    public float forewardspeed = 0.001f;
    Vector3 up = new Vector3(0, 1, 0);
    Vector3 right = new Vector3(1, 0, 0);
    Vector3 zero = new Vector3(0, 0, 0);
    bool start;
    bool dead;
  // Use this for initialization
  void Start () {
        start = false;
        dead = false;
        birdRB = gameObject.GetComponent<Rigidbody2D>();
  }
  
  // Update is called once per frame
  void Update () {
        if(!dead)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                birdRB.velocity = up * upspeed + right * forewardspeed;
                if (!start)
                {
                    birdRB.gravityScale = 0.7f;
                    start = true;
                }
            }
        }
        else
        {
            birdRB.velocity = zero;
        }
        
  }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collide with " + collision.gameObject.name);
        if (collision.gameObject.name == "floor")
        {
            birdRB.velocity = zero;
            dead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("triggered " + collision.gameObject.name);
        if (collision.gameObject.name == "background" 
            || collision.gameObject.name == "floor")
        {
            Vector3 pos = collision.gameObject.transform.position;
             if (collision.gameObject.name == "pipes" || collision.gameObject.name == "floor" )
            {
                if(collision.gameObject.name == "pipes")
                {
                    float y = Random.Range(-0.7f, 0.7f);
                    pos.y = y;
                }
                pos.x += 4 * 1.674f; 
            }
            else
            {
                pos.x += 4 * 1.42f;
            }
            collision.gameObject.transform.position = pos;
        }
    }
}

```

The bird will now stop when it hits the ground. 

===========

Next we will add a ceiling finally.. 

on every background, add a new box collider (non trigger)
set height to 1 and y offset to 1.8

that should do the trick


============


next we will add the pipes
we will have a pipe at every ground increment, starting at the second ground 
piece. it will be teleported like like the ground and the background, 
except that when it is teleported it will also randomize the gap height.

drag two pipes into the scene, flip one upside down call them both "pipe"
add an empty game object to the scene from hierchy->create empty
drag the pip pieces onto the empty object in the heirchy and call it "pipes"
position the pipes and game object at (0,0, -1)
drag the pipes apart to the desired gap. 
(i chose the bottom at -1.2y and top at 1.2y)
add a collider to each individual pipe (non trigger)
(fit around pipe)
add a collider to the game object parent (trigger) 
height = 10 width = 1 x offset = 3

copy and paste 3 more pipes and place each pipes object at (1.674*k,-1,-1) 
for k =  1, 2, 3, 4 (not zero

name the objects all "pipe"

next, we'll have the "OnTriggerEnter" teleport the "pipes" like the other two,
but also randomize the y position up or down of the parent "pipes" object
(note: you could also do weird things and change the gaps of the children)

Also "onCollisionEnter " will include "pipe" to the death condition.

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    Rigidbody2D birdRB;
    public float upspeed = 0.001f;
    public float forewardspeed = 0.001f;
    Vector3 up = new Vector3(0, 1, 0);
    Vector3 right = new Vector3(1, 0, 0);
    Vector3 zero = new Vector3(0, 0, 0);
    bool start;
    bool dead;
  // Use this for initialization
  void Start () {
        start = false;
        dead = false;
        birdRB = gameObject.GetComponent<Rigidbody2D>();
  }
  
  // Update is called once per frame
  void Update () {
        if(!dead)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                birdRB.velocity = up * upspeed + right * forewardspeed;
                if (!start)
                {
                    birdRB.gravityScale = 0.7f;
                    start = true;
                }
            }
        }
        else
        {
            birdRB.velocity = zero;
        }
        
  }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collide with " + collision.gameObject.name);
        if (collision.gameObject.name == "floor"
            || collision.gameObject.name == "pipe")
        {
            birdRB.velocity = zero;
            dead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("triggered " + collision.gameObject.name);
        if (collision.gameObject.name == "background" 
            || collision.gameObject.name == "floor"
            || collision.gameObject.name == "pipes")
        {
            Vector3 pos = collision.gameObject.transform.position;
            
            if (collision.gameObject.name == "pipes" || collision.gameObject.name == "floor" )
            {
                if(collision.gameObject.name == "pipes")
                {
                    float y = Random.Range(-0.7f, 0.7f);
                    pos.y = y;
                }
                pos.x += 4 * 1.674f; 
            }
            else
            {
                pos.x += 4 * 1.42f;
            }

            collision.gameObject.transform.position = pos;

        }


    }
}

```






