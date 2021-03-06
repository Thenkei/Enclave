﻿// Script to link to a Gemstone of the Main Menu
// 
// Each Gemstone had the following componants : Light, Ellipsoid Particle Emiter, Particle Emiter,Particle Animator, Particle Renderer 
// 
// Must be present on the scene : GUI Text (already configurate)
// 
// Role :
//		-Looping the glowing/pulsing effect of the object
//		-Change the light color when hovered
//		-Star the particle when hovered / Stop it when it's not
//		-Display the text of the menu
// 
// Sources : 
//		- http://docs.unity3d.com/Documentation/ScriptReference/ParticleEmitter.html
//		- http://docs.unity3d.com/Documentation/ScriptReference/ParticleAnimator.html
//		- http://docs.unity3d.com/Documentation/ScriptReference/Light.html
//		- http://answers.unity3d.com/questions/41931/how-to-randomly-change-the-intensity-of-a-point-li.html
//		- http://docs.unity3d.com/Documentation/ScriptReference/GUIText.html
//
// Created by Ludivine Barast
// Version 2.0

using UnityEngine;
using System.Collections;

public class WhiteGem : MonoBehaviour 
{
	/*		MEMBERS			*/
	
	//Déclatation des composant des Particules
	private ParticleAnimator GemPartAnimation;
	private ParticleEmitter GemPartEmitter;
	private Color InitialColor;
	private Color OppositeColor;
	
	//Déclaration du tableau de Couleur des particules
	private Color[] GemPartStarkle;
	
	//Déclaration de la lumière
	private Light GemLight;																				
	
	//Membres pour l'effet lumineux
	private float minIntensity;
	private float maxIntensity;
	private float randomIntensity;


	//Texte
	private TextMesh GemTitle;
	
	//Changement de scene
	public bool ChangeMode;

	//Scripts
	private BlueGem Blue;
	private VioletGem Violet;
	private YellowGem Yellow;
	private BlackGem Black;
	private RedGem Red;
	
	/*			METHODES			*/
	
	//Constructor
	void Awake()
	{
		//Lien avec les composants des particules
		GemPartEmitter = GetComponent<ParticleEmitter>();												//Lien avec l'Emitter des particules
		GemPartAnimation = GetComponent<ParticleAnimator>();											//Lien avec l'Animator des particules
		InitialColor = Color.white;																		//Définition de la couleur initiale de la Gemme
		OppositeColor = Color.black;																	//Définition de la couleur opposé de la Gemme
		
		
		//Initialisation du tableau dynamiquement
		GemPartStarkle = new Color[5];																	//Initialisation des 5 couleurs comosant le Renderer
		
		//Lien avec la Lumière
		GemLight = GetComponent<Light>();
		
		GemTitle = GameObject.Find("Title").GetComponent<TextMesh>();									//Lien avec le GUIText de la scène
		
		
		//Initialisation des statuts
		GemPartEmitter.emit = false;																	//Dissimulation des particules
		GemTitle.renderer.enabled = false;																//Dissimulation du texte
	
		
		ChangeMode = false;
																										//Récuparation des scripts
		Blue = GameObject.FindGameObjectWithTag("Blue").GetComponent<BlueGem>();
		Violet = GameObject.FindGameObjectWithTag("Violet").GetComponent<VioletGem>();
		Yellow = GameObject.FindGameObjectWithTag("Yellow").GetComponent<YellowGem>();
		Black = GameObject.FindGameObjectWithTag("Black").GetComponent<BlackGem>();
		Red = GameObject.FindGameObjectWithTag("Red").GetComponent<RedGem>();
	}

	// Use this for initialization
	void Start () 
	{
		//Configuration de l'Emetteur
		GemPartEmitter.minSize = 0.1f;																	//Taille minimale de la particule émise
		GemPartEmitter.maxSize = 0.2f;																	//Taille maximale de la particule émise
		GemPartEmitter.minEnergy = 3.0f;
		GemPartEmitter.maxEnergy = 3.0f;
		GemPartEmitter.minEmission = 15.0f;																//Nombre minimale de particules émisent
		GemPartEmitter.maxEmission = 25.0f;																//Nombre maximale de particules émisent
		GemPartEmitter.worldVelocity = new Vector3(0.0f,1.1f,0.0f);										//Vitesse d'émission des particules (x,y,z) ; "Distance de projection"
		GemPartEmitter.rndVelocity = new Vector3(0.8f,0.0f,0.0f);										//Angle d'émission des particules (x,y,z)
		
		//Configuration de l'Animateur
		GemPartAnimation.sizeGrow = 0.15f;																//Définition de la taille des particule au cours du temps ; "Assez petit, peut etre modifié"
		
		
		//Définition des 5 couleurs et application
		GemPartStarkle[0] = new Color32(0,0,0,10);														
		GemPartStarkle[1] = new Color32(0,0,0,180);
		GemPartStarkle[2] = new Color32(0,0,0,200);
		GemPartStarkle[3] = new Color32(0,0,0,180);
		GemPartStarkle[4] = new Color32(0,0,0,10);
		GemPartAnimation.colorAnimation = GemPartStarkle;												//Assignation du tableau a l'ensemble des couleurs
		
		
		//Configuration de la lumière
		GemLight.type = LightType.Spot;																	//Type de lumière
		GemLight.range = 2.0f;																			//Portée de la lumière
		GemLight.spotAngle = 20;																		//Angle de vue de la lumière
		GemLight.color = InitialColor;																	//Couleur de la lumière
		
		//Configuration des effets de lumière
		minIntensity = 0.2f;																			//Intensité minimale
		maxIntensity = 0.6f;																			//Intensité maximale
		randomIntensity = Random.Range(0.0f, 600.0f);													//Tirage aléatoire d'un float pour l'intensité
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!ChangeMode)
		{
			//Effet de lumière
			float noise = Mathf.PerlinNoise(randomIntensity, Time.time);
        	GemLight.intensity = Mathf.Lerp(minIntensity, maxIntensity,noise);							//La palpitation !!
		}
		
		if(ChangeMode)
		{
			gameObject.renderer.enabled = false;
			Blue.ChangeMode = true;
			Violet.ChangeMode = true;
			Yellow.ChangeMode = true;
			Black.ChangeMode = true;
			Red.ChangeMode = true;
			GemPartEmitter.emit = false;															
			GemPartEmitter.ClearParticles();
			GemLight.enabled = false;
		}
	}
	
	
	void OnMouseOver()
	{
		if(!ChangeMode)
		{
			GemTitle.text = "Quitter";																	//Initilaisation du texte de la Gemme
			GemLight.color = OppositeColor;																//Changement de couleur
			GemPartEmitter.emit = true;																	//Lancement des particules
			GemTitle.renderer.enabled = true;															//Affichage du text
		}																					
	}
	
	void OnMouseExit()
	{
		if(!ChangeMode)
		{
			GemLight.color = InitialColor;																//Changement de couleur
			GemPartEmitter.emit = false;																//Arret des particules
			GemPartEmitter.ClearParticles();															//Suppresion des particules restantes sur l'écran
			GemTitle.renderer.enabled = false;															//Dissimulation du text
		}
	}
	
	void OnMouseDown()
	{
		ChangeMode = true;
	}
}