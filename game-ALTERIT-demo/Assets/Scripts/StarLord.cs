using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarLord : MonoBehaviour {

    public static bool starLordHasCreatedTheStars;

    public Transform star;
    
    Transform genStar;

    public int generatorSeed;
    public float genXmin, genXmax, genYmin, genYmax;
    public Transform[] layersOfStars;
    public int[] starsPerLayer;

    public enum starTypePerLayer { oneSize, twoSizes, threeSizes };
    public starTypePerLayer[] layerStarType;
    public float[] layerScale;
    public Vector2[] layerAscendingSizeFreq;
    public float[] layerTwinklingStarsFreq;


    void Start() {

        SetBoundaries();

        Random.seed = generatorSeed;

        for (int i = 0; i < layersOfStars.Length; i++) {

            for (int j = 0; j < starsPerLayer[i]; j++) {

                switch (layerStarType[i]) {

                    case starTypePerLayer.oneSize:
                        PlaceStar(1.0f, i);
                        break;

                    case starTypePerLayer.twoSizes:

                        if (Random.Range(0.0f, 1.0f) < layerAscendingSizeFreq[i].x) {

                            PlaceStar(1.0f, i);

                        } else if (Random.Range(0.0f, 1.0f) < layerAscendingSizeFreq[i].y) {

                            PlaceStar(2.0f, i);

                        }
                        break;

                    case starTypePerLayer.threeSizes:

                        if (Random.Range(0.0f, 1.0f) < layerAscendingSizeFreq[i].x) {

                            PlaceStar(1.0f, i);

                        } else if (Random.Range(0.0f, 1.0f) < layerAscendingSizeFreq[i].y) {

                            PlaceStar(2.0f, i);

                        } else {

                            PlaceStar(2.5f, i);
                        }
                        break;
                }
            }
        }

        starLordHasCreatedTheStars = true;
    }

    void PlaceStar(float starScale, int layerOfStars) {

        genStar = Instantiate(star, new Vector3(Random.Range(genXmin, genXmax), Random.Range(genYmin, genYmax), 0.0f), Quaternion.identity) as Transform;
        genStar.parent = layersOfStars[layerOfStars].transform;
        genStar.localScale *= layerScale[layerOfStars] * starScale;

        if (Random.Range(0.0f, 1.0f) < layerTwinklingStarsFreq[layerOfStars]) {

            genStar.GetComponent<StarTwinkler>().twinklingStar = true;
        }
    }

    void Update() {

        starLordHasCreatedTheStars = false;
    }

    void SetBoundaries() {

        genXmin = transform.position.x - genXmin;
        genXmax = transform.position.x + genXmax;
        genYmin = transform.position.y - genYmin;
        genYmax = transform.position.y + genYmax;
    }
}
