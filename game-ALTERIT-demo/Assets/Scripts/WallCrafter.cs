using UnityEngine;
using System.Collections;

public class WallCrafter : MonoBehaviour {

    public Transform regBrick, regCon, regNexus;
    public Transform obliBrick, obliCon, obliGlue, obliNexus;
    Vector2 vertexPos, crosshairLocalPos, crosshairAbsPos;

    public Transform[] linkedVertices;

	void Awake() {

        if (true) {

            vertexPos = transform.position;

            foreach (Transform linkedVertexTransform in linkedVertices) {

                crosshairAbsPos = linkedVertexTransform.position;
                crosshairLocalPos = linkedVertexTransform.position - transform.position;
                CraftWall();        
            }
        }
	}
    Transform instantiatedBrick;
    Quaternion rotId = Quaternion.identity;

	void CraftWall() {

        if (crosshairLocalPos.x == 0) {

            CraftVertical();

        } else if (crosshairLocalPos.y == 0) {

            CraftHorizontal();

        } else {

            CraftOblique();
        }
	}

    // Vertical Wall

    void CraftVertical() {

        float regBrickY;

        bool up = crosshairLocalPos.y > 0 ? true : false;
        int units = (int)(Mathf.Abs(crosshairLocalPos.y));

        if (units == 2) {

            PlaceVerticalRegNexus(up);

        } else {

            for (int i = 1; i < units; i++) {

                if (i == 1) {

                    PlaceVerticalRegConnector(up, true);

                } else if (i > 1 && i < units - 1) {

                    if (up) {
                        regBrickY = vertexPos.y + i;
                    } else {
                        regBrickY = vertexPos.y - i;
                    }

                    instantiatedBrick = Instantiate(regBrick, new Vector3(vertexPos.x, regBrickY, 0f), rotId) as Transform;
                    instantiatedBrick.parent = transform;

                } else {

                    PlaceVerticalRegConnector(up, false);
                }
            }
        }
    }

    void PlaceVerticalRegConnector(bool up, bool firstCon) {

        Quaternion regConRot;
        float regConY;
    
        if (up) {  

            if (firstCon) {
                regConY = vertexPos.y + 0.96f;
                regConRot = rotId;
            } else {
                regConY = crosshairAbsPos.y - 0.96f;
                regConRot = Quaternion.Euler(new Vector3(0, 0, 180));
            }

        } else {

            if (firstCon) {
                regConY = vertexPos.y - 0.96f;
                regConRot = Quaternion.Euler(new Vector3(0, 0, 180));
            } else {
                regConY = crosshairAbsPos.y + 0.96f;
                regConRot = rotId;
            }
        }
          
        instantiatedBrick = Instantiate(regCon, new Vector3(vertexPos.x, regConY, 0f), regConRot) as Transform;
        instantiatedBrick.parent = transform;
    }

    void PlaceVerticalRegNexus(bool up) {

        float regNexusY;

        if (up) {
            regNexusY = vertexPos.y + 1f;
        } else {
            regNexusY = vertexPos.y - 1f;
        }

        instantiatedBrick = Instantiate(regNexus, new Vector3(vertexPos.x, regNexusY, 0f), rotId) as Transform;
        instantiatedBrick.parent = transform;
    }

    // Horizontal Wall

    void CraftHorizontal() {

        float regBrickX;
        Quaternion regBrickRot = Quaternion.Euler(new Vector3(0, 0, 90));

        bool right = crosshairLocalPos.x > 0 ? true : false;
        int units = (int)(Mathf.Abs(crosshairLocalPos.x));

        if (units == 2) {

            PlaceHorizontalRegNexus(right);

        } else {

            for (int i = 1; i < units; i++) {

                if (i == 1) {

                    PlaceHorizontalRegConnector(right, true);

                } else if (i > 1 && i < units - 1) {

                    if (right) {
                        regBrickX = vertexPos.x + i;
                    } else {
                        regBrickX = vertexPos.x - i;
                    }

                    instantiatedBrick = Instantiate(regBrick, new Vector3(regBrickX, vertexPos.y, 0f), regBrickRot) as Transform;
                    instantiatedBrick.parent = transform;

                } else {

                    PlaceHorizontalRegConnector(right, false);
                }
            }
        }
    }

