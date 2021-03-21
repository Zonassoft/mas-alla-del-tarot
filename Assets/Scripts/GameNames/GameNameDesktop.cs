using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;
using TMPro;

[Serializable]
public class NameInfoPC
{
    public string detail;
}

public class GameNameDesktop : MonoBehaviour
{
    public GameObject panelShowing;
    public GameObject panelSex;
    public GameObject LoadingPanel;
    public GameObject buttonShowingSelectGender;
    public GameObject buttonRestart;
    public GameObject components;
    public GameObject buttonFemale;
    public GameObject buttonMale;
    public GameObject layoutName2;
    public GameObject Menu;
    public GameObject panelOptions;

    public GameObject particleLetters;
    
    public bool sexFemale;
    private bool stopNameLegth;
    private bool panelOptionActive;

    public TextMeshProUGUI nameUser;
    public TextMeshProUGUI description;
    
    public GameObject[] LettersList = new GameObject[26];
    public ParticleSystem[] particles = new ParticleSystem[26];
    public Material[] materialParticles = new Material[26];
    public Material[] materialLightParticles = new Material[26];
    private List<string> letterWrited = new List<string>();    // contiene las letras que han sido escritas
    
    public Button buttonFullScreen;
    public Button buttonMinimize;
    public Button buttonContinue;
    
    public Sprite buttonEnnable;
    public Sprite buttonDisable;
    
    private void Start()
    {
        particleLetters.gameObject.SetActive(true);
    }
    
    private void Update()
    {
        if (nameUser.text.Length > 1 && !stopNameLegth)
        {
            buttonContinue.gameObject.GetComponent<Image>().sprite = buttonEnnable;
            buttonContinue.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }

        if (nameUser.text.Length == 1)
        {
            buttonContinue.gameObject.GetComponent<Image>().sprite = buttonDisable;
            buttonContinue.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }

        if (!stopNameLegth)
        {
            letterWrited.Clear();
            
            for (int i = 0; i < nameUser.text.Length - 1; i++)
                letterWrited.Add(nameUser.text[i].ToString());
            
            ActivateLetter();
        }

        if (Screen.fullScreen)
        {
            buttonMinimize.gameObject.SetActive(true);
            buttonFullScreen.gameObject.SetActive(false);
        }
        else
        {
            buttonMinimize.gameObject.SetActive(false);
            buttonFullScreen.gameObject.SetActive(true);
        }
    }
    
