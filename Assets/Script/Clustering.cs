using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class Clustering : MonoBehaviour
{
    [SerializeField]private GameObject point;
    [SerializeField]private GameObject center;
    [SerializeField]private List<UnityEngine.Color> color;
    private static List<Transform> points = new List<Transform>();
    private static List<Cluster> clusters = new List<Cluster>();

    static public bool inMoving = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (Input.GetMouseButtonDown(0) && !inMoving)
            addPoint();

        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(KMean(10));
    }

    private void addPoint() {
        points.Add(Instantiate(point, Camera.main.ScreenToWorldPoint(Input.mousePosition)+new Vector3(0,0,10), Quaternion.identity).transform);
        //Debug.Log("point added count:" + points.Count);

    }
    public static void deletePoint(GameObject g) {
        points.Remove(g.transform);
        //Debug.Log("point deleted count:"+ points.Count);
    }

    float distance(Vector3 t1, Vector3 t2) {
        return Mathf.Pow(Mathf.Pow(t1.x - t2.x, 2) + Mathf.Pow(t1.y - t2.y, 2), 0.5f);
    }

    bool isEqual(List<Cluster> c1, List<Cluster> c2) {
        if (c1.Count != c2.Count)
            return false;
        for (int i = 0; i < c1.Count; i++) {
            bool b = isEqual(c1[i].getPoints(), c2[i].getPoints());
            if (!b)
                return b;
        }
        return true;
    
    }

    bool isEqual(List<Transform> t1, List<Transform> t2) {
        if (t1.Count != t2.Count)
            return false;
        for (int i = 0; i < t1.Count; i++) {
            if (t1[i].position != t2[i].position)
                return false;
        }
        return true;
        
    }

    bool isEqual(List<Vector3> c1, List<Vector3> c2) {
        if (c1.Count != c2.Count)
            return false;
        for (int i = 0; i < c1.Count; i++)
        {
            if (c1[i] != c2[i])
                return false;
        }
        return true;

    }
    void destroyTransform(List<Cluster> c) {
        foreach (Cluster cl in c) {
            Destroy(cl.getCenter().gameObject);
            cl.getPoints().Clear();
        }
            

    }


    void changeColor(UnityEngine.Color c) {
        foreach (Transform t in points) {
            t.GetComponent<SpriteRenderer>().color = c;
        }
    }

    void destroyAllPoints() {
        foreach (Transform t in points) { 
            Destroy (t.gameObject);
        }
        points.Clear();
    }
    bool isCalculating = false;
    IEnumerator KMean(int k) {
        if (!isCalculating) {
            isCalculating = true;
            destroyTransform(clusters);
            clusters = new List<Cluster>();
            //List<Cluster> newClusters = new List<Cluster>();
            List<Vector3> newCenters = new List<Vector3>();
            List<int> usedIndex = new List<int>();
            for (int i = 0; i < k; i++)
            { // chooseing K points to create the clusters from
                
                int x = Random.Range(0, points.Count-1);
                
                while (usedIndex.Contains(x))
                    x = Random.Range(0, points.Count - 1);
                usedIndex.Add(x);
                newCenters.Add(points[x].transform.position);
                clusters.Add(new Cluster(Instantiate(center, newCenters[i], Quaternion.identity).transform, new List<Transform>(), color[(i % color.Count)]));

            }
            changeColor(UnityEngine.Color.white);
            List<Vector3> centers = new List<Vector3>();
            int iteration = 0;
            yield return new WaitForSecondsRealtime(0.5f);
            while (!isEqual(centers, newCenters) && iteration <= 20)
            {
                iteration++;
                //newClusters = new List<Cluster>();


                foreach (Transform g in points)
                {
                    Cluster nearest = clusters[0];
                    for (int i = 1; i < k; i++)
                    {
                        if (distance(nearest.getCenter().position, g.transform.position) > distance(clusters[i].getCenter().position, g.transform.position))
                            nearest = clusters[i];
                    }
                    nearest.addPoint(g);
                }
                //clusters = newClusters;
                
                // calulate the new center
                centers = newCenters;
                newCenters = new List<Vector3>();
                for (int i=0;i<clusters.Count; i++)
                {
                    Vector3 total = Vector3.zero;
                    foreach (Transform t in clusters[i].getPoints())
                    {
                        total += t.position;
                    }
                    Vector3 v = total / (clusters[i].getPoints().Count);

                    newCenters.Add(v);
                    clusters[i].setCenterPos(v);
                    clusters[i].getPoints().Clear();
                    ;
                }
                Debug.Log("Clusters:" + clusters[0] +"\titeration:"+iteration+ "\tcenters:" + centers.Count  );
                //newClusters = new List<Cluster>();
                yield return new WaitForSecondsRealtime(0.5f);
            }

            isCalculating = false;
        }

    }

}

class Cluster
{
    Transform center;
    List<Transform> points;
    UnityEngine.Color color;
    public Cluster(Transform center, List<Transform> points, UnityEngine.Color color)
    {
        this.center = center;
        this.points = points;
        this.color = color;
        this.center.GetComponent<SpriteRenderer>().color = color;
    }

    public Transform getCenter() { return center; }
    public void setCenter(Transform center) { this.center = center; }
    public void setCenterPos(Vector3 v) { this.center.position = v; }
    public List<Transform> getPoints() { return points; }
    public void setPoints(List<Transform> points) { this.points = points; }
    public void addPoint(Transform point) { 
        points.Add(point);
        point.GetComponent<SpriteRenderer>().color = color;
    }
    public UnityEngine.Color getColor() { 
        return color;
    }

    override public string ToString() {

        return "center:" + center + "\tpoints:" + points.Count + "\tcolor:" + color;
    }
}