    void PlaceHorizontalRegConnector(bool right, bool firstCon) {

        Quaternion regConRot;
        float regConX;

        if (right) {

            if (firstCon) {
                regConX = vertexPos.x + 0.96f;
                regConRot = Quaternion.Euler(new Vector3(0, 0, -90));
            } else {
                regConX = crosshairAbsPos.x - 0.96f;
                regConRot = Quaternion.Euler(new Vector3(0, 0, 90));
            }

        } else {

            if (firstCon) {
                regConX = vertexPos.x - 0.96f;
                regConRot = Quaternion.Euler(new Vector3(0, 0, 90));
            } else {
                regConX = crosshairAbsPos.x + 0.96f;
                regConRot = Quaternion.Euler(new Vector3(0, 0, -90));
            }
        }

        instantiatedBrick = Instantiate(regCon, new Vector3(regConX, vertexPos.y, 0f), regConRot) as Transform;
        instantiatedBrick.parent = transform;
    }

    void PlaceHorizontalRegNexus(bool right) {

        float regNexusX;
        Quaternion regNexusRot = Quaternion.Euler(new Vector3(0, 0, 90));

        if (right) {
            regNexusX = vertexPos.x + 1f;
        } else {
            regNexusX = vertexPos.x - 1f;
        }

        instantiatedBrick = Instantiate(regNexus, new Vector3(regNexusX, vertexPos.y, 0f), regNexusRot) as Transform;
        instantiatedBrick.parent = transform;
    }

    // Oblique Wall

    void CraftOblique() {

        float units = Mathf.Abs(crosshairLocalPos.x);
        int quadrant = WhichQuadrant();

        if (units == 1) {

            PlaceObliqueNexus(quadrant);

        } else {

            for (float i = 0.5f; i < units; i += 0.5f) {

                if (i == 0.5f) {

                    PlaceObliqueConnector(quadrant, true);

                } else if (i > 0.5f && i <= units - 1f) {

                    if (i % 1 == 0) {

                        PlaceObliqueGlue(quadrant, i);

                    } else {

                        PlaceObliqueBrick(quadrant, i);
                    }

                } else {

                    PlaceObliqueConnector(quadrant, false);
                }
            }
        }
    }

    int WhichQuadrant() {

        bool right = crosshairLocalPos.x > 0 ? true : false;
        bool up = crosshairLocalPos.y > 0 ? true : false;

        if (up & right) { // Q1

            return 1;

        } else if (up & !right) { // Q2

            return 2;

        } else if (!up & !right) { // Q3

            return 3;

        } else if (!up & right) { // Q4

            return 4;
        }

        return 0;
    }

