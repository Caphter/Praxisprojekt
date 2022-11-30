﻿namespace ExtendedColliders3D {
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    [AddComponentMenu("Physics/Extended Colliders 3D")]
    public class ExtendedColliders3D : MonoBehaviour {

        //Enumerated Types.
        public enum ColliderType {
            Circle = 0,
            CircleHalf = 1,
            Cone = 2,
            ConeHalf = 3,
            Cube = 4,
            Cylinder = 5,
            CylinderHalf = 6,
            Dodecahedron = 10,
            Icosahedron = 11,
            Octahedron = 12,
            Quad = 7,
            Sphere = 8,
            Triangle = 9
        };
        public enum GizmoType { None, Position, Rotation, Scale };

        //Classes;
        [Serializable]
        public class ExtendedCollders3DProperties {

            //Mesh collider properties.
            public bool convex;
            public bool isTrigger;
            public MeshColliderCookingOptions meshColliderCookingOptions =
                MeshColliderCookingOptions.CookForFasterSimulation |
                MeshColliderCookingOptions.EnableMeshCleaning |
                MeshColliderCookingOptions.WeldColocatedVertices |
                MeshColliderCookingOptions.UseFastMidphase;
            public PhysicMaterial material;

            //General collider properties.
            public ColliderType colliderType = ColliderType.Cylinder;
            public Vector3 centre = Vector3.zero;
            public Vector3 rotation = Vector3.zero;
            public Vector3 size = Vector3.one;
            public bool flipFaces = false;

            //Collider-specific properties.
            public int circleVertices = 16;
            public bool circleTwoSided = false;
            public int coneFaces = 16;
            public bool coneCap = true;
            public bool coneHalfCapFlatEnd = true;
            public bool cubeTopFace = true;
            public bool cubeBottomFace = true;
            public bool cubeLeftFace = true;
            public bool cubeRightFace = true;
            public bool cubeForwardFace = true;
            public bool cubeBackFace = true;
            public int cylinderFaces = 16;
            public bool cylinderCapTop = true;
            public bool cylinderCapBottom = true;
            public Vector2 cylinderTaperTop = Vector2.one;
            public Vector2 cylinderTaperBottom = Vector2.one;
            public bool cylinderHalfCapFlatEnd = true;
            public bool quadTwoSided = false;
            public bool triangleTwoSided = false;
            public int sphereStacks = 8;
            public int sphereSlices = 16;

            //Editor properties.
            public Color colour = new Color32(145, 244, 140, 239);
            public GizmoType gizmoType = GizmoType.None;
        }

        //Properties.
        public ExtendedCollders3DProperties properties = new ExtendedCollders3DProperties();

        //Reset.
        void Reset() {
            autoSizeColliderToMeshFilter();
        }

        //Awake.
        void Awake() {

            //Generate the mesh collider from the properties.
            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.enabled = enabled;
            meshCollider.sharedMesh = generateMesh(false);
            meshCollider.convex = properties.convex;
            meshCollider.isTrigger = properties.isTrigger;
            meshCollider.cookingOptions = properties.meshColliderCookingOptions;
            meshCollider.material = properties.material;

            //Remove this component.
            Destroy(this);
        }

        //Start.
        void Start() { }

        //Draw gizmos, if selected.
#if UNITY_EDITOR
        void OnDrawGizmosSelected() {

            //Set the mesh colour - depends on whether the mesh is enabled.
            Gizmos.color = enabled ? properties.colour : new Color(properties.colour.r * 0.5286f, properties.colour.g * 0.8197f, properties.colour.b * 0.5286f,
                    properties.colour.a * 0.3513f);

            //Draw the mesh.
            Vector3[] vertices;
            int[] triangles;
            generateVerticesAndTriangles(true, out vertices, out triangles);
            for (int i = 0; i < triangles.Length / 3; i++) {
                Gizmos.DrawLine(vertices[triangles[i * 3]], vertices[triangles[(i * 3) + 1]]);
                Gizmos.DrawLine(vertices[triangles[(i * 3) + 1]], vertices[triangles[(i * 3) + 2]]);
                Gizmos.DrawLine(vertices[triangles[(i * 3) + 2]], vertices[triangles[i * 3]]);
            }
        }
#endif

        //Generate the mesh collider.
        Mesh generateMesh(bool applyTransform) {

            //Create a new mesh and initialise vertices and triangles arrays.
            Mesh mesh = new Mesh();
            mesh.name = "Extended Colliders 3D Mesh";
            Vector3[] vertices;
            int[] triangles;
            generateVerticesAndTriangles(applyTransform, out vertices, out triangles);

            //Set the vertices and triangles on the mesh, recalculate the normals and return the mesh.
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }

        //Generate the vertices and triangles for a mesh.
        void generateVerticesAndTriangles(bool applyTransform, out Vector3[] vertices, out int[] triangles) {
            int nextTriangle = 0;

            //Circles.
            if (properties.colliderType == ColliderType.Circle || properties.colliderType == ColliderType.CircleHalf) {
                int circleVertices = properties.circleVertices + (properties.colliderType == ColliderType.Circle ? 0 : 1);
                vertices = new Vector3[circleVertices];
                triangles = new int[(circleVertices - 2) * (properties.circleTwoSided ? 6 : 3)];
                List<int> circleVerticesList = new List<int>();
                for (int i = 0; i < circleVertices; i++) {
                    float angle = ((float) i / properties.circleVertices) * Mathf.PI * (properties.colliderType == ColliderType.Circle ? 2 : 1);
                    vertices[i] = new Vector3(Mathf.Sin(angle) / 2, -0.5f, Mathf.Cos(angle) / 2);
                    circleVerticesList.Add(i);
                }
                int arrayPosition = circleVerticesList.Count / 2;
                bool previousVertex = false;
                while (circleVerticesList.Count > 2) {
                    triangles[nextTriangle++] = circleVerticesList[(arrayPosition + circleVerticesList.Count - 1) % circleVerticesList.Count];
                    triangles[nextTriangle++] = circleVerticesList[arrayPosition];
                    triangles[nextTriangle++] = circleVerticesList[((arrayPosition + 1) % circleVerticesList.Count)];
                    circleVerticesList.RemoveAt(arrayPosition);
                    if (previousVertex)
                        arrayPosition = (arrayPosition + circleVerticesList.Count - 1) % circleVerticesList.Count;
                    previousVertex = !previousVertex;
                }
                if (properties.circleTwoSided)
                    for (int i = 0; i < circleVertices - 2; i++) {
                        triangles[(i + circleVertices - 2) * 3] = triangles[i * 3];
                        triangles[((i + circleVertices - 2) * 3) + 1] = triangles[(i * 3) + 2];
                        triangles[((i + circleVertices - 2) * 3) + 2] = triangles[(i * 3) + 1];
                    }
            }

            //Cones.
            else if (properties.colliderType == ColliderType.Cone || properties.colliderType == ColliderType.ConeHalf) {
                int coneFaces = properties.coneFaces + (properties.colliderType == ColliderType.Cone ? 0 : 1);
                vertices = new Vector3[coneFaces + 1];
                triangles = new int[(coneFaces * 3) + (properties.coneCap ? ((coneFaces - 2) * 3) : 0) -
                        (properties.colliderType != ColliderType.Cone && !properties.coneHalfCapFlatEnd ? 3 : 0)];
                vertices[vertices.Length - 1] = new Vector3(0, 0.5f, 0);
                for (int i = 0; i < coneFaces; i++) {
                    float angle = ((float) i / properties.coneFaces) * Mathf.PI * (properties.colliderType == ColliderType.Cone ? 2 : 1);
                    vertices[i] = new Vector3(Mathf.Sin(angle) / 2, -0.5f, Mathf.Cos(angle) / 2);
                    if (properties.colliderType == ColliderType.Cone || properties.coneHalfCapFlatEnd || i < coneFaces - 1) {
                        triangles[nextTriangle++] = i;
                        triangles[nextTriangle++] = (i + 1) % coneFaces;
                        triangles[nextTriangle++] = vertices.Length - 1;
                    }
                }
                if (properties.coneCap) {
                    List<int> capVertices = new List<int>();
                    for (int i = 0; i < coneFaces; i++)
                        capVertices.Add(i);
                    int arrayPosition = capVertices.Count / 2;
                    bool previousVertex = false;
                    while (capVertices.Count > 2) {
                        triangles[nextTriangle++] = capVertices[(arrayPosition + capVertices.Count - 1) % capVertices.Count];
                        triangles[nextTriangle++] = capVertices[((arrayPosition + 1) % capVertices.Count)];
                        triangles[nextTriangle++] = capVertices[arrayPosition];
                        capVertices.RemoveAt(arrayPosition);
                        if (previousVertex)
                            arrayPosition = (arrayPosition + capVertices.Count - 1) % capVertices.Count;
                        previousVertex = !previousVertex;
                    }
                }
            }

            //Cubes.
            else if (properties.colliderType == ColliderType.Cube) {
                vertices = new Vector3[8];
                triangles = new int[(properties.cubeTopFace ? 6 : 0) + (properties.cubeBottomFace ? 6 : 0) + (properties.cubeLeftFace ? 6 : 0) +
                        (properties.cubeRightFace ? 6 : 0) + (properties.cubeForwardFace ? 6 : 0) + (properties.cubeBackFace ? 6 : 0)];
                int nextVertex = 0;
                for (int i = -1; i <= 1; i += 2)
                    for (int j = -1; j <= 1; j += 2)
                        for (int k = -1; k <= 1; k += 2)
                            vertices[nextVertex++] = new Vector3((float) j / 2, (float) i / 2, (float) k / 2);
                if (properties.cubeBottomFace) {
                    triangles[nextTriangle++] = 0;
                    triangles[nextTriangle++] = 2;
                    triangles[nextTriangle++] = 1;
                    triangles[nextTriangle++] = 1;
                    triangles[nextTriangle++] = 2;
                    triangles[nextTriangle++] = 3;
                }
                if (properties.cubeTopFace) {
                    triangles[nextTriangle++] = 4;
                    triangles[nextTriangle++] = 5;
                    triangles[nextTriangle++] = 6;
                    triangles[nextTriangle++] = 6;
                    triangles[nextTriangle++] = 5;
                    triangles[nextTriangle++] = 7;
                }
                if (properties.cubeLeftFace) {
                    triangles[nextTriangle++] = 0;
                    triangles[nextTriangle++] = 1;
                    triangles[nextTriangle++] = 4;
                    triangles[nextTriangle++] = 4;
                    triangles[nextTriangle++] = 1;
                    triangles[nextTriangle++] = 5;
                }
                if (properties.cubeRightFace) {
                    triangles[nextTriangle++] = 3;
                    triangles[nextTriangle++] = 2;
                    triangles[nextTriangle++] = 6;
                    triangles[nextTriangle++] = 3;
                    triangles[nextTriangle++] = 6;
                    triangles[nextTriangle++] = 7;
                }
                if (properties.cubeBackFace) {
                    triangles[nextTriangle++] = 0;
                    triangles[nextTriangle++] = 4;
                    triangles[nextTriangle++] = 2;
                    triangles[nextTriangle++] = 4;
                    triangles[nextTriangle++] = 6;
                    triangles[nextTriangle++] = 2;
                }
                if (properties.cubeForwardFace) {
                    triangles[nextTriangle++] = 1;
                    triangles[nextTriangle++] = 3;
                    triangles[nextTriangle++] = 5;
                    triangles[nextTriangle++] = 5;
                    triangles[nextTriangle++] = 3;
                    triangles[nextTriangle++] = 7;
                }
            }

            //Cylinders.
            else if (properties.colliderType == ColliderType.Cylinder || properties.colliderType == ColliderType.CylinderHalf) {
                int cylinderFaces = properties.cylinderFaces + (properties.colliderType == ColliderType.Cylinder ? 0 : 1);
                vertices = new Vector3[cylinderFaces * 2];
                triangles = new int[(cylinderFaces * 6) + (properties.cylinderCapTop ? ((cylinderFaces - 2) * 3) : 0) +
                        (properties.cylinderCapBottom ? ((cylinderFaces - 2) * 3) : 0) -
                        (properties.colliderType != ColliderType.Cylinder && !properties.cylinderHalfCapFlatEnd ? 6 : 0)];
                for (int i = 0; i < cylinderFaces; i++) {
                    float angle = ((float) i / properties.cylinderFaces) * Mathf.PI * (properties.colliderType == ColliderType.Cylinder ? 2 : 1);
                    vertices[i] = new Vector3(Mathf.Sin(angle) / 2, 0.5f, Mathf.Cos(angle) / 2);
                    vertices[i + cylinderFaces] = vertices[i] + new Vector3(0, -1, 0);
                    vertices[i].x *= properties.cylinderTaperTop.x;
                    vertices[i].z *= properties.cylinderTaperTop.y;
                    vertices[i + cylinderFaces].x *= properties.cylinderTaperBottom.x;
                    vertices[i + cylinderFaces].z *= properties.cylinderTaperBottom.y;
                    if (properties.colliderType == ColliderType.Cylinder || properties.cylinderHalfCapFlatEnd || i < cylinderFaces - 1) {
                        triangles[nextTriangle++] = i;
                        triangles[nextTriangle++] = i + cylinderFaces;
                        triangles[nextTriangle++] = (i + 1) % cylinderFaces;
                        triangles[nextTriangle++] = (i + 1) % cylinderFaces;
                        triangles[nextTriangle++] = i + cylinderFaces;
                        triangles[nextTriangle++] = Mathf.Max((i + cylinderFaces + 1) % (cylinderFaces * 2), cylinderFaces);
                    }
                }
                for (int j = 0; j < 2; j++)
                    if ((j == 0 && properties.cylinderCapTop) || (j == 1 && properties.cylinderCapBottom)) {
                        List<int> capVertices = new List<int>();
                        for (int i = 0; i < cylinderFaces; i++)
                            capVertices.Add(i + (cylinderFaces * j));
                        int arrayPosition = capVertices.Count / 2;
                        bool previousVertex = false;
                        while (capVertices.Count > 2) {
                            triangles[nextTriangle++] = capVertices[(arrayPosition + capVertices.Count - 1) % capVertices.Count];
                            triangles[nextTriangle++] = capVertices[j == 0 ? arrayPosition : ((arrayPosition + 1) % capVertices.Count)];
                            triangles[nextTriangle++] = capVertices[j == 1 ? arrayPosition : ((arrayPosition + 1) % capVertices.Count)];
                            capVertices.RemoveAt(arrayPosition);
                            if (previousVertex)
                                arrayPosition = (arrayPosition + capVertices.Count - 1) % capVertices.Count;
                            previousVertex = !previousVertex;
                        }
                    }
            }

            //Quads.
            else if (properties.colliderType == ColliderType.Quad) {
                vertices = new Vector3[] { new Vector3(-0.5f, 0, -0.5f), new Vector3(-0.5f, 0, 0.5f), new Vector3(0.5f, 0, -0.5f), new Vector3(0.5f, 0, 0.5f) };
                triangles = new int[properties.quadTwoSided ? 12 : 6];
                triangles[nextTriangle++] = 0;
                triangles[nextTriangle++] = 1;
                triangles[nextTriangle++] = 2;
                triangles[nextTriangle++] = 3;
                triangles[nextTriangle++] = 2;
                triangles[nextTriangle++] = 1;
                if (properties.quadTwoSided) {
                    triangles[nextTriangle++] = 0;
                    triangles[nextTriangle++] = 2;
                    triangles[nextTriangle++] = 1;
                    triangles[nextTriangle++] = 3;
                    triangles[nextTriangle++] = 1;
                    triangles[nextTriangle++] = 2;
                }
            }

            //Triangles.
            else if (properties.colliderType == ColliderType.Triangle) {
                vertices = new Vector3[] { new Vector3(-0.5f, 0, -0.5f), new Vector3(-0.5f, 0, 0.5f), new Vector3(0.5f, 0, -0.5f) };
                triangles = new int[properties.triangleTwoSided ? 6 : 3];
                triangles[nextTriangle++] = 0;
                triangles[nextTriangle++] = 1;
                triangles[nextTriangle++] = 2;
                if (properties.triangleTwoSided) {
                    triangles[nextTriangle++] = 0;
                    triangles[nextTriangle++] = 2;
                    triangles[nextTriangle++] = 1;
                }
            }

            //Spheres.
            else if (properties.colliderType == ColliderType.Sphere) {
                vertices = new Vector3[((properties.sphereStacks - 1) * properties.sphereSlices) + 2];
                triangles = new int[properties.sphereStacks * properties.sphereSlices * 6];
                vertices[0] = new Vector3(0, 0.5f, 0);
                vertices[vertices.Length - 1] = new Vector3(0, -0.5f, 0);
                for (int i = 1; i < properties.sphereStacks; i++) {
                    float stackRadius = Mathf.Sin(((float) i / properties.sphereStacks) * Mathf.PI);
                    float stackHeight = Mathf.Cos(((float) i / properties.sphereStacks) * Mathf.PI) / 2;
                    for (int j = 0; j < properties.sphereSlices; j++) {
                        int vertexIndex = ((i - 1) * properties.sphereSlices) + j + 1;
                        int nextVertexIndex = j == properties.sphereSlices - 1 ? ((i - 1) * properties.sphereSlices) + 1 : (vertexIndex + 1);
                        float angle = ((float) j / properties.sphereSlices) * Mathf.PI * 2;
                        vertices[vertexIndex] = new Vector3((Mathf.Sin(angle) / 2) * stackRadius, stackHeight, (Mathf.Cos(angle) / 2) * stackRadius);
                        if (i == 1) {
                            triangles[j * 3] = 0;
                            triangles[(j * 3) + 1] = vertexIndex;
                            triangles[(j * 3) + 2] = nextVertexIndex;
                        }
                        else {
                            int triangleIndex = (properties.sphereSlices * 3) + ((i - 2) * properties.sphereSlices * 6) + (j * 6);
                            triangles[triangleIndex] = vertexIndex;
                            triangles[triangleIndex + 1] = nextVertexIndex;
                            triangles[triangleIndex + 2] = vertexIndex - properties.sphereSlices;
                            triangles[triangleIndex + 3] = nextVertexIndex - properties.sphereSlices;
                            triangles[triangleIndex + 4] = vertexIndex - properties.sphereSlices;
                            triangles[triangleIndex + 5] = nextVertexIndex;
                            if (i == properties.sphereStacks - 1) {
                                triangleIndex = (properties.sphereSlices * 3) + ((properties.sphereStacks - 2) * properties.sphereSlices * 6) + (j * 3);
                                triangles[triangleIndex] = nextVertexIndex;
                                triangles[triangleIndex + 1] = vertexIndex;
                                triangles[triangleIndex + 2] = vertices.Length - 1;
                            }
                        }
                    }
                }
            }

            //Octahedron.
            else if (properties.colliderType == ColliderType.Octahedron) {
                vertices = new Vector3[] {
                    new Vector3(0, 0.5f, 0),
                    new Vector3(0.5f, 0, 0.5f),
                    new Vector3(0.5f, 0, -0.5f),
                    new Vector3(-0.5f, 0, -0.5f),
                    new Vector3(-0.5f, 0, 0.5f),
                    new Vector3(0, -0.5f, 0)
                };
                triangles = new int[] {
                    0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1,
                    5, 2, 1, 5, 3, 2, 5, 4, 3, 5, 1, 4
                };
            }

            //Dodecahedrons.
            else if (properties.colliderType == ColliderType.Dodecahedron) {
                float faceAngle = Mathf.Acos(Mathf.Sqrt(5f) / 5f) * Mathf.Rad2Deg;
                float edgeDistance = (Mathf.Sqrt(5f) + 1f) / 8f;
                float edgeLength = -edgeDistance / ((1f / (Mathf.Sqrt(5f) + 1f)) + 1f);
                vertices = new Vector3[20];
                triangles = new int[108];
                int[] triangleVertexIndices = new int[5];
                int nextVertexIndex = 0;
                int nextTriangleIndex = 0;
                for (int i = 0; i < 12; i++) {
                    Quaternion faceRotation = Quaternion.Euler(
                        0,
                        i == 0 || i == 11 ? 0 : (((i % 5) * 72) + (i < 6 ? 36 : 0)),
                        i == 0 ? 0 : (i < 6 ? faceAngle : (i < 11 ? 180 - faceAngle : 180))
                    );
                    Vector3 faceCentre = faceRotation * new Vector3(0, edgeDistance, 0);
                    for (int j = 0; j < 5; j++) {
                        Vector3 vertex = (faceRotation *
                            Quaternion.Euler(
                                0,
                                (j * 72) + (i < 6 || i == 11 ? 0 : 36),
                                0
                            ) * new Vector3(edgeLength, 0, 0)) + faceCentre;
                        triangleVertexIndices[j] = -1;
                        for (int k = 0; k < nextVertexIndex; k++)
                            if (Mathf.Abs(vertices[k].x - vertex.x) < 0.0001f && Mathf.Abs(vertices[k].y - vertex.y) < 0.0001f &&
                                    Mathf.Abs(vertices[k].z - vertex.z) < 0.0001f) {
                                triangleVertexIndices[j] = k;
                                break;
                            }
                        if (triangleVertexIndices[j] == -1) {
                            vertices[nextVertexIndex] = vertex;
                            triangleVertexIndices[j] = nextVertexIndex;
                            nextVertexIndex++;
                        }
                        if (j > 1) {
                            triangles[nextTriangleIndex++] = triangleVertexIndices[0];
                            triangles[nextTriangleIndex++] = triangleVertexIndices[j - 1];
                            triangles[nextTriangleIndex++] = triangleVertexIndices[j];
                        }
                    }
                }
            }

            //Icosahedrons.
            else if (properties.colliderType == ColliderType.Icosahedron) {
                vertices = new Vector3[12];
                float goldenRatio = (Mathf.Sqrt(5f) + 1f) / 2f;
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 4; j++) {
                        float x = ((j % 2) * 2) - 1;
                        float y = goldenRatio * (j < 2 ? 1 : -1);
                        vertices[(i * 4) + j] = new Vector3(
                            i == 0 ? x : (i == 1 ? 0 : y),
                            i == 0 ? y : (i == 1 ? x : 0),
                            i == 0 ? 0 : (i == 1 ? y : x)
                        ) * (0.5f / goldenRatio);
                    }
                triangles = new int[] {
                    0, 11, 5, 0, 5, 1, 0, 1, 7, 0, 7, 10, 0, 10, 11,
                    1, 5, 9, 5, 11, 4, 11, 10, 2, 10, 7, 6, 7, 1, 8,
                    3, 9, 4, 3, 4, 2, 3, 2, 6, 3, 6, 8, 3, 8, 9,
                    4, 9, 5, 2, 4, 11, 6, 2, 10, 8, 6, 7, 9, 8, 1
                };
            }

            //Unknown type.
            else
                throw new Exception("Extended Colliders 3D: Unknown collider type.");

            //Adjust the vertices to account for position and size.
            for (int i = 0; i < vertices.Length; i++) {
                vertices[i].x *= properties.size.x;
                vertices[i].y *= properties.size.y;
                vertices[i].z *= properties.size.z;
                vertices[i] = Quaternion.Euler(properties.rotation) * vertices[i];
                vertices[i] += properties.centre;
                if (applyTransform) {
                    Transform currentTransform = transform;
                    while (currentTransform != null) {
                        vertices[i].x *= currentTransform.localScale.x;
                        vertices[i].y *= currentTransform.localScale.y;
                        vertices[i].z *= currentTransform.localScale.z;
                        vertices[i] = currentTransform.localRotation * vertices[i];
                        vertices[i] += currentTransform.localPosition;
                        currentTransform = currentTransform.parent;
                    }
                }
            }

            //Flip faces, if required.
            if (properties.flipFaces)
                for (int i = 0; i < triangles.Length / 3; i++) {
                    int triangleIndexSwap = triangles[i * 3];
                    triangles[i * 3] = triangles[(i * 3) + 1];
                    triangles[(i * 3) + 1] = triangleIndexSwap;
                }
        }

        //Auto-size the extended colliders 3D component to the mesh filter, if there is one.
        public void autoSizeColliderToMeshFilter() {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = null;
            if (meshFilter != null)
                mesh = meshFilter.sharedMesh;
            if (mesh != null) {
                properties.centre = mesh.bounds.center;
                properties.size = mesh.bounds.size;
            }
        }
    }
}