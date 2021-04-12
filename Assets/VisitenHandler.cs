using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using UnityEngine.Networking;

public class VisitenHandler : MonoBehaviour
{

    public GameObject cardObject;
    public GameObject cardParent;
    public float cardHeight;
    

    void Start()
    {

        StartCoroutine(LoadAllRows());

    }

    IEnumerator LoadAllRows()

    {

        string url = "https://himmelsterrasse.byfive.at/dbConnect.php";



        WWWForm form = new WWWForm();

        form.AddField("method", "allRows");

        // Outputs URL
        Debug.Log(url);



        using (UnityWebRequest www = UnityWebRequest.Post(url, form))

        {

            yield return www.SendWebRequest();



            if (www.isNetworkError || www.isHttpError)

            {

                Debug.Log(www.error);

            }

            else

            {

                string result = www.downloadHandler.text;

                //Debug.Log(result);

                if (result[1] == '1')

                {

                    result = result.Substring(0, result.Length - 1);

                    string[] resultArray = result.Split('%');

                    foreach (string resultPerson in resultArray)

                    {

                        GameObject spawnCard = Instantiate(cardObject);

                        spawnCard.transform.position = new Vector3(Random.Range(-10f, 10f), cardHeight, Random.Range(-10f, 10f));

                        spawnCard.transform.SetParent(cardParent.transform);

                        spawnCard.SetActive(true);

                        string[] resultDetails = resultPerson.Split('|');
                        Transform canvasObject = spawnCard.transform.Find("Canvas").transform;

                        canvasObject.Find("Name").GetComponent<Text>().text = resultDetails[4] + " " + resultDetails[3] + " " + resultDetails[2];

                        string[] birthDates = resultDetails[5].Split('-');
                        canvasObject.Find("born Value").GetComponent<Text>().text = birthDates[2] + "." + birthDates[1] + "." + birthDates[0];
                        string[] deathDates = resultDetails[6].Split('-');
                        canvasObject.Find("death Value").GetComponent<Text>().text = deathDates[2] + "." + deathDates[1] + "." + deathDates[0];
                        canvasObject.Find("lifecity Value").GetComponent<Text>().text = resultDetails[13];

                        // Image cardImage = spawnCard.transform.Find("Canvas").transform.Find("Image").GetComponent<Image>();

                        //Debug.Log("Imagename: " + resultDetails[15]);

                        //StartCoroutine(DownloadImage(resultArray[15], cardImage)); 

                    }

                }

                else

                {

                    Debug.Log("Fehler Visitenkarte");

                }

            }

        }

    }

}
