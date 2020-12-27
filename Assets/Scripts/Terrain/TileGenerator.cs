using UnityEngine;
	
public class TileGenerator : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private TerrainGenerator terrainGenerator;
    private int depth;
    private int width;

    void Awake()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
    }

    public void GenerateTile(TerrainGenerator terrainGenerator)
    {
        this.terrainGenerator = terrainGenerator;

        // calculate tile depth and width based on the mesh vertices
        Vector3[] meshVertices = meshFilter.mesh.vertices;
        depth = (int)Mathf.Sqrt(meshVertices.Length);
        width = depth;

        // calculate the offsets based on the tile position
        float offsetX = -gameObject.transform.position.x / gameObject.transform.localScale.x;
        float offsetZ = -gameObject.transform.position.z / gameObject.transform.localScale.z;
        
        // generate a heightMap using Perlin noise
        float[,] heightMap = GenerateNoiseMap(offsetX, offsetZ);

        // build a texture2d from the heightMap
        Texture2D tileTexture = BuildTexture(heightMap);
        meshRenderer.material.mainTexture = tileTexture;

        // move the vertices up according to the height map
        UpdateMesh(heightMap);
    }

    public float[,] GenerateNoiseMap(float offsetX, float offsetZ)
    {
        float[,] map = new float[depth, width];

        for (int z = 0; z < depth; z++) {
            for (int x = 0; x < width; x++) {
                float sampleX = (x + offsetX) / terrainGenerator.noiseScale;
                float sampleZ = (z + offsetZ) / terrainGenerator.noiseScale;
                
                float noise = 0f;
                float normalization = 0f;
                foreach(Wave wave in terrainGenerator.waves) {
                    noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + terrainGenerator.seed, sampleZ * wave.frequency + terrainGenerator.seed);
                    normalization += wave.amplitude;
                }
                noise /= normalization;
                
                map[z,x] = noise;
            }
        }

        return map;
    }

    private Texture2D BuildTexture(float[,] heightMap) {
        Color[] colorMap = new Color[depth * width];

        for (int z = 0; z < depth; z++) {
            for (int x = 0; x < width; x++) {
                // transform the 2D map index is an Array index
                int colorIndex = z * width + x;

                float height= heightMap[z, x];

                TerrainType terrainType = terrainGenerator.ChooseTerrainType(height);
                colorMap[colorIndex] = terrainType.color;
            }
        }

        // create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D (width, depth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels (colorMap);
        tileTexture.Apply ();

        return tileTexture;
    }

    private void UpdateMesh(float[,] heightMap)
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        int vertexIndex = 0;
        for (int z = 0; z < depth; z++) {
            for (int x = 0; x < width; x++) {
                float height = heightMap[z,x];

                Vector3 vertex = vertices[vertexIndex];
                vertices[vertexIndex] = new Vector3(vertex.x, height * terrainGenerator.heightMultiplier, vertex.z);

                vertexIndex++;
            }
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();

        meshCollider.sharedMesh = meshFilter.mesh;
    }

}
