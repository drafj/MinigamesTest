using GeneralsMiniGames;
using Org.BouncyCastle.Crypto.Macs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public static AnimalSpawner Instance;

    [SerializeField] private List<GameObject> animals = new List<GameObject>();
    [SerializeField] private List<GameObject> chickens = new List<GameObject>();
    [SerializeField] private List<Transform> posiblePositionsList = new List<Transform>();
    [SerializeField] private List<Transform> posibleDuckPositionsList = new List<Transform>();
    [SerializeField] private List<GameObject> animalsRemaining = new List<GameObject>();
    private Dictionary<Vector3, bool> posiblePositionsDic = new Dictionary<Vector3, bool>();
    private Dictionary<Vector3, bool> posibleDuckPositionsDic = new Dictionary<Vector3, bool>();
    private List<GameObject> actualAnimals = new List<GameObject>();
    public List<int> pastGoals = new List<int>();
    public List<GameObject> selectedAnimal = new List<GameObject>();

    public int distractorAnimalsDisplayed;
    public int diferentAnimalsDisplayed;
    public int simultaneousChicken;
    public float animalsDisplayTime = 4;
    public Coroutine spawnCoroutine;
    private bool diferentAnimals;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);

        ResetRemainAnimals();// al ser una lista aparte se pueden retirar los animales seleccionados para que no se repitan en posteriores retos y no perder la referencia a los animales usados al conservar la lista original de animales
        for (int i = 0; i < posiblePositionsList.Count; i++)
        {
            posiblePositionsDic.Add(posiblePositionsList[i].position, false);
        }
        for (int i = 0; i < posibleDuckPositionsList.Count; i++)
        {
            posibleDuckPositionsDic.Add(posibleDuckPositionsList[i].position, false);
        }
    }

    public void StartChallenge(int minGoal, int maxGoal, int simultaneousChicken, int distractorAnimals, int animalsDisplayTime, int limitTime, int diferentAnimalsDisplayed, bool diferentAnimals)
    {
        PointsManager.instance.goalPoints = Random.Range(minGoal, maxGoal);
        if (pastGoals.Count > 0)
        {
            bool originalGoal = false;
            while (!originalGoal)
            {
                originalGoal = true;
                for (int i = 0; i < pastGoals.Count; i++)
                {
                    if (pastGoals[i] == PointsManager.instance.goalPoints)
                    {
                        originalGoal = false;
                        PointsManager.instance.goalPoints = Random.Range(minGoal, maxGoal);
                    }
                }
            }
        }
        pastGoals.Add(PointsManager.instance.goalPoints);
        this.simultaneousChicken = simultaneousChicken;
        distractorAnimalsDisplayed = distractorAnimals;
        this.animalsDisplayTime = animalsDisplayTime;
        this.diferentAnimals = diferentAnimals;
        this.diferentAnimalsDisplayed = diferentAnimalsDisplayed;
        PanelCronometro.Instance.timeToCompleteChallenge = limitTime;
        PanelCronometro.Instance.OpenPanel();
        PanelCronometro.Instance.StartCronometro();

        SelectAnimalChallenge();
        spawnCoroutine = StartCoroutine(SpawnAnimals());
    }

    public void SelectAnimalChallenge()// selecciona un animal de la lista de animales faltantes
    {
        selectedAnimal.Clear();
        for (int i = 0; i < diferentAnimalsDisplayed; i++)
        {
            if (animalsRemaining.Count == 0)
                ResetRemainAnimals();
            int newSelectedAnimal = Random.Range(0, animalsRemaining.Count);
            selectedAnimal.Add(animalsRemaining[newSelectedAnimal]);
            animalsRemaining.RemoveAt(newSelectedAnimal);
        }
    }

    private IEnumerator SpawnAnimals()// corrutina que se encarga de ubicar los animales distractores en todo el reto
    {
        while (true)// solo funciona cuando se empieza el reto y al finalizarlo se termina para seleccionar el/los siguientes animales distractores
        {
            ClearPositions();
            DestroyActualAnimals();
            for (int i = 0; i < distractorAnimalsDisplayed; i++)// ubica la cantidad de animales simultaneos que deben aparecer dependiendo del nivel
            {
                if (diferentAnimals)
                    SpawnAnimal(selectedAnimal[i]);
                else
                    SpawnAnimal(selectedAnimal[0]);
            }
            for (int i = 0; i < simultaneousChicken; i++)// ubica la cantidad de animales simultaneos que deben aparecer dependiendo del nivel
            {
                SpawnAnimal(chickens[Random.Range(0, chickens.Count)]);
            }
            yield return new WaitForSeconds(animalsDisplayTime);
        }
    }

    private void SpawnAnimal(GameObject animal)// función encargada de instanciar los animales
    {
        if (animal.GetComponent<Animal>().type != AnimalType.duck)
        {
            int randPosition = Random.Range(0, posiblePositionsDic.Count);
            Vector3 dictionaryKey = posiblePositionsDic.ElementAt(randPosition).Key;
            int exitCounter = 0;
            while (posiblePositionsDic[dictionaryKey] && exitCounter < 50)// me aseguro de que ese lugar no esté ocupado
            {
                exitCounter++;
                randPosition = Random.Range(0, posiblePositionsDic.Count);
                dictionaryKey = posiblePositionsDic.ElementAt(randPosition).Key;
            }

            GameObject actualAnimal = Instantiate(animal, dictionaryKey, Quaternion.identity);
            actualAnimals.Add(actualAnimal);

            posiblePositionsDic[dictionaryKey] = true;// pongo ese lugar verdadero para que no se pueda ocupar un mismo lugar dos veces
        }
        else if (animal.GetComponent<Animal>().type == AnimalType.duck)
        {
            int randPosition = Random.Range(0, posibleDuckPositionsDic.Count);
            Vector3 dictionaryKey = posibleDuckPositionsDic.ElementAt(randPosition).Key;
            int exitCounter = 0;
            while (posibleDuckPositionsDic[dictionaryKey] && exitCounter < 50)// me aseguro de que ese lugar no esté ocupado
            {
                exitCounter++;
                randPosition = Random.Range(0, posibleDuckPositionsDic.Count);
                dictionaryKey = posibleDuckPositionsDic.ElementAt(randPosition).Key;
            }

            GameObject actualAnimal = Instantiate(animal, dictionaryKey, Quaternion.identity);
            actualAnimals.Add(actualAnimal);

            posibleDuckPositionsDic[dictionaryKey] = true;// pongo ese lugar verdadero para que no se pueda ocupar un mismo lugar dos veces
        }
    }

    private void DestroyActualAnimals()// función que destruye todos los animales acutales
    {
        for (int i = 0; i < actualAnimals.Count; i++)
        {
            Destroy(actualAnimals[i]);
        }
        actualAnimals.Clear();
    }

    public void RemoveSpecificAnimal(GameObject animal)
    {
        if (actualAnimals.Contains(animal))
        actualAnimals.Remove(animal);
    }

    public void ClearPositions()// función que pone todas las posiciones en false para que se puedan ocupar nuevamente
    {
        for (int i = 0; i < posiblePositionsList.Count; i++)
        {
            posiblePositionsDic[posiblePositionsList[i].position] = false;
        }
        for (int i = 0; i < posibleDuckPositionsList.Count; i++)
        {
            posibleDuckPositionsDic[posibleDuckPositionsList[i].position] = false;
        }
    }

    public void ResetRemainAnimals()
    {
        animalsRemaining.Clear();
        for (int i = 0; i < animals.Count; i++)
        {
            animalsRemaining.Add(animals[i]);
        }
    }

    public void ResetAnimalSpawner()
    {
        DestroyActualAnimals();
        ClearPositions();
        if (spawnCoroutine != null)
        StopCoroutine(spawnCoroutine);
    }
}