    public void ActivateLetter()
    {
        if (letterWrited.Contains("A") || letterWrited.Contains("a") || letterWrited.Contains("á") || letterWrited.Contains("Á"))
            AuxLightLetter(0);
        else if (!letterWrited.Contains("A") && !letterWrited.Contains("a") && !letterWrited.Contains("á") && !letterWrited.Contains("Á"))
            AuxNonLightLetter(0);
        
        if (letterWrited.Contains("B") || letterWrited.Contains("b"))
            AuxLightLetter(1);
        else if (!letterWrited.Contains("B") && !letterWrited.Contains("b"))
            AuxNonLightLetter(1);
        
        if (letterWrited.Contains("C") || letterWrited.Contains("c"))
            AuxLightLetter(2);
        else if (!letterWrited.Contains("C") && !letterWrited.Contains("c"))
            AuxNonLightLetter(2);
        
        if (letterWrited.Contains("D") || letterWrited.Contains("d"))
            AuxLightLetter(3);
        else if (!letterWrited.Contains("D") && !letterWrited.Contains("d"))
            AuxNonLightLetter(3);
        
        if (letterWrited.Contains("E") || letterWrited.Contains("e") || letterWrited.Contains("é") || letterWrited.Contains("É"))
            AuxLightLetter(4);
        else if (!letterWrited.Contains("E") && !letterWrited.Contains("e") && !letterWrited.Contains("é") && !letterWrited.Contains("É"))
            AuxNonLightLetter(4);
        
        if (letterWrited.Contains("F") || letterWrited.Contains("f"))
            AuxLightLetter(5);
        else if (!letterWrited.Contains("F") && !letterWrited.Contains("f"))
            AuxNonLightLetter(5);
        
        if (letterWrited.Contains("G") || letterWrited.Contains("g"))
            AuxLightLetter(6);
        else if (!letterWrited.Contains("G") && !letterWrited.Contains("g"))
            AuxNonLightLetter(6);
        
        if (letterWrited.Contains("H") || letterWrited.Contains("h"))
            AuxLightLetter(7);
        else if (!letterWrited.Contains("H") && !letterWrited.Contains("h"))
            AuxNonLightLetter(7);
        
        if (letterWrited.Contains("I") || letterWrited.Contains("i") || letterWrited.Contains("í") || letterWrited.Contains("Í"))
            AuxLightLetter(8);
        else if (!letterWrited.Contains("I") && !letterWrited.Contains("i") && !letterWrited.Contains("í") && !letterWrited.Contains("Í"))
            AuxNonLightLetter(8);
        
        if (letterWrited.Contains("J") || letterWrited.Contains("j"))
            AuxLightLetter(9);
        else if (!letterWrited.Contains("J") && !letterWrited.Contains("j"))
            AuxNonLightLetter(9);
        
        if (letterWrited.Contains("K") || letterWrited.Contains("k"))
            AuxLightLetter(10);
        else if (!letterWrited.Contains("K") && !letterWrited.Contains("k"))
            AuxNonLightLetter(10);
        
        if (letterWrited.Contains("L") || letterWrited.Contains("l"))
            AuxLightLetter(11);
        else if (!letterWrited.Contains("L") && !letterWrited.Contains("l"))
            AuxNonLightLetter(11);
        
        if (letterWrited.Contains("M") || letterWrited.Contains("m"))
            AuxLightLetter(12);
        else if (!letterWrited.Contains("M") && !letterWrited.Contains("m"))
            AuxNonLightLetter(12);
        
        if (letterWrited.Contains("N") || letterWrited.Contains("n") || letterWrited.Contains("ñ") || letterWrited.Contains("Ñ"))
            AuxLightLetter(13);
        else if (!letterWrited.Contains("N") && !letterWrited.Contains("n") && !letterWrited.Contains("ñ") && !letterWrited.Contains("Ñ"))
            AuxNonLightLetter(13);
        
        if (letterWrited.Contains("O") || letterWrited.Contains("o") || letterWrited.Contains("ó") || letterWrited.Contains("Ó"))
            AuxLightLetter(14);
        else if (!letterWrited.Contains("O") && !letterWrited.Contains("o") && !letterWrited.Contains("ó") && !letterWrited.Contains("Ó"))
            AuxNonLightLetter(14);
        
        if (letterWrited.Contains("P") || letterWrited.Contains("p"))
            AuxLightLetter(15);
        else if (!letterWrited.Contains("P") && !letterWrited.Contains("p"))
            AuxNonLightLetter(15);
        
        if (letterWrited.Contains("Q") || letterWrited.Contains("q"))
            AuxLightLetter(16);
        else if (!letterWrited.Contains("Q") && !letterWrited.Contains("q"))
            AuxNonLightLetter(16);
        
        if (letterWrited.Contains("R") || letterWrited.Contains("r"))
            AuxLightLetter(17);
        else if (!letterWrited.Contains("R") && !letterWrited.Contains("r"))
            AuxNonLightLetter(17);
        
        if (letterWrited.Contains("S") || letterWrited.Contains("s"))
            AuxLightLetter(18);
        else if (!letterWrited.Contains("S") && !letterWrited.Contains("s"))
            AuxNonLightLetter(18);
        
        if (letterWrited.Contains("T") || letterWrited.Contains("t"))
            AuxLightLetter(19);
        else if (!letterWrited.Contains("T") && !letterWrited.Contains("t"))
            AuxNonLightLetter(19);
        
        if (letterWrited.Contains("U") || letterWrited.Contains("u") || letterWrited.Contains("ú") || letterWrited.Contains("Ú"))
            AuxLightLetter(20);
        else if (!letterWrited.Contains("U") && !letterWrited.Contains("u") && !letterWrited.Contains("ú") && !letterWrited.Contains("Ú"))
            AuxNonLightLetter(20);
        
        if (letterWrited.Contains("V") || letterWrited.Contains("v"))
            AuxLightLetter(21);
        else if (!letterWrited.Contains("V") && !letterWrited.Contains("v"))
            AuxNonLightLetter(21);
        
        if (letterWrited.Contains("W") || letterWrited.Contains("w"))
            AuxLightLetter(22);
        else if (!letterWrited.Contains("W") && !letterWrited.Contains("w"))
            AuxNonLightLetter(22);
        
        if (letterWrited.Contains("X") || letterWrited.Contains("x"))
            AuxLightLetter(23);
        else if (!letterWrited.Contains("X") && !letterWrited.Contains("x"))
            AuxNonLightLetter(23);
        
        if (letterWrited.Contains("Y") || letterWrited.Contains("y"))
            AuxLightLetter(24);
        else if (!letterWrited.Contains("Y") && !letterWrited.Contains("y"))
            AuxNonLightLetter(24);
        
        if (letterWrited.Contains("Z") || letterWrited.Contains("z"))
            AuxLightLetter(25);
        else if (!letterWrited.Contains("Z") && !letterWrited.Contains("z"))
            AuxNonLightLetter(25);
    }

