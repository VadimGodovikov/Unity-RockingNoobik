using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using Unity.VisualScripting;
using UnityEngine;

public class LassoController : MonoBehaviour
{
    private vThirdPersonController playerController;

    public Material materialLine;
    public GameObject player;
    public Transform ropeStartPoint;
    public Transform finishPoint;
    public float ropeSpeed = 2f;
    public int strengthPoints = 1;

    public List<GameObject> cubeVariations;

    private GameObject cubePrefab;
    private GameObject currentCube;
    private LineRenderer ropeRenderer;
    private bool isCubeAttached = false;

    System.Random rand = new System.Random();

    private void Start()
    {
        ropeRenderer = GetComponent<LineRenderer>();
        playerController = GetComponent<vThirdPersonController>();
    }

    private void Update()
    {
        if (!isCubeAttached && Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
            {
                if (hit.collider.CompareTag("TableTree"))
                {
                    cubePrefab = cubeVariations.ToArray()[0];
                    currentCube = Instantiate(cubePrefab, hit.point + new Vector3(0f, 2f, -1.5f), Quaternion.identity);
                    AttachCubeWithRope();
                    isCubeAttached = true;
                    ropeSpeed = 1.5f;
                    playerController.freeSpeed.runningSpeed = 2.5f;
                    playerController.freeSpeed.sprintSpeed = 3.5f;
                }
                if(strengthPoints >= 25)
                {
                    if (hit.collider.CompareTag("TableWord"))
                    {
                        cubePrefab = cubeVariations.ToArray()[1];
                        currentCube = Instantiate(cubePrefab, hit.point + new Vector3(0f, 2f, -1.5f), Quaternion.identity);
                        AttachCubeWithRope();
                        isCubeAttached = true;
                        ropeSpeed = 1f;
                        playerController.freeSpeed.runningSpeed = 1f;
                        playerController.freeSpeed.sprintSpeed = 2f;
                    }
                }
            }
        }

        if (isCubeAttached && Vector3.Distance(player.transform.position, finishPoint.position) < 1f)
        {
            Destroy(currentCube);
            isCubeAttached = false;
            if (cubeVariations[0] == cubePrefab)
            {
                strengthPoints += rand.Next(5, 11);
            }
            if (cubeVariations[1] == cubePrefab)
            {
                strengthPoints += rand.Next(100, 150);
            }
            Debug.Log("Points + " + strengthPoints);
            Destroy(ropeRenderer);
            ropeRenderer = null;
            ropeSpeed = 2f;
            playerController.freeSpeed.runningSpeed = 4f;
            playerController.freeSpeed.sprintSpeed = 6f;
        }
    }

    private void FixedUpdate()
    {
        if (isCubeAttached)
        {
            ropeRenderer.SetPosition(0, ropeStartPoint.position);
            ropeRenderer.SetPosition(1, currentCube.transform.position);
            if (Vector3.Distance(currentCube.transform.position, ropeStartPoint.position) > 2f)
            {
                Vector3 ropeDirection = (ropeStartPoint.position - currentCube.transform.position).normalized;
                float distanceToMove = ropeSpeed * Time.deltaTime;
                currentCube.transform.position += ropeDirection * distanceToMove;
            }

        }
    }

    private void AttachCubeWithRope()
    {
        GameObject rope = new GameObject("Rope");
        rope.transform.position = ropeStartPoint.position;
        rope.AddComponent<LineRenderer>();
        ropeRenderer = rope.GetComponent<LineRenderer>();
        ropeRenderer.material = materialLine;
        ropeRenderer.startColor = Color.white;
        ropeRenderer.endColor = Color.white;
        ropeRenderer.startWidth = 0.1f;
        ropeRenderer.endWidth = 0.1f;
        ropeRenderer.SetPosition(0, ropeStartPoint.position);
        ropeRenderer.SetPosition(1, currentCube.transform.position);
    }
}

