using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class CubeEditor : EditorWindow
{
    private GameObject cubePrefab;

    private GameObject spawnedCube;
    private MeshRenderer cubeMeshRenderer;
    private Color cubeColor;

    private List<GameObject> spawnedCubes = new();


    [MenuItem("Window/Cube Editor")]
    public static void ShowExample()
    {
        CubeEditor wnd = GetWindow<CubeEditor>();
        wnd.titleContent = new GUIContent("Cube Editor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        /*// VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);*/

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CubeEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        var cubePrefabObjectField = root.Q<ObjectField>("CubePrefabObjectField");
        cubePrefabObjectField.objectType = typeof(GameObject);
        cubePrefabObjectField.generateVisualContent += (MeshGenerationContext ctx) => cubePrefab = cubePrefabObjectField.value as GameObject;

        var spawnCubeButton = root.Q<Button>("SpawnCubeButton");
        spawnCubeButton.clicked += SpawnCube;

        var deleteCubeButton = root.Q<Button>("DeleteCubeButton");
        deleteCubeButton.clicked += DeleteCube;

        var cubeRednessSlider = root.Q<Slider>("CubeRednessSlider");
        cubeRednessSlider.RegisterValueChangedCallback((ChangeEvent<float> evt) =>
            cubeMeshRenderer.sharedMaterial.color = new Color(cubeRednessSlider.value, cubeColor.g, cubeColor.b));

        var hideCubeToggle = root.Q<Toggle>("HideCubeToggle");
        hideCubeToggle.RegisterValueChangedCallback((ChangeEvent<bool> evt) => spawnedCube.SetActive(!evt.newValue));

        var deleteAllCubesButton = root.Q<Button>("DeleteAllCubesButton");
        deleteAllCubesButton.clicked += () => spawnedCubes.ForEach(DestroyImmediate);
    }

    private void SpawnCube()
    {
        spawnedCube = Instantiate(cubePrefab, oezyowen.Utils.RandomRangeVector3(-10, 10, -5, 5, -10, 10), Quaternion.identity);
        //spawnedCube = PrefabUtility.InstantiatePrefab(cubePrefab) as GameObject;
        //spawnedCube.transform.position = oezyowen.Utils.RandomRangeVector3(-10, 10, -5, 5, -10, 10);
        cubeMeshRenderer = spawnedCube.GetComponent<MeshRenderer>();
        cubeColor = cubeMeshRenderer.sharedMaterial.color;

        spawnedCubes.Add(spawnedCube);
    }

    private void DeleteCube()
    {
        spawnedCubes.Remove(spawnedCube);
        DestroyImmediate(spawnedCube);
    }
}