    public void AuxLightLetter(int pos)
    {
        particles[pos].gameObject.GetComponent<ParticleSystemRenderer>().material = materialLightParticles[pos];
    }
    
    public void AuxNonLightLetter(int pos)
    {
        particles[pos].gameObject.GetComponent<ParticleSystemRenderer>().material = materialParticles[pos];
    }

    public void ButtonShowing()
    {
        if (nameUser.text.Length != 1)
        {
            stopNameLegth = true;
            panelSex.SetActive(true);
            particleLetters.SetActive(false);
            CreateName();
        }
        else
        {
            StartCoroutine(Message());
        }
    }

    public IEnumerator Message()
    {
        GameObject msgButton = buttonContinue.gameObject.transform.GetChild(1).gameObject;
        msgButton.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        msgButton.SetActive(false);
    }

    public void CreateName()
    {
        for (int i = 0; i < nameUser.text.Length - 1; i++)
        {
            if (nameUser.text[i].ToString() == "A" || nameUser.text[i].ToString() == "a" || nameUser.text[i].ToString() == "á" || nameUser.text[i].ToString() == "Á")
                AuxCreateLetter(0);
            if (nameUser.text[i].ToString() == "B" || nameUser.text[i].ToString() == "b")
                AuxCreateLetter(1);
            if (nameUser.text[i].ToString() == "C" || nameUser.text[i].ToString() == "c")
                AuxCreateLetter(2);
            if (nameUser.text[i].ToString() == "D" || nameUser.text[i].ToString() == "d")
                AuxCreateLetter(3);
            if (nameUser.text[i].ToString() == "E" || nameUser.text[i].ToString() == "e" || nameUser.text[i].ToString() == "é" || nameUser.text[i].ToString() == "É")
                AuxCreateLetter(4);
            if (nameUser.text[i].ToString() == "F" || nameUser.text[i].ToString() == "f")
                AuxCreateLetter(5);
            if (nameUser.text[i].ToString() == "G" || nameUser.text[i].ToString() == "g")
                AuxCreateLetter(6);
            if (nameUser.text[i].ToString() == "H" || nameUser.text[i].ToString() == "h")
                AuxCreateLetter(7);
            if (nameUser.text[i].ToString() == "I" || nameUser.text[i].ToString() == "i" || nameUser.text[i].ToString() == "í" || nameUser.text[i].ToString() == "Í") 
                AuxCreateLetter(8);
            if (nameUser.text[i].ToString() == "J" || nameUser.text[i].ToString() == "j")
                AuxCreateLetter(9);
            if (nameUser.text[i].ToString() == "K" || nameUser.text[i].ToString() == "k")
                AuxCreateLetter(10);
            if (nameUser.text[i].ToString() == "L" || nameUser.text[i].ToString() == "l")
                AuxCreateLetter(11);
            if (nameUser.text[i].ToString() == "M" || nameUser.text[i].ToString() == "m")
                AuxCreateLetter(12);
            if (nameUser.text[i].ToString() == "N" || nameUser.text[i].ToString() == "n" || nameUser.text[i].ToString() == "ñ" || nameUser.text[i].ToString() == "Ñ")
                AuxCreateLetter(13);
            if (nameUser.text[i].ToString() == "O" || nameUser.text[i].ToString() == "o" || nameUser.text[i].ToString() == "ó" || nameUser.text[i].ToString() == "Ó")
                AuxCreateLetter(14);
            if (nameUser.text[i].ToString() == "P" || nameUser.text[i].ToString() == "p")
                AuxCreateLetter(15);
            if (nameUser.text[i].ToString() == "Q" || nameUser.text[i].ToString() == "q")
                AuxCreateLetter(16);
            if (nameUser.text[i].ToString() == "R" || nameUser.text[i].ToString() == "r")
                AuxCreateLetter(17);
            if (nameUser.text[i].ToString() == "S" || nameUser.text[i].ToString() == "s")
                AuxCreateLetter(18);
            if (nameUser.text[i].ToString() == "T" || nameUser.text[i].ToString() == "t")
                AuxCreateLetter(19);
            if (nameUser.text[i].ToString() == "U" || nameUser.text[i].ToString() == "u" || nameUser.text[i].ToString() == "ú" || nameUser.text[i].ToString() == "Ú")
                AuxCreateLetter(20);
            if (nameUser.text[i].ToString() == "V" || nameUser.text[i].ToString() == "v")
                AuxCreateLetter(21);
            if (nameUser.text[i].ToString() == "W" || nameUser.text[i].ToString() == "w")
                AuxCreateLetter(22);
            if (nameUser.text[i].ToString() == "X" || nameUser.text[i].ToString() == "x")
                AuxCreateLetter(23);
            if (nameUser.text[i].ToString() == "Y" || nameUser.text[i].ToString() == "y")
                AuxCreateLetter(24);
            if (nameUser.text[i].ToString() == "Z" || nameUser.text[i].ToString() == "z")
                AuxCreateLetter(25);
        }
    }
    