    void PlaceObliqueConnector(int quadrant, bool firstCon) {

        Vector3 obliConPos = Vector3.zero;
        Quaternion obliConRot = rotId;

        switch (quadrant) {

            case 1:
                if (firstCon) {
                    obliConPos = new Vector2(vertexPos.x + 0.5f, vertexPos.y + 0.5f);
                    obliConRot = rotId;
                } else {
                    obliConPos = new Vector2(crosshairAbsPos.x - 0.5f, crosshairAbsPos.y - 0.5f);
                    obliConRot = Quaternion.Euler(new Vector3(0, 0, 180));
                }
                break;
            case 2:
                if (firstCon) {
                    obliConPos = new Vector2(vertexPos.x - 0.5f, vertexPos.y + 0.5f);
                    obliConRot = Quaternion.Euler(new Vector3(0, 0, 90));
                } else {
                    obliConPos = new Vector2(crosshairAbsPos.x + 0.5f, crosshairAbsPos.y - 0.5f);
                    obliConRot = Quaternion.Euler(new Vector3(0, 0, -90));
                }
                break;
            case 3:
                if (firstCon) {
                    obliConPos = new Vector2(vertexPos.x - 0.5f, vertexPos.y - 0.5f);
                    obliConRot = Quaternion.Euler(new Vector3(0, 0, 180));
                } else {
                    obliConPos = new Vector2(crosshairAbsPos.x + 0.5f, crosshairAbsPos.y + 0.5f);
                    obliConRot = rotId;
                }
                break;
            case 4:
                if (firstCon) {
                    obliConPos = new Vector2(vertexPos.x + 0.5f, vertexPos.y - 0.5f);
                    obliConRot = Quaternion.Euler(new Vector3(0, 0, -90));
                } else {
                    obliConPos = new Vector2(crosshairAbsPos.x - 0.5f, crosshairAbsPos.y + 0.5f);
                    obliConRot = Quaternion.Euler(new Vector3(0, 0, 90));
                }
                break;
        }

        instantiatedBrick = Instantiate(obliCon, obliConPos, obliConRot) as Transform;
        instantiatedBrick.parent = transform;
    }

    void PlaceObliqueBrick(int quadrant, float i) {

        Vector3 obliBrickPos = Vector3.zero;
        Quaternion obliBrickRot = rotId;

        switch (quadrant) {

            case 1:
                obliBrickPos = new Vector2(vertexPos.x + i, vertexPos.y + i);
                obliBrickRot = rotId;
                break;
            case 2:
                obliBrickPos = new Vector2(vertexPos.x - i, vertexPos.y + i);
                obliBrickRot = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case 3:
                obliBrickPos = new Vector2(vertexPos.x - i, vertexPos.y - i);
                obliBrickRot = rotId;
                break;
            case 4:
                obliBrickPos = new Vector2(vertexPos.x + i, vertexPos.y - i);
                obliBrickRot = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
        }

        instantiatedBrick = Instantiate(obliBrick, obliBrickPos, obliBrickRot) as Transform;
        instantiatedBrick.parent = transform;
    }

    void PlaceObliqueGlue(int quadrant, float i) {

        Vector3 obliGluePos = Vector3.zero;
        Quaternion obliBrickRot = rotId;

        switch (quadrant) {

            case 1:
                obliGluePos = new Vector2(vertexPos.x + i, vertexPos.y + i);
                obliBrickRot = rotId;
                break;
            case 2:
                obliGluePos = new Vector2(vertexPos.x - i, vertexPos.y + i);
                obliBrickRot = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case 3:
                obliGluePos = new Vector2(vertexPos.x - i, vertexPos.y - i);
                obliBrickRot = rotId;
                break;
            case 4:
                obliGluePos = new Vector2(vertexPos.x + i, vertexPos.y - i);
                obliBrickRot = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
        }

        instantiatedBrick = Instantiate(obliGlue, obliGluePos, obliBrickRot) as Transform;
        instantiatedBrick.parent = transform;
    }

    void PlaceObliqueNexus(int quadrant) {

        Vector3 obliNexusPos = Vector3.zero;
        Quaternion obliNexusRot = rotId;

        switch (quadrant) {

            case 1:
                obliNexusPos = new Vector2(vertexPos.x + 0.5f, vertexPos.y + 0.5f);
                obliNexusRot = rotId;
                break;
            case 2:
                obliNexusPos = new Vector2(vertexPos.x - 0.5f, vertexPos.y + 0.5f);
                obliNexusRot =  Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case 3:
                obliNexusPos = new Vector2(vertexPos.x - 0.5f, vertexPos.y - 0.5f);
                obliNexusRot = rotId;
                break;
            case 4:
                obliNexusPos = new Vector2(vertexPos.x + 0.5f, vertexPos.y - 0.5f);
                obliNexusRot = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
        }

        instantiatedBrick = Instantiate(obliNexus, obliNexusPos, obliNexusRot) as Transform;
        instantiatedBrick.parent = transform;

    }
}