    public void AuxCreateLetter(int pos)
    {
        GameObject newLetter2 = Instantiate(LettersList[pos], layoutName2.transform.position, layoutName2.transform.rotation) as GameObject;
        newLetter2.transform.SetParent(layoutName2.transform);
    }

    public void ButtonSexFemale()
    {
        GameObject borderNoSelected = buttonMale.transform.GetChild(2).gameObject;
        borderNoSelected.SetActive(false);
        GameObject borderSelected = buttonFemale.transform.GetChild(2).gameObject;
        borderSelected.SetActive(true);
        
        sexFemale = true;
        buttonShowingSelectGender.SetActive(true);
    }
    
    public void ButtonSexMale()
    {
        GameObject borderNoSelected = buttonFemale.transform.GetChild(2).gameObject;
        borderNoSelected.SetActive(false);
        GameObject borderSelected = buttonMale.transform.GetChild(2).gameObject;
        borderSelected.SetActive(true);
        
        sexFemale = false;
        buttonShowingSelectGender.SetActive(true);
    }

    public void ButtonShowing2()
    {
        LoadingPanel.SetActive(true);
        
        if(sexFemale)
            StartCoroutine(GetNameDescription("Femenino"));
        else
            StartCoroutine(GetNameDescription("Masculino"));
    }
    
    public IEnumerator GetNameDescription(string gender)
    {
        string nameT = nameUser.text.Remove(nameUser.text.Length - 1);
        
        WWWForm form = new WWWForm();
        form.AddField("name", nameT);
        form.AddField("gender", gender);
        
        UnityWebRequest req = UnityWebRequest.Post(SoundUi.Instance.urlName, form);
        req.SetRequestHeader("Authorization", "Token " + SoundUi.Instance.TokenAPI);
        
        yield return req.SendWebRequest();
        
        if (req.result == UnityWebRequest.Result.ProtocolError || req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            string jsonString = req.downloadHandler.text;
            NameInfoPC dataDescription = JsonUtility.FromJson<NameInfoPC>(jsonString);
            string descriptionOk = dataDescription.detail;
            
            if (descriptionOk.Contains("\u2028"))
                descriptionOk = descriptionOk.Replace("\u2028", " ");

            description.text = descriptionOk;
            panelSex.SetActive(false);
            buttonRestart.SetActive(true);
            components.SetActive(false);
            LoadingPanel.SetActive(false);
            panelShowing.SetActive(true);
            SoundUi.Instance.PlaySound(3);
        }
    }
    
    public void ButtonOptions()
    {
        SoundUi.Instance.Options(panelOptions, Menu, "MenuInDesktop");
    }
    
    public void ButtonQuitOptions()
    {
        SoundUi.Instance.QuitOptions(panelOptions, Menu, "MenuOutDesktop");
    }

    public void StartScene(string nameScene)
    {
        SoundUi.Instance.StartScene(nameScene, Menu, "MenuOutDesktop");
    }
    
    public void Restart(string nameScene)
    {
        SceneManager.LoadScene	(nameScene);
    }
}